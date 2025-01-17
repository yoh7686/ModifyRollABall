using UnityEngine;

public class VirtualPadController : MonoBehaviour
{
    void Awake()
    {
        // モバイルデバイスで実行されているかチェック
        if (!Application.isMobilePlatform)
        {
            gameObject.SetActive(false);
        }
    }
}