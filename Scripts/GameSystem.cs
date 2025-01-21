using UnityEngine;
using System.Collections;//コルーチンを使うために必要
using TMPro;//TextMeshProを使うために必要
using UnityEngine.SceneManagement;
using System.Collections.Generic;
[RequireComponent(typeof(AudioSource))]

public class GameSystem : MonoBehaviour
{
    private Player player;//プレイヤーのスクリプト
    private CameraFollow cameraFollow;//カメラのスクリプト
    [SerializeField] private List<Gate> gateList = new List<Gate>(); 
    TextController textController = null;//メッセージ表示用スクリプト
    ScoreDisplay scoreDisplay = null;//スコア表示用スクリプト
    [SerializeField] AudioClip PickUpSE;//アイテム取得時の音を鳴らすAudioClip
    [SerializeField] AudioClip HitSE;//Hazard接触時の音を鳴らすAudioClip
    [SerializeField] private bool IsClearOnAllItemsCollected=true;//全アイテム回収でゴールとするか否か
    [SerializeField] private string[] tutorialMessages = new string[3];//チュートリアルメッセージのテキスト
    [SerializeField] private string SceneName = "SampleScene";
    private int score = 0;//スコアの値
    private int maxScorePoint;//アイテムを全部取った時のポイント
    private bool playerIsDead;//プレイヤーがHazardに接触してからリスポーンするまでTrueになるフラグ 
    private Vector3 playerRespawnAt;//リスポーンポイントの座標
    private AudioSource audioSource;

