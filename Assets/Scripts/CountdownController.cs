using UnityEngine;
using TMPro;

public class CountdownController : MonoBehaviour
{
    public int Seconds = 0, Minutes = 2;

    private bool _isValid = false;
    private GameController _gameControllerRef;
    private TextMeshProUGUI _timerText;


    /* Get Reference for Game Controller & TextMeshProUGUI */
    void Awake()
    {
        _gameControllerRef = GameObject.FindGameObjectWithTag("GameController")
        .GetComponent<GameController>();

        _timerText = GetComponentInChildren<TextMeshProUGUI>();

        if (_timerText == null || _gameControllerRef == null) {
            Debug.LogError("Error Finding Game References");
            Debug.Break();
        }

        //Poll every second...
        InvokeRepeating("Poll", 1f, 1f);
    }

    void Poll()
    {
        CountDown();
        TimeFormatting();
    }


    private void CountDown() {
        if (!_isValid) {
            if (Seconds == 0) {
                if (Minutes == 0) {
                    _isValid = true;
                    _gameControllerRef.SayGameOver();
                } else {
                    Seconds = 59;
                    Minutes -= 1;
                }
            } else {
                 Seconds -= 1;
            }
        }
    }

    private void TimeFormatting() {
        string minuteText = "", secondText = "";

        if (Minutes < 10) minuteText = "0" + Minutes; else minuteText = Minutes.ToString();

        if (Seconds < 10) secondText = "0" + Seconds; else secondText = Seconds.ToString();

        _timerText.text = $"{minuteText}:{secondText}";
    }
}
