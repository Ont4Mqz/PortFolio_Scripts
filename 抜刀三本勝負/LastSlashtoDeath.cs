using UnityEngine;
using System.Collections;

public class LastSlashToDeath : MonoBehaviour
{
    [Header("�v���C���[�I�u�W�F�N�g���w��")]
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;

    [Header("�ړ���|�W�V����")]
    [SerializeField] private Transform p1TargetPos;
    [SerializeField] private Transform p2TargetPos;

    [Header("�ړ����x")]
    [SerializeField] private float moveSpeed = 5f;

    public void P1Win()//Player1����
    {
        if (player1 != null && p1TargetPos != null)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToTarget(player1, p1TargetPos.position));
        }
    }

    public void P2Win()//Player2����
    {
        if (player2 != null && p2TargetPos != null)
        {
            StopAllCoroutines();
            StartCoroutine(MoveToTarget(player2, p2TargetPos.position));
        }
    }

    private IEnumerator MoveToTarget(GameObject player, Vector3 targetPos)//�ړ��p�̃��\�b�h
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

    public void Draw()//������
    {
        //�������̂Ƃ��Ƃ�
    }
}
