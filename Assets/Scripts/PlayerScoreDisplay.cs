using TMPro;
using UnityEngine;

public class PlayerScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI timeDisplay;

    public void UpdateDisplay(string _name, string _score, string _time)
    {
        nameDisplay.text = _name;
        scoreDisplay.text = _score;
        timeDisplay.text = _time;
    }
}
