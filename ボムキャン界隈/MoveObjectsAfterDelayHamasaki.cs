using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveObjectAfterDelayHamasaki : MonoBehaviour
{
    [System.Serializable]
    public class ObjectMovement
    {
        public GameObject targetObject;      // 移動させるオブジェクト
        public float delaySeconds;           // 遅延時間（秒）
        public GameObject targetDestination; // 移動先のオブジェクト
        public float moveDuration;           // 移動にかける時間（秒）
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
        // 遅延時間を待機
        yield return new WaitForSeconds(movement.delaySeconds);

        // オブジェクトと移動先が設定されているか確認
        if (movement.targetObject != null && movement.targetDestination != null)
        {
            Vector3 startPosition = movement.targetObject.transform.position;
            Vector3 endPosition = movement.targetDestination.transform.position;
            float elapsedTime = 0;

            while (elapsedTime < movement.moveDuration)
            {
                // 時間に応じてオブジェクトを移動
                movement.targetObject.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / movement.moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 最終位置をターゲットの位置に合わせる
            movement.targetObject.transform.position = endPosition;
        }
    }
}
