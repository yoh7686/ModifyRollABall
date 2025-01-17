using UnityEngine;
using System.Collections;//コルーチンを使うために必要
using TMPro;//TextMeshProを使うために必要
using UnityEngine.SceneManagement;//シーンのロードのために必要

public class GameSystem : MonoBehaviour
{
    private PlayerMove playerMove;//プレイヤーのスクリプト
    private Item[] items;//アイテムのスクリプト
    private GateController[] gates;//ゲートのスクリプト
    private CameraFollow cameraFollow;//カメラのスクリプト
    [SerializeField] TextController textController;//メッセージ表示用スクリプト
    [SerializeField] TimerDisplay timerDisplay;//タイム表示用スクリプト
    [SerializeField] ScoreDisplay scoreDisplay;//スコア表示用スクリプト
    [SerializeField] TextMeshProUGUI highScoreDisplay;//ハイスコア表示用TMPro
    [SerializeField] AudioSource PickUpSE;//アイテム取得時の音を鳴らすAudioSourse
    [SerializeField] AudioSource HitSE;//Hazardコリジョン接触時の音を鳴らすAudioSourse
    [SerializeField] private string[] tutorialMessages = new string[3];//チュートリアルメッセージのテキスト
    [SerializeField] private bool IsClearOnAllItemsCollected=true;//全アイテム回収でゴールとするか否か
    private int score = 0;//スコアの値
    private Vector3 playerRespawnAt;//リスポーンポイントの座標
    private int highScorePoint;//ハイスコアのポイント
    private float highScoreTime;//ハイスコアのタイム
    private bool playerIsDead;//プレイヤーがHazardコリジョンに接触してからリスポーンするまでTrueになるフラグ 

    void Start()
    {
        //tutorialMessageの設定（入力しなくても動くよう設定。自身で書きたい方はこれを消してインスペクタに入れてください）
        tutorialMessages[0]="AWSD or ←↑→↓\nto Move";
        tutorialMessages[1]="Respawn!";
        tutorialMessages[2]="Goal!";
        //Playerを探す
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        playerMove.EnableControl(false);//床に接触するまで動かないようにしておく
        playerMove.OnPlayerCollisionEnter += HandlePlayerCollisionAtStart;//床に接触したら呼ばれる
        playerMove.OnPlayerFirstMove += HandlePlayerFirstMove;//ゲーム開始して最初に動いたら呼ばれる
        if(timerDisplay!=null)timerDisplay.StopTimer();//始まるまでタイマーを止めておく
        //メインカメラを探す
        cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        // Itemタグの全オブジェクトを探す
        GameObject[] itemObjects = GameObject.FindGameObjectsWithTag("Item");
        items = new Item[itemObjects.Length];
        for (int i = 0; i < itemObjects.Length; i++)
        {
            items[i] = itemObjects[i].GetComponent<Item>();
            items[i].OnTriggerEnterAndDestroyed += HandleItemDestroyed;//アイテムがPlayerと接触したら呼ばれる
        }
        //スコアのセットと表示
        if(scoreDisplay!=null)
        {
            scoreDisplay.SetMaxScore(items.Length);
            scoreDisplay.SetScore(0);
        }
        // Gateタグの全オブジェクトを探す
        GameObject[] gateObjects = GameObject.FindGameObjectsWithTag("Gate");
        gates = new GateController[gateObjects.Length];
        for (int i = 0; i < gateObjects.Length; i++)
        {
            gates[i] = gateObjects[i].GetComponent<GateController>();
        }
        //RespawnPointに接触した時の処理
        playerMove.OnRespawnEnter += HandleRespawnCollision;
        //PlayerPrefからハイスコア読み込み
        highScorePoint = PlayerPrefs.GetInt("HighScorePoint",0);
        highScoreTime = PlayerPrefs.GetFloat("HighScoreTime",0);
        if(highScoreDisplay!=null&&highScoreTime!=0)SetHighScore(highScorePoint,highScoreTime);//ハイスコアの表示
    }

    //ハイスコアの表示をする関数
    private void SetHighScore(int score, float time)
    {
        highScoreDisplay.text = "High Score "+ScorePointToString(score)+" "+ScoreTimeToString(time);
    }

    //スコアのポイントをテキスト表示形式に変換する関数
    private string ScorePointToString(int score)
    {
        string pointText = string.Format("Score: {0:00}/{1:00}", score, items.Length);
        return pointText;
    }

