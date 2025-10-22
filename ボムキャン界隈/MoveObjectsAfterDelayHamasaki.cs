using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveObjectAfterDelayHamasaki : MonoBehaviour
{
    [System.Serializable]
    public class ObjectMovement
    {
        public GameObject targetObject;      // �ړ�������I�u�W�F�N�g
        public float delaySeconds;           // �x�����ԁi�b�j
        public GameObject targetDestination; // �ړ���̃I�u�W�F�N�g
        public float moveDuration;           // �ړ��ɂ����鎞�ԁi�b�j
    }

    public List<ObjectMovement> movements = new List<ObjectMovement>();

    void Start()
    {
        foreach (var movement in movements)
        {
            StartCoroutine(MoveObjectAfterDelay(movement));
        }
    }

    IEnumerator MoveObjectAfterDelay(ObjectMovement movement)
    {
        // �x�����Ԃ�ҋ@
        yield return new WaitForSeconds(movement.delaySeconds);

        // �I�u�W�F�N�g�ƈړ��悪�ݒ肳��Ă��邩�m�F
        if (movement.targetObject != null && movement.targetDestination != null)
        {
            Vector3 startPosition = movement.targetObject.transform.position;
            Vector3 endPosition = movement.targetDestination.transform.position;
            float elapsedTime = 0;

            while (elapsedTime < movement.moveDuration)
            {
                // ���Ԃɉ����ăI�u�W�F�N�g���ړ�
                movement.targetObject.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / movement.moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // �ŏI�ʒu���^�[�Q�b�g�̈ʒu�ɍ��킹��
            movement.targetObject.transform.position = endPosition;
        }
    }
}
