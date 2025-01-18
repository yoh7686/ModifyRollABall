using System;
using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private float time;//カウントの値
    private bool isStopped;//trueならカウントを止める

    void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        ResetTimer();
        StartTimer();
    }

    void Update()
    {
        if (!isStopped)
        {
            // カウントアップして表示を更新
            time += Time.deltaTime;
            SetTimerText();
        }
    }
    //Timeをテキストに変換して表示
    private void SetTimerText()
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int centiseconds = (int)(time * 100) % 100;  // ミリ秒ではなくセンチ秒（1/100秒）を表示
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, centiseconds);
    }

    //タイマーをスタート
    public void StartTimer()
    {
        isStopped = false;
    }

    //タイマーを止める
    public void StopTimer()
    {
        isStopped = true;
    }

    //タイマーをリセットする
    public void ResetTimer()
    {
        time = 0f;
        SetTimerText();
    }

    //timeの値を返す
    public float GetTime()
    {
        return time;
    }
}
