using UnityEngine;
using System.Collections;

public class LastSlashToDeath : MonoBehaviour
{
    [Header("プレイヤーオブジェクトを指定")]
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    [Header("移動先ポジション")]
    [SerializeField] private Transform p1TargetPos;
    [SerializeField] private Transform p2TargetPos;

    [Header("移動速度")]
    [SerializeField] private float moveSpeed = 5f;

    public void P1Win()//Player1勝利
    {
        if (player1 != null && p1TargetPos != null)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToTarget(player1, p1TargetPos.position));
        }
    }

    public void P2Win()//Player2勝利
    {
        if (player2 != null && p2TargetPos != null)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToTarget(player2, p2TargetPos.position));
        }
    }

    private IEnumerator MoveToTarget(GameObject player, Vector3 targetPos)//移動用のメソッド
    {
        while (player != null && Vector3.Distance(player.transform.position, targetPos) > 0.01f)
        {
            player.transform.position = Vector3.MoveTowards(
                player.transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
    }

    public void Draw()//あいこ
    {
        //あいこのときとか
    }
}
