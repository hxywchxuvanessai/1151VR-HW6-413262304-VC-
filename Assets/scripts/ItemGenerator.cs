
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public GameObject applePrefab;
    public GameObject bombPrefab;

    public float interval = 1f;
    public float dropSpeed = 2f;

    [Range(0, 100)]
    public int bombRate = 20;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        // 隨機決定蘋果或炸彈
        bool isBomb = Random.Range(0, 100) < bombRate;

        GameObject prefab = isBomb ? bombPrefab : applePrefab;

        // 九宮格隨機位置
        int x = Random.Range(-1, 2);
        int z = Random.Range(-1, 2);

        Vector3 spawnPosition = new Vector3(x, 4f, z);

        GameObject item = Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );

        // 設定掉落速度
        ItemController controller = item.GetComponent<ItemController>();

        if (controller != null)
        {
            controller.dropSpeed = dropSpeed;
        }
    }
}
