using UnityEngine;
using UnityEngine.UI;

public class Temp : MonoBehaviour
{
    public int playerScore;
    public Text scoreText;

    [ContextMenu("Increase score")]
    public void addScore(int score)
    {
        playerScore += score;
        scoreText.text = playerScore.ToString();
    }
}
