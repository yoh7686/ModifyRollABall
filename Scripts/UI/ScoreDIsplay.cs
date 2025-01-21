using TMPro;
using UnityEngine;
using System.Collections;

public class ScoreDisplay : MonoBehaviour
{

    private TextMeshProUGUI scoreText;
    private int currentScore;//現在のスコア
    private int maxScore;//最大スコア（アイテムの数）
    private float currentTime;//カウントの値
    private bool isStopped;//trueならカウントを止める
    private int highScorePoint;
    private float highScoreTime;
    private string pointText;
    private string timeText;
    private string highScoreText;
    private GameSystem gameSystem;
    [SerializeField]int HighScoreTextSize=-1;

    void Awake()
    {
        isStopped = true;
        scoreText = GetComponent<TextMeshProUGUI>();
        scoreText.text = "";
        StartCoroutine(FindTarget());
        //PlayerPrefからハイスコア読み込み
        highScorePoint = PlayerPrefs.GetInt("HighScorePoint",0);
        highScoreTime = PlayerPrefs.GetFloat("HighScoreTime",0);
        //ハイスコアの記録があれば表示
        if(highScoreTime!=0){
            SetHighScore(highScorePoint,highScoreTime);
        }else{
            highScoreText = "";
        }
    }
    //GameSystemを探すコルーチン
    IEnumerator FindTarget()
    {
        GameObject target = null;
        while (target == null)
        {
            target = GameObject.FindWithTag("GameController");
            if (target == null)
            {
                yield return null;  // 1フレーム待機し次のフレームに処理を移行
            }
        }
        gameSystem = target.GetComponent<GameSystem>();
        gameSystem.SetScoreDisplay(this);
    }
    void Start(){
        timeText = SetTimerText(currentTime);
        UpdateScoreDisplay();
    }

    void Update()
    {
        if (!isStopped)
        {
            // カウントアップして表示を更新
            currentTime += Time.deltaTime;
            timeText = SetTimerText(currentTime);
            UpdateScoreDisplay();
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            gameSystem.onRetryButtonClick();
        }
    }
    //表示の更新
    public void UpdateScoreDisplay(){
        scoreText.text=highScoreText+"Score:"+pointText+" Time:"+timeText;
    }
    //スコアが更新された時に呼ぶ
    public void SetScore(int score)
    {
        currentScore = score;
        pointText=SetScoreText(currentScore,maxScore);
    }
    //スコア最大数のセット
    public void SetMaxScore(int score)
    {
        Debug.Log("SetMacScore");
        maxScore = score;
        pointText=SetScoreText(currentScore,maxScore);
        if(highScoreTime!=0)SetHighScore(highScorePoint,highScoreTime);
        UpdateScoreDisplay();
    }
    //ハイスコアのセット
    private void SetHighScore(int score, float time){
        highScoreText = "<size="+HighScoreTextSize.ToString()
        +">HighScore: "+SetScoreText(score,maxScore)+" "+SetTimerText(time)+"</size>\n";
    }
    //ハイスコア表示用文字列の更新
    public bool UpdateHighScoreText()
    {
        //ポイントまたはタイムが更新されていたらハイスコアの更新と表示
        if(highScoreTime==0 || currentScore>highScorePoint || (currentScore>=highScorePoint && currentTime<highScoreTime))
        {
            SetHighScore(currentScore,currentTime);
            PlayerPrefs.SetInt("HighScorePoint",currentScore);
            PlayerPrefs.SetFloat("HighScoreTime",currentTime);
            PlayerPrefs.Save();
            return true;
        }else
        {
            return false;
        }
    }
    //スコア表示用文字列の更新
    private string SetScoreText(int cs, int ms)
    {
        return string.Format("<mspace=0.6em>{0:00}</mspace>/<mspace=0.6em>{1:00}</mspace>", cs, ms);
    }

    //現在のスコア表示用文字列を返す
    public string GetCurrentScoreText()
    {
        return SetScoreText(currentScore, maxScore);
    }

    //Time表示用テキストの更新
    private string SetTimerText(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int centiseconds = (int)(time * 100) % 100;  // ミリ秒ではなくセンチ秒（1/100秒）を表示
        return string.Format("<mspace=0.6em>{0:00}</mspace>:<mspace=0.6em>{1:00}</mspace>:<mspace=0.6em>{2:00}</mspace>", minutes, seconds, centiseconds);
    }
    //現在のタイム表示用文字列を返す
    public string GetCurrentTimerText()
    {
        return SetTimerText(currentTime);
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
        currentTime = 0f;
        SetTimerText(currentTime);
    }
    //timeの値を返す
    public float GetTime()
    {
        return currentTime;
    }
}