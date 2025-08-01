using System;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Transform player; // Player Transform 참조
    
    private int currScore;
    private int maxScore;
    
    private float currHeight;
    private float maxHeight;
    private int heightScoreMultiplier = 1; // 점수 변환 비율

    private void Update()
    {
        if (UIManager.Instance != null && !UIManager.Instance.IsGameStarted) 
            return;
        
        UpdateScore();
    }

    void UpdateScore()
    {
        currHeight = player.position.y;
        currScore = Mathf.FloorToInt(currHeight * heightScoreMultiplier);
        currScore = Mathf.Max(0, currScore); // 음수 방지

        if (currScore >= maxScore)
        {
            maxScore = currScore;
            PlayerPrefs.SetInt("MaxScore", currScore);
        }
        
        scoreText.text = currScore.ToString() + " m";
    }
}
