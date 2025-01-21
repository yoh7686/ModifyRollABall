using System.Collections;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private bool isHazardTrigger = false; // インスペクタからオンオフできる
    private GameSystem gameSystem;

    void Start()
    {
        // MeshRendererを無効化
        if (isHazardTrigger)
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }

            // BoxColliderのisTriggerをtrueに設定
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                boxCollider.isTrigger = true;
            }
            else
            {
                Debug.LogWarning("BoxCollider not found on the Hazard.");
            }
        }

        StartCoroutine(FindGameSystem());
    }

    IEnumerator FindGameSystem()
    {
        while (gameSystem == null)
        {
            GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
            if (gameController != null)
            {
                gameSystem = gameController.GetComponent<GameSystem>();
            }
            yield return null; // 1フレーム待機
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameSystem != null)gameSystem.onPlayerHitHazard();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameSystem != null)gameSystem.onPlayerHitHazard();
        }
    }
}
