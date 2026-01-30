using System.Collections.Generic;
using UnityEngine;

public class DarumaSpawner : MonoBehaviour
{
    [Header("Manager参照")]
    [SerializeField] private DarumaManager manager; // 生成先のManager参照
    private GameObject prefab; // 生成用プレハブ一時保管

    [Header("生成位置設定")]
    public Transform spawnBasePos; // 一番下の基準座標
    public float yOffset = 0.8f; // 段の高さ間隔
    public int maxLayers = 4; // 段の数
    [Header("生成プレハブリスト")]
    public List<GameObject> darumaPrefabs = new List<GameObject>(); // 達磨プレハブリスト

    [Header("ShinyDaruma設定")]
    [SerializeField] private GameObject shinyDarumaPrefab; // ShinyDarumaプレハブ
    [SerializeField] private float shinySpawnRate = 0.15f; // ShinyDarumaのスポーン確率(15%)

    [Header("DebuffDaruma設定")]
    [SerializeField] private float debuffSpawnRate = 0.1f; // 通常の達磨にDebuffDarumaタグを付ける確率(10%)
    [SerializeField] private Material debuffMaterial; // DebuffDaruma用のマテリアル
    [SerializeField] private GameObject debuffEffectPrefab; // DebuffDaruma用のエフェクトプレハブ
    [SerializeField] private bool debuffEnabled = true; // ラスト10秒では無効化

    void Start()
    {
        for (int i = 0; i < maxLayers; i++) // 初期生成
        {
            SpawnNewDaruma();
        }
    }

    public void SpawnNewDaruma() // 1段だけスポーンしてmanagerに登録
    {
        Vector3 spawnPos = spawnBasePos.position + Vector3.up * (manager.currentDarumas.Count * yOffset); // 生成位置を算出
        GameObject newDaruma = InstantiateRandomDaruma(spawnPos);
        manager.currentDarumas.Add(newDaruma); // リストに追加
    }

    public GameObject SpawnDarumaAtPosition(Vector3 spawnPos, bool addToManagerList = true) // 指定位置にスポーン
    {
        GameObject newDaruma = InstantiateRandomDaruma(spawnPos);
        if (addToManagerList && manager != null)
        {
            manager.currentDarumas.Add(newDaruma);
        }
        return newDaruma;
    }

    private GameObject InstantiateRandomDaruma(Vector3 spawnPos) // ランダムに達磨を生成
    {
        GameObject newDaruma;

        float rand = Random.Range(0.0f, 1.0f);
        if (rand < shinySpawnRate && shinyDarumaPrefab != null) // 確率でShinyDarumaをスポーン
        {
            prefab = shinyDarumaPrefab;
            newDaruma = Instantiate(prefab, spawnPos, Quaternion.Euler(0, 180, 0)); // y軸を180度回転
            if (!newDaruma.CompareTag("ShinyDaruma")) // ShinyDarumaにはタグ付けする
            {
                Debug.LogWarning("ShinyDarumaプレハブに'ShinyDaruma'タグが設定されてない。Unityエディタで設定して。");
            }
        }
        else // 通常の達磨をスポーン
        {
            int availableCount = darumaPrefabs.Count;
            if (DifficultyLevel.easy)  availableCount = Mathf.Min(4, darumaPrefabs.Count); // 0～3の4つ
            if (DifficultyLevel.normal) availableCount = Mathf.Min(8, darumaPrefabs.Count); // 0～7の8つ
            if (DifficultyLevel.hard)  availableCount = darumaPrefabs.Count; // 全種類

            if (availableCount <= 0)
            {
                Debug.LogWarning("darumaPrefabsにプレハブが設定されていません。");
                return null;
            }

            prefab = darumaPrefabs[Random.Range(0, availableCount)]; // 上限は排他的なのでCountを直接渡す

            newDaruma = Instantiate(prefab, spawnPos, Quaternion.Euler(0,90,0));
            if (debuffEnabled)
            {
                float debuffRand = Random.Range(0.0f, 1.0f); // 確率でDebuffDarumaタグを付ける
                if (debuffRand < debuffSpawnRate)
                {
                    try
                    {
                        newDaruma.tag = "DebuffDaruma";

                        if (debuffMaterial != null) // DebuffDaruma用のマテリアルに帰る
                        {
                            Renderer renderer = newDaruma.GetComponent<Renderer>();
                            if (renderer != null)
                            {
                                renderer.material = debuffMaterial;
                            }
                            else
                            {
                                Renderer[] childRenderers = newDaruma.GetComponentsInChildren<Renderer>(); // 子オブジェクトのレンダラーも探す
                                foreach (Renderer childRenderer in childRenderers)
                                {
                                    childRenderer.material = debuffMaterial;
                                }
                            }
                        }

                        if (debuffEffectPrefab != null) // DebuffDaruma用のエフェクトを付ける
                        {
                            Instantiate(debuffEffectPrefab, newDaruma.transform);
                        }
                    }
                    catch
                    {
                        Debug.LogWarning("'DebuffDaruma'タグが存在しません。Unityエディタでタグを作成してください。");
                    }
                }
            }
        }

        return newDaruma;
    }

    public void SetManager(DarumaManager newManager) // Manager参照を設定
    {
        manager = newManager;
    }

    public void SetDebuffEnabled(bool enabled) // DebuffDaruma生成の有効/無効を設定
    {
        debuffEnabled = enabled;
    }
}