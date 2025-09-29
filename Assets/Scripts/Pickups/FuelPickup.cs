using UnityEngine;

public class FuelPickup : MonoBehaviour
{
    [SerializeField] private float fuelAmount = 10f;

    public float GetFuelAmount() => fuelAmount;
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
