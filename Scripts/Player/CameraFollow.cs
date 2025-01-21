using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    private GameObject target; // ターゲットへの参照
    [SerializeField] private Vector3 offset; // 相対座標
    [SerializeField] private float smoothTime = 0.3f; // 追従するスピード
    private Vector3 velocity = Vector3.zero; // ベロシティを保存する参照用変数

    void Start()
    {
        StartCoroutine(FindTarget());
    }

    IEnumerator FindTarget()
    {
        target = null;
        while (target == null)
        {
            target = GameObject.FindWithTag("Player");
            if (target == null)
            {
                yield return null;  // 1フレーム待機し次のフレームに処理を移行
            }
        }
        offset = transform.position - target.transform.position;
    }

    public void ResetCameraPosition()
    {
        // リセット
        velocity = Vector3.zero;
        transform.position = target.transform.position + offset;
    }
    
    void LateUpdate () {
        if(target!=null)
        {
            // 目標とするポジションを設定
            Vector3 goalPosition = target.transform.position + offset;
            // 現在から目標とするポジションへスムーズに移動
            transform.position = Vector3.SmoothDamp(transform.position, goalPosition, ref velocity, smoothTime);
        }
    }
}
