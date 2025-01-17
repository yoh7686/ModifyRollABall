using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{

    private TextMeshProUGUI scoreText;
    private int currentScore;//現在のスコア
    private int maxScore;//最大スコア（アイテムの数）

    void Awake()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        UpdateScoreText();//スコアの表示
    }

    //スコアが更新された時に呼ぶ
    public void SetScore(int score)
    {
        currentScore = score;
        UpdateScoreText();
    }

    //ハイスコアのセット
    public void SetMaxScore(int score)
    {
        maxScore = score;
    }

    //スコアを表示
    private void UpdateScoreText()
    {
        scoreText.text = string.Format("Score: {0:00}/{1:00}", currentScore, maxScore);
    }
}