using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public float speed = 10.0f; //速度を設定
    [SerializeField]bool isControlEnabled = true;//操作を受け付けるかどうか
    private Rigidbody rb;//RigidBodyコンポーネント
    private Vector2 moveInput;//入力されている値
    public delegate void PlayerDelegate();//イベント型
    public delegate void PlayerCollisionEnterDelegate(Vector3 position);//コリジョンやトリガー接触時のイベント型
    public event PlayerDelegate OnPlayerFirstMove;  //プレイヤーが動いたときのイベント定義
    public event PlayerCollisionEnterDelegate OnPlayerCollisionEnter;   //コリジョン接触時のイベント定義
    public event PlayerCollisionEnterDelegate OnHazardEnter; // ゲームオーバータグでの衝突時のイベント定義
    public event PlayerCollisionEnterDelegate OnRespawnEnter; // リスタートポイントタグでの衝突時のイベント定義
    public event PlayerCollisionEnterDelegate OnGoalEnter; // ゴールタグでの衝突時のイベント定義
    //スタート時の処理
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //Playerタグがセットされているかをチェック
        if(!gameObject.CompareTag("Player"))
        {
            Debug.Log("PlayerオブジェクトのタグをPlayerにセットしてください");
            gameObject.tag = "Player";
        }

    }

    //InputSystemで設定されたMoveが呼ばれたときの処理
    void OnMove(InputValue movementValue)
    {
            moveInput = movementValue.Get<Vector2>();
            if(isControlEnabled)OnPlayerFirstMove?.Invoke();
    }

    //毎フレーム最後に呼ばれる処理
    void FixedUpdate()
    {
        if (isControlEnabled) 
        {
            // カメラの座標系に応じて入力を変換する
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 cameraRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;

            // 入力値をカメラ座標系での方向へ変換
            Vector3 movement = cameraRight * moveInput.x + cameraForward * moveInput.y;

            // Rigidbodyに力を加えてプレイヤーを移動
            rb.AddForce(movement * speed);
        }
    }

    //外部から操作のOn/Offを操作する関数
    public void EnableControl(bool enable)
    {
        isControlEnabled = enable;
    }

    //コリジョン接触判定
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Hazard")//Hazardオブジェクトとの接触はコリジョン判定
        {
            OnHazardEnter?.Invoke(transform.position);
        } 
        else if(collision.gameObject.tag == "Finish")//ゴールに入ったときの処理
        {
            OnGoalEnter?.Invoke(transform.position);
        }else
        {
            OnPlayerCollisionEnter?.Invoke(transform.position);//その他の処理
        }
    }

    //トリガー接触判定
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.tag == "Respawn")
        {
            OnRespawnEnter?.Invoke(collider.transform.position);
        } 
        else if(collider.gameObject.tag == "Hazard")//落下時はトリガーで判定
        {
            OnHazardEnter?.Invoke(collider.transform.position);
        }
        else if(collider.gameObject.tag == "Finish")//ゴールに入ったときの処理
        {
            OnGoalEnter?.Invoke(collider.transform.position);
        } 

    }

    //リスポーンの際に速度をリセット
    public void StopMovement()
    {
        // playerの速度を0にする
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.ResetInertiaTensor();
    }
}