using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    public static Lander Instance { get; private set; }

    private const float GRAVITY_NORMAL = 0.7f;

    public event EventHandler OnUpForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs: EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs: EventArgs
    {
        public State state;
    }

    public enum LandingType
    {
        Success,
        WrongLanding,
        TooSteepAngle,
        TooFastLanding
    }

    public enum State
    {
        Waiting,
        Normal,
        GameOver
    }

    private Rigidbody2D landerRigidbody2D;
    private float fuelAmount;
    private float fuelAmountMax = 20f;
    private float fuelComsumptionAmount = 1f;
    private State state;

    private void Awake()
    {
        Instance = this;
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        landerRigidbody2D.gravityScale = 0f;
        fuelAmount = fuelAmountMax;
        state = State.Waiting;
    }

    private void FixedUpdate()
    {
               

        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            default:
            case State.Waiting:
                if (GameInput.Instance.IsUpActionPressed()
                    || GameInput.Instance.IsRightActionPressed()
                    || GameInput.Instance.IsLeftActionPressed()
                    || GameInput.Instance.GetMovementInputVector() != Vector2.zero)
                {
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    SetState(State.Normal);

                }
                break;
            case State.Normal:
                if (fuelAmount <= 0f)
                {
                    //Hết xăng
                    return;
                }

                if (GameInput.Instance.IsUpActionPressed()
                    || GameInput.Instance.IsRightActionPressed()
                    || GameInput.Instance.IsLeftActionPressed()
                    || GameInput.Instance.GetMovementInputVector() != Vector2.zero)
                {
                    ConsumeFuel();
                }
                float gamepadDeadzone = .6f;
                if (GameInput.Instance.IsUpActionPressed() || GameInput.Instance.GetMovementInputVector().y > gamepadDeadzone)
                {
                    float force = 700f;
                    landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }
                if (GameInput.Instance.IsLeftActionPressed() || GameInput.Instance.GetMovementInputVector().x < -gamepadDeadzone)
                {
                    float turnspeed = +100f;
                    landerRigidbody2D.AddTorque(turnspeed * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }
                if (GameInput.Instance.IsRightActionPressed() || GameInput.Instance.GetMovementInputVector().x > gamepadDeadzone)
                {
                    float turnspeed = -100f;
                    landerRigidbody2D.AddTorque(turnspeed * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }
                break;

            case State.GameOver:
                break;
        }

        
    }
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if(!collision2D.gameObject.TryGetComponent(out LandingPad landingPad))
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.WrongLanding,
                dotVector = 0f,
                landingSpeed = 0,
                scoreMultiplier = 0,
                score = 0,
            });
            // đáp sai vị trí sân đáp
            SetState(State.GameOver);
            return;
        }

        float softLandingVelocityMagnitude = 4f;
        float relativeVelocityMagnitude = collision2D.relativeVelocity.magnitude;

        if ( relativeVelocityMagnitude > softLandingVelocityMagnitude)
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFastLanding,
                dotVector = 0f,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0,
            });
            //Đáp đất quá mạnh
            SetState(State.GameOver);
            return;
        }

        float dotVector =  Vector2.Dot(Vector2.up, transform.up);
        float minDotVector = .90f;
        if (dotVector < minDotVector)
        {
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                dotVector = dotVector,
                landingSpeed = relativeVelocityMagnitude,
                scoreMultiplier = 0,
                score = 0,
            });
            //Góc độ đáp quá nghiêng
            SetState(State.GameOver);
            return;
        }

        //đáp thành công
        float maxScoreAmountLandingAngle = 100;
        float scoreDotVectorMultiplier = 10f;
        float landingAngleScore = maxScoreAmountLandingAngle - Mathf.Abs(dotVector - 1f) * scoreDotVectorMultiplier * maxScoreAmountLandingAngle;

        float maxScoreAmountLandingSpeed = 100;
        float landingSpeedScore = (softLandingVelocityMagnitude - relativeVelocityMagnitude) * maxScoreAmountLandingSpeed;
        float scoreMultiplier = landingPad.GetScoreMultiplier();
        int score = Mathf.RoundToInt((landingAngleScore +  landingSpeedScore) * scoreMultiplier);

        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            landingType = LandingType.Success,
            dotVector = dotVector,
            landingSpeed = relativeVelocityMagnitude,
            scoreMultiplier = scoreMultiplier,
            score = score,
        });
        SetState(State.GameOver);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.TryGetComponent(out FuelPickup fuelPickup)){
            fuelAmount = Mathf.Min(fuelAmount + fuelPickup.GetFuelAmount(), fuelAmountMax);
            fuelPickup.DestroySelf();
        }

        if (collider2D.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }
    }

    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state,
        });
    }

    private void ConsumeFuel()
    {
        fuelAmount -= fuelComsumptionAmount * Time.deltaTime;
    }

    public float GetSpeed() => landerRigidbody2D.linearVelocity.magnitude;
    public Vector2 GetVelocity() => landerRigidbody2D.linearVelocity;
    public float GetFuel() => fuelAmount;
    public float GetFuelNomalized() => fuelAmount/fuelAmountMax;

}
