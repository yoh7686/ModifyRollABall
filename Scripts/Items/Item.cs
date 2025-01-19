using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    public string targetTag = "Player";//接触したら消える相手をStringで指定
    public delegate void TriggerEnterAndDestroyDelegate(Item item);//イベント型の宣言
    public event TriggerEnterAndDestroyDelegate OnTriggerEnterAndDestroyed; //指定のタグのオブジェクトに接触したら呼ばれるイベント

    private void Awake()
    {
        StartCoroutine(FindTarget());
        //isTriggerがtrueかチェック
        BoxCollider boxCollider=GetComponent<BoxCollider>();
        if(boxCollider==null){
            Debug.LogError("BoxColliderがアタッチされていません！");
        }else if(!boxCollider.isTrigger){
            boxCollider.isTrigger = true;
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
        GameSystem gameSystem = target.GetComponent<GameSystem>();
        gameSystem.SetItems(this);
    }

    //コライダーの範囲に他のオブジェクトが入ったら呼ばれる
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(targetTag))
        {
            //OnTriggerEnterAndDestroyedに登録された関数を呼ぶ
            OnTriggerEnterAndDestroyed?.Invoke(this);
            //自分を消す
            Destroy(this.gameObject);
        }
    }
}