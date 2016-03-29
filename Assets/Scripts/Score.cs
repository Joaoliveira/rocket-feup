using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour {
    int score;
    public Text scoreText;

    public void incrementScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
