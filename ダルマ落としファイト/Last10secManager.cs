using System.Collections.Generic;
using UnityEngine;

public class Last10secManager : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private TimerCount timerCount; // タイマーを見る
    [SerializeField] private DarumaManager player1Manager; // P1管理
    [SerializeField] private DarumaManager player2Manager; // P2管理
    [SerializeField] private DarumaSpawner finalSpawner; // 共有スポーナー

    [Header("生成位置（未指定ならfinalSpawnerの基準位置）")]
    [SerializeField] private Transform finalSpawnPoint; // 共有の起点
    [SerializeField] private int finalStackCount = 4; // ラストで積む段数

    private bool hasTriggered; // 一度きり
    private List<GameObject> sharedDarumaList = new List<GameObject>(); // 共有リスト

    void Update()
    {
        if (hasTriggered) return; // もう発動済み
        if (timerCount == null || !timerCount.StartFlag) return; // 試合前は何もしない

        if (timerCount._timerCount <= 10f) // 残り10秒
        {
            TriggerFinalDaruma();
            hasTriggered = true;
        }
    }

    private void TriggerFinalDaruma()
    {
        if (player1Manager == null || player2Manager == null || finalSpawner == null)
        {
            Debug.LogWarning("Last10secManagerの参照がない。インスペクターをみろ");
            return;
        }

        ClearDarumas(player1Manager);  // 既存を消して共有リストに差し替え
        ClearDarumas(player2Manager);
        sharedDarumaList.Clear();
        player1Manager.currentDarumas = sharedDarumaList;
        player2Manager.currentDarumas = sharedDarumaList;

        // 共有用スポーナー設定
        if (finalSpawnPoint != null)
        {
            finalSpawner.spawnBasePos = finalSpawnPoint; // 基準座標を共有地点に
        }

        finalSpawner.SetManager(player1Manager); // 高さ計算の参照先を共有に
        finalSpawner.SetDebuffEnabled(false);    // デバフは切る
        player1Manager.SetSpawner(finalSpawner);
        player2Manager.SetSpawner(finalSpawner);
        player1Manager.SetAutoSpawnEnabled(true);
        player2Manager.SetAutoSpawnEnabled(true);

        for (int i = 0; i < Mathf.Max(1, finalStackCount); i++) // 共有達磨を積む
        {
            finalSpawner.SpawnNewDaruma(); // 共有リストに積む
        }
    }

    private void ClearDarumas(DarumaManager manager)
    {
        foreach (var daruma in manager.currentDarumas)
        {
            if (daruma != null) Destroy(daruma);
        }
        manager.currentDarumas.Clear();
    }
}

