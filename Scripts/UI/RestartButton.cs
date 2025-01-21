using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    private Button restartButton;
    private GameSystem gameSystem;  // GameSystemへの参照

    void Start()
    {
        // このGameObjectからButtonコンポーネントを取得
        restartButton = GetComponent<Button>();
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        else
        {
            Debug.LogError("Buttonオブジェクトにアタッチしてください");
        }

        // GameControllerを検索
        StartCoroutine(FindGameControllerAndSetup());
    }

    IEnumerator FindGameControllerAndSetup()
    {
        while (gameSystem == null)
        {
            GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
            if (gameController != null)
            {
                gameSystem = gameController.GetComponent<GameSystem>();
            }
            yield return null;  // オブジェクトが見つかるまで1フレーム待機
        }
    }

    public void RestartGame()
    {
        if (gameSystem != null)gameSystem.onRetryButtonClick();
    }
}
