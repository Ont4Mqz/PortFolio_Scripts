using System.Collections.Generic;
using UnityEngine;

public class DarumaSpawner : MonoBehaviour
{
    [Header("Manager参照")]
    [SerializeField] private DarumaManager manager;

    [Header("生成位置設定")]
    public Transform spawnBasePos; // 一番下の基準座標
    public float yOffset = 0.8f;   // 段の高さ間隔 多分1くらいがいい
    public int maxLayers = 4; // だんのかず

    [Header("生成プレハブリスト")]
    public List<GameObject> darumaPrefabs = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < maxLayers; i++)         //初期生成
        {
            SpawnNewDaruma();
        }
    }

    public void SpawnNewDaruma() //達磨をスポーンさせるメソッド
    {
        Vector3 spawnPos = spawnBasePos.position + Vector3.up * (manager.currentDarumas.Count * yOffset); //

        GameObject prefab = darumaPrefabs[Random.Range(0, darumaPrefabs.Count)]; //ランダムにプレハブ選ぶ

        GameObject newDaruma = Instantiate(prefab, spawnPos, Quaternion.identity);
        manager.currentDarumas.Add(newDaruma);
    }
}


// --------------------------- このスクリプトの流れ ---------------------------
//
// ▼ 概要
// 達磨を段ごとにランダム生成するスクリプト。
// DarumaManager と連携して、ゲーム開始時や達磨破壊後に新しい段を追加する役割。
//
// ▼ Start()
// ・ゲーム開始時に for ループで maxLayers 回（例：4段）スポーン。
// ・SpawnNewDaruma() を繰り返し呼び出して初期タワーを形成。
//
// ▼ SpawnNewDaruma()
// ・今の達磨の数に応じて次の生成位置を算出。
// 　　spawnBasePos.position（最下段の位置） + Vector3.up * 段数 * yOffset
// ・darumaPrefabs リストからランダムで1つ選び、Instantiate で生成。
// ・生成した達磨を manager.currentDarumas に追加してリスト管理。
// 　　→ このリストを DarumaManager 側で削除・参照してゲームを進行。
//
// ▼ 補足
// ・SpawnNewDaruma() は DarumaManager 側からも呼び出される。
// 　　→ 達磨を1段壊したときに上から新しい段を補充する。
// ・プレハブの見た目や物理挙動は prefab 側で調整。
// ・生成間隔や段数はインスペクタ上で調整可能（yOffset, maxLayers）。
//
// ---------------------------------------------------------------------------
