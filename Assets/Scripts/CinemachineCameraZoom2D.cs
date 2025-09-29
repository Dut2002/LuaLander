using Unity.Cinemachine;
using UnityEngine;

public class CinemachineCameraZoom2D : MonoBehaviour
{
    private const float NORMAL_ORTHOGRAPHIC_SIZE = 15f;
  
    public static CinemachineCameraZoom2D Instance;

    [SerializeField] private CinemachineCamera cinemachineCamera;

    private float targetOrthographicSize = 10f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        float zoomSpeed = 2f;
        cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(cinemachineCamera.Lens.OrthographicSize, targetOrthographicSize, zoomSpeed * Time.deltaTime);
    }

    public void SetTargetOrthographicSize(float targetOrthographicSize)
    {
        this.targetOrthographicSize = targetOrthographicSize;
    }

    public void SetTargetOrthographicSizeNormal()
    {
        this.targetOrthographicSize = NORMAL_ORTHOGRAPHIC_SIZE;
    }
}
