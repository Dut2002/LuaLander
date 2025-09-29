using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] private int leverNumber;
    [SerializeField] private Transform landerStartPosition;
    [SerializeField] private Transform cameraStartPosition;
    [SerializeField] private float zoomOutOthorgrahicSize;
    public int GetLevel() => leverNumber;
    public Vector3 GetLanderStartPosition() => landerStartPosition.position;
    public Transform GetCameraStartPositionTransform() => cameraStartPosition;
    public float GetZoomOutOthorgrahicSize() => zoomOutOthorgrahicSize;
}
