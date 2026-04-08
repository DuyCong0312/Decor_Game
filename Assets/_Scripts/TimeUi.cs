using TMPro;
using UnityEngine;

public class TimeUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float timeScale = 4f;

    private float currentTime = 0f;

    private void Update()
    {
        AdvanceTime();
        UpdateClockUI();
    }

    private void AdvanceTime()
    {
        currentTime += Time.deltaTime * timeScale / 3600f;

        if (currentTime >= 24f)
        {
            currentTime = 0f;
        }
    }

    private void UpdateClockUI()
    {
        int hours = Mathf.FloorToInt(currentTime);
        int minutes = Mathf.FloorToInt((currentTime - hours) * 60f);

        timeText.text = $"{hours:00}:{minutes:00}";
    }
}