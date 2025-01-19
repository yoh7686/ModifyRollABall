using UnityEngine;
using TMPro;
using System.Collections;

public class TextController : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    
    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = "";
        StartCoroutine(FindTarget());
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
        gameSystem.SetTextController(this);
    }

    //テキストを空にする
    private void ResetText()
    {
        textMesh.text = string.Empty;
    }

    //テキストを表示する
    public void DisplayMessage(string message)
    {
        textMesh.text = message;
    }

    //表示されているメッセージにテキストを足す
      public void AddDisplayMessage(string message)
    {
        textMesh.text += message;
    }

    //メッセージを表示してフェイドアウト
    public void DisplayMessageWithFade(string message, float displayTime, float fadeTime)
    {
        textMesh.text = message;
        StartCoroutine(FadeAndReset(displayTime, fadeTime));
    }

    IEnumerator FadeAndReset(float displayTime, float fadeTime)
    {
        yield return new WaitForSeconds(displayTime);

        float rate = 1.0f / fadeTime;
        for (float i = 1.0f; i >= 0; i -= rate * Time.deltaTime)
        {
            // Alpha値を変更してフェードアウト効果を作る
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, i);
            yield return null;
        }
        //フェードが終わったらテキストを消してAlpha値を元に戻す
        ResetText();
        textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1.0f); // アルファ値をリセット
    }
}
