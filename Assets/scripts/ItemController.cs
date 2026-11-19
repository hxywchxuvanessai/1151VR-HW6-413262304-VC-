
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public float dropSpeed = 2f;
    public float destroyY = -1f;

    void Update()
    {
        // 每秒往下移動
        transform.position += Vector3.down * dropSpeed * Time.deltaTime;

        // 掉到舞台下方就刪除
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}
