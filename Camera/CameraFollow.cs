using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private GameObject target; // ターゲットへの参照
    [SerializeField] private Vector3 offset = new Vector3(0f, 7f, -13f); // 相対座標
    [SerializeField] private float smoothTime = 0.3f; // 追従するスピード
    private Vector3 velocity = Vector3.zero; // ベロシティを保存する参照用変数

    void Start()
    {
        target = GameObject.FindWithTag("Player");
        //Playerタグのオブジェクトがなかったらエラーメッセージを出す
        if(target==null)
        {
            Debug.LogError("Playerタグをプレイヤーオブジェクトにセットしてください");
        }
    }

    public void ResetCameraPosition()
    {
        // リセット
        velocity = Vector3.zero;
        transform.position = target.transform.position + offset;
    }
    
    void LateUpdate () {
        // 目標とするポジションを設定
        Vector3 goalPosition = target.transform.position + offset;
        // 現在から目標とするポジションへスムーズに移動
        transform.position = Vector3.SmoothDamp(transform.position, goalPosition, ref velocity, smoothTime);
    }
}