    //スコアの時間をテキスト表示形式に変換する関数
    private string ScoreTimeToString(float time)
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        int centiseconds = (int)(time * 100) % 100;  // ミリ秒ではなくセンチ秒（1/100秒）を表示
        string timerText = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, centiseconds);
        return timerText;
    }

    //スタート時、プレイヤーが最初にコリジョンに接触したときの処理
    //動かすまでタイムがスタートしないようにしている
    private void HandlePlayerCollisionAtStart(Vector3 position)
    {
        playerMove.EnableControl(true);
        if(textController!=null)textController.DisplayMessageWithFade(tutorialMessages[0], 3.0f, 2.0f);//操作方法の表示
        playerMove.OnPlayerCollisionEnter -= HandlePlayerCollisionAtStart;
        playerMove.OnHazardEnter += HandleHazardCollision;//Hazardコリジョンに接触すると呼ばれるイベントの登録
        playerMove.OnGoalEnter += HandleGoalTrigger;//ゴールに到達した時に呼ばれるイベントの登録
    }

    //プレイヤーが最初にコリジョンに接触したときに呼ばれるイベント関数
    private void HandlePlayerFirstMove()
    {
        if(timerDisplay!=null)timerDisplay.StartTimer();      
    }

    //Hazardタグのオブジェクトに接触すると呼び出されるイベント関数
    private void HandleHazardCollision(Vector3 position)
    {
        StartCoroutine(ProcessRespawnSequence());
    }

    //リスポーンの処理
    private IEnumerator ProcessRespawnSequence()
    {
        if(HitSE!=null)HitSE.Play();
        //リスポーンして地面に落ちるまで操作、アイテム取得、リスポーンポイントの変更を止めておく
        playerIsDead = true;
        playerMove.EnableControl(false);
        playerMove.gameObject.tag = "Untagged";//アイテムはPlayerタグで接触を判別しているので一時的に回避
        if(timerDisplay!=null)timerDisplay.StopTimer();//タイマーを止めておく
        playerMove.OnHazardEnter -= HandleHazardCollision;//Hazard判定を止めておく
        if(textController!=null)textController.DisplayMessageWithFade(tutorialMessages[1], 3.0f, 2.0f);
        yield return new WaitForSeconds(3.0f);
        playerMove.StopMovement();//プレイヤーの速度と慣性をリセット
        //プレイヤーとカメラの位置をリスポーンポイントに移動
        playerMove.gameObject.transform.position = playerRespawnAt;
        if(cameraFollow!=null)cameraFollow.ResetCameraPosition();
        playerMove.OnPlayerCollisionEnter += HandlePlayerCollisionRespawn;//地面に落ちたら再開するためのイベント登録
    }
    //Respawn後、コリジョンと接触すると呼び出されるイベント
    private void HandlePlayerCollisionRespawn(Vector3 position)
    {
        //止めていた処理を元に戻す
        playerIsDead = false;
        playerMove.gameObject.tag = "Player";
        playerMove.EnableControl(true);
        playerMove.OnPlayerCollisionEnter -= HandlePlayerCollisionRespawn;
        playerMove.OnHazardEnter += HandleHazardCollision;
        if(timerDisplay!=null)timerDisplay.StartTimer();//タイマーを再開
    }

    //RespawnPointに接触したときの処理
    private void HandleRespawnCollision(Vector3 position)
    {
        if(!playerIsDead)playerRespawnAt = position;//リスポーンポイントの更新
    }

   //Goalタグのオブジェクトに接触すると呼び出されるイベント
    private void HandleGoalTrigger(Vector3 position)
    {
        StartCoroutine(ProcessGoalSequence());//ゴールプロセスの開始
    }

    //ゴールの処理
    private IEnumerator ProcessGoalSequence()
    {
        if(PickUpSE!=null)PickUpSE.Play();//音を鳴らす
        //タイマーや操作、イベントなどを止める処理
        if(timerDisplay!=null)timerDisplay.StopTimer();
        playerMove.EnableControl(false);
        playerMove.OnHazardEnter -= HandleGoalTrigger;
        playerMove.OnHazardEnter -= HandleHazardCollision;//Hazard判定を止めておく
        //ゴール表示
        if(textController!=null)textController.DisplayMessage(tutorialMessages[2]);
        float clearTime = timerDisplay.GetTime();
        string clearPointText = ScorePointToString(score);
        string clearTimeText = ScoreTimeToString(clearTime);
        yield return new WaitForSeconds(1.0f);
        if(PickUpSE!=null)PickUpSE.Play();
        if(textController!=null)textController.AddDisplayMessage("\n\n"+clearPointText);
        yield return new WaitForSeconds(1.0f);
        if(PickUpSE!=null)PickUpSE.Play();
        if(textController!=null)textController.AddDisplayMessage("\nTime:"+clearTimeText);
        //ポイントまたはタイムが更新されていたらハイスコアの更新と表示
        if(score>highScorePoint || (score>=highScorePoint && clearTime<highScoreTime))
        {
            yield return new WaitForSeconds(1.0f);
            SetHighScore(score,clearTime);
            if(PickUpSE!=null)PickUpSE.Play();
            if(textController!=null)textController.AddDisplayMessage("\nHigh Score!");
            PlayerPrefs.SetInt("HighScorePoint",score);
            PlayerPrefs.SetFloat("HighScoreTime",clearTime);
            PlayerPrefs.Save();
        }
    }

    //アイテム取得時の処理
    public void HandleItemDestroyed(Item item)
    {
        ScoreUp();
        // 新しいスコアを全てのGateに通知する
        foreach (var gate in gates)
        {
            gate.UpdateScore(score);
        }
    }

    //スコアの加算処理
    private void ScoreUp()
    {
        score++;
        if(scoreDisplay!=null)scoreDisplay.SetScore(score);
        if(PickUpSE!=null)PickUpSE.Play();
        if(IsClearOnAllItemsCollected && score == items.Length){//全アイテム回収でゴールとする場合のゴール判定
            StartCoroutine(ProcessGoalSequence());//ゴールプロセスの開始
        }
    }

    //リトライの処理
    public void onRetryButtonClick()
    {
        SceneManager.LoadScene("Game");
    }
}
