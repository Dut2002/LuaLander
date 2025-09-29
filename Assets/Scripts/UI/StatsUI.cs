using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;
    [SerializeField] private RectTransform speedArrow;
    [SerializeField] private Image fuelImage;

    private void Update()
    {
        UpdateStatsTextMesh();
        UpdateSpeedArrow();
    }

    private void UpdateStatsTextMesh()
    {
        fuelImage.fillAmount = Lander.Instance.GetFuelNomalized();

        statsTextMesh.text =
            GameManager.Instance.GetLevelNumber() + "\n" +
            GameManager.Instance.GetScore() + "\n" +
            Mathf.RoundToInt(GameManager.Instance.GetTime()) + "\n" +
            Mathf.RoundToInt(Lander.Instance.GetSpeed()) + "\n0";
    }

    private void UpdateSpeedArrow()
    {
        if (Lander.Instance.GetSpeed() <= 0.1f)
        {
            speedArrow.gameObject.SetActive(false);
            return;
        }
        if(!speedArrow.gameObject.activeSelf) speedArrow.gameObject.SetActive(true);
        Vector2 v = Lander.Instance.GetVelocity();
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        float snapped = Mathf.Round(angle / 45f) * 45f;
        speedArrow.localEulerAngles = new Vector3(0, 0, snapped);
    }

}
