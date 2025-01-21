using UnityEngine;

public class Respawn : MonoBehaviour
{
    public string tagName = "Respawn"; // タグ名を外部から設定可能に

    void Awake()
    {
        // オブジェクトのタグを設定
        gameObject.tag = tagName;

        // メッシュレンダラーを取得してオフにする
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }
        else
        {
            Debug.LogWarning(gameObject.name+"のMeshRendererが見つかりません");
        }

        // BoxColliderを取得し、isTriggerをTrueにする
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.isTrigger = true;
        }
        else
        {
            Debug.LogWarning(gameObject.name+"のBoxColliderが見つかりません");
        }
    }
}
