using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour
{
    [Header("スポーンさせるゾンビプレハブ(複数可)")]
    public GameObject[] zombiePrefabs;

    [Header("スポーン間隔(秒)")]
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 3f;

    [Header("親にしたいCanvas")]
    public Transform parentCanvas;   // ここにCanvasを指定

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
            SpawnZombie();
        }
    }

    private void SpawnZombie()
    {
        if (zombiePrefabs.Length == 0 || mainCam == null || parentCanvas == null) return;

        // ランダムなプレハブを選択
        GameObject prefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];

        // どの方向から出すか (0=上, 1=左, 2=右)
        int side = Random.Range(0, 3);

        Vector3 spawnPos = Vector3.zero;
        switch (side)
        {
            case 0: // 上から
                spawnPos = mainCam.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 1.1f, mainCam.nearClipPlane));
                break;
            case 1: // 左から
                spawnPos = mainCam.ViewportToWorldPoint(new Vector3(-0.1f, Random.Range(0f, 1f), mainCam.nearClipPlane));
                break;
            case 2: // 右から
                spawnPos = mainCam.ViewportToWorldPoint(new Vector3(1.1f, Random.Range(0f, 1f), mainCam.nearClipPlane));
                break;
        }

        // Z座標を0に固定（2D用）
        spawnPos.z = 0;

        // 角度を常にゼロでスポーン & Canvasの子オブジェクトにする
        GameObject zombie = Instantiate(prefab, spawnPos, Quaternion.identity, parentCanvas);
    }
}
