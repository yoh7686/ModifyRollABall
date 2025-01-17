using UnityEngine;
public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotationVector = new Vector3(15,30,45);

    void Update()
    {
        // 時間に応じた角度を計算
        Vector3 angle = rotationVector * Time.deltaTime;
        // 回転を適用
        transform.Rotate(angle);
    }

}