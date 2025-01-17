using System.Collections;//コルーチンを使うために必要
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private int scoreThreshold;//このスコアになったら動く
    [SerializeField] private float moveDuration;//動く時間
    [SerializeField] private Vector3 moveVector;//この距離だけ動く

    private Vector3 initialPosition;//初期位置
    private bool isMoved = false;//動いたらtrueになる

    private void Start()
    {
        initialPosition = transform.position;
        if (gameObject.tag != "Gate")
        {
            Debug.LogError("Gateタグをゲートオブジェクトにセットしてください");
        }
    }

    //スコアが更新されるたびに呼ばれる関数
    public void UpdateScore(int score)
    {
        //スコアが一定を越えていてまだ動いていなかったらコルーチンを呼ぶ
        if (!isMoved && score >= scoreThreshold)
        {
            isMoved = true;
            StartCoroutine("MoveGate");
        }
    }

    IEnumerator MoveGate()
    {
        float elapsedTime = 0;
        //指定時間の間動く
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float ratio = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(initialPosition, initialPosition + moveVector, ratio);
            yield return null;
        }
        // 終点に到達した後の位置の微調整
        transform.position = initialPosition + moveVector;
    }
}