    void Awake(){
        gameObject.tag = "GameController";
        //tutorialMessageの設定（入力しなくても動くよう設定。自身で書きたい方はこれを消してインスペクタに入れてください）
        tutorialMessages[0]="AWSD or ←↑→↓\nto Move";
        tutorialMessages[1]="Respawn!";
        tutorialMessages[2]="Goal!";
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
    void Start()
    {
        //Playerを探す
        StartCoroutine(FindTarget());
        if(scoreDisplay!=null)scoreDisplay.StopTimer();//始まるまでタイマーを止めておく
        //メインカメラを探す
        cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        //RespawnPointに接触した時の処理
        player.OnRespawnEnter += HandleRespawnCollision;
    }
    //プレイヤーを探すコルーチン
    IEnumerator FindTarget()
    {
        GameObject target = null;
        while (target == null)
        {
            target = GameObject.FindWithTag("Player");
            if (target == null)
            {
                yield return null;  // 1フレーム待機し次のフレームに処理を移行
            }
        }
        player = target.GetComponent<Player>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerRespawnAt = player.transform.position;
        player.EnableControl(false);//床に接触するまで動かないようにしておく
        player.OnPlayerCollisionEnter += HandlePlayerCollisionAtStart;//床に接触したら呼ばれる
        player.OnPlayerFirstMove += HandlePlayerFirstMove;//ゲーム開始して最初に動いたら呼ばれる
    }
    //スタート時、プレイヤーが最初にコリジョンに接触したときの処理
    //動かすまでタイムがスタートしないようにしている
    private void HandlePlayerCollisionAtStart(Vector3 position)
    {
        player.EnableControl(true);
        if(textController!=null)textController.DisplayMessageWithFade(tutorialMessages[0], 3.0f, 2.0f);//操作方法の表示
        player.OnPlayerCollisionEnter -= HandlePlayerCollisionAtStart;
        player.OnGoalEnter += HandleGoalTrigger;//ゴールに到達した時に呼ばれるイベントの登録
    }
    //プレイヤーが最初にコリジョンに接触したときに呼ばれるイベント関数
    private void HandlePlayerFirstMove()
    {
        if(scoreDisplay!=null)scoreDisplay.StartTimer();      
    }
    //Hazardタグのオブジェクトに接触すると呼び出されるイベント関数
    public void onPlayerHitHazard()
    {
        StartCoroutine(ProcessRespawnSequence());
    }
    //リスポーンの処理
    private IEnumerator ProcessRespawnSequence()
    {
        if(HitSE!=null)audioSource.PlayOneShot(HitSE);
        //リスポーンして地面に落ちるまで操作、アイテム取得、リスポーンポイントの変更を止めておく
        playerIsDead = true;
        player.EnableControl(false);
        player.gameObject.tag = "Untagged";//アイテムはPlayerタグで接触を判別しているので一時的に回避
        if(scoreDisplay!=null)scoreDisplay.StopTimer();//タイマーを止めておく
        if(textController!=null)textController.DisplayMessageWithFade(tutorialMessages[1], 3.0f, 2.0f);
        yield return new WaitForSeconds(3.0f);
        player.StopMovement();//プレイヤーの速度と慣性をリセット
        //プレイヤーとカメラの位置をリスポーンポイントに移動
        player.gameObject.transform.position = playerRespawnAt;
        if(cameraFollow!=null)cameraFollow.ResetCameraPosition();
        player.OnPlayerCollisionEnter += HandlePlayerCollisionRespawn;//地面に落ちたら再開するためのイベント登録
    }
    //Respawn後、コリジョンと接触すると呼び出されるイベント
    private void HandlePlayerCollisionRespawn(Vector3 position)
    {
        //止めていた処理を元に戻す
        playerIsDead = false;
        player.gameObject.tag = "Player";
        player.EnableControl(true);
        player.OnPlayerCollisionEnter -= HandlePlayerCollisionRespawn;
        if(scoreDisplay!=null)scoreDisplay.StartTimer();//タイマーを再開
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
        if(PickUpSE!=null)audioSource.PlayOneShot(PickUpSE);//音を鳴らす
        //タイマーや操作、イベントなどを止める処理
        if(scoreDisplay!=null)scoreDisplay.StopTimer();
        player.EnableControl(false);
        player.OnHazardEnter -= HandleGoalTrigger;//ゴール判定を終了
        //ゴール表示
        if(textController!=null)textController.DisplayMessage(tutorialMessages[2]);
        //結果の表示開始
        if(scoreDisplay!=null){
            yield return new WaitForSeconds(1.0f);
            if(PickUpSE!=null)audioSource.PlayOneShot(PickUpSE);//音を鳴らす
            if(textController!=null)textController.AddDisplayMessage("\n\n"+scoreDisplay.GetCurrentScoreText());
            yield return new WaitForSeconds(1.0f);
            if(PickUpSE!=null)audioSource.PlayOneShot(PickUpSE);//音を鳴らす
            if(textController!=null)
            {
                textController.AddDisplayMessage("\nTime:"+scoreDisplay.GetCurrentTimerText());
                bool isHighscore = scoreDisplay.UpdateHighScoreText();
                //ハイスコアの更新処理と表示
                if(isHighscore)
                {
                    yield return new WaitForSeconds(1.0f);
                    if(PickUpSE!=null)audioSource.PlayOneShot(PickUpSE);//音を鳴らす
                    textController.AddDisplayMessage("\nHigh Score!");
                }
            }
        }
    }

    //アイテム取得時の処理
    public void HandleItemDestroyed(Item item)
    {
        ScoreUp();
        // 新しいスコアを全てのGateに通知する
        if(gateList.Count>0)
        {
            foreach (var gate in gateList)
            {
                gate.UpdateScore(score);
            }
        }
    }

    //スコアの加算処理
    private void ScoreUp()
    {
        score++;
        if(scoreDisplay!=null)
        {
            scoreDisplay.SetScore(score);
            scoreDisplay.UpdateScoreDisplay();
        }
        if(PickUpSE!=null)audioSource.PlayOneShot(PickUpSE);//音を鳴らす
        if(IsClearOnAllItemsCollected && score == maxScorePoint){//全アイテム回収でゴールとする場合のゴール判定
            StartCoroutine(ProcessGoalSequence());//ゴールプロセスの開始
        }
    }
    //リトライの処理
    public void onRetryButtonClick()
    {
        SceneManager.LoadScene(SceneName);
    }
    //Attackボタンが押されたらリトライ

    //itemのセット
    public void SetItems(Item item)
    {
        Debug.Log("Itemがセットされた");
        maxScorePoint += 1;
        item.OnTriggerEnterAndDestroyed += HandleItemDestroyed;//アイテムがPlayerと接触したら呼ばれる
        //ScoreDisplayがアタッチされたらスコアのセットと表示
        StartCoroutine(WaitAttachingScoreDisplay());
    }
    IEnumerator WaitAttachingScoreDisplay()
    {
        while (scoreDisplay == null)
        {
            yield return null;  // 1フレーム待機し次のフレームに処理を移行
        }
        scoreDisplay.SetMaxScore(maxScorePoint);
    }
    //gateのセット
    public void SetGates(Gate Gate)
    {
        Debug.Log("Gateがセットされた");
        gateList.Add(Gate);
    }
    //TextControllerのセット
    public void SetTextController(TextController tc){
        Debug.Log("TextControllerがセットされた");
        textController = tc;
    }
    //ScoreDisplayのセット
    public void SetScoreDisplay(ScoreDisplay sd){
        Debug.Log("ScoreDisplayがセットされた");
        scoreDisplay = sd;
    }
}
