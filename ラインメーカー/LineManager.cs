using UnityEngine;

public class LineManager : MonoBehaviour
{
    public LineRenderer lineRenderer;  // LineRendererを設定する変数
    public GameObject movingObject;    // 動かしたい丸いオブジェクト
    public float speed = 5f;           // オブジェクトの移動速度

    private Vector3[] points;          // 線の頂点座標を格納する配列
    private int currentPointIndex = 0; // 現在の頂点インデックス
    private float t = 0;               // 補間用のタイマー

    void Start()
    {
        // LineRendererの全頂点座標を取得
        int pointCount = lineRenderer.positionCount;
        points = new Vector3[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            points[i] = lineRenderer.GetPosition(i);
        }

        // オブジェクトの初期位置を最初の頂点に設定
        Vector3 startPosition = points[0];
        startPosition.z = 1; // z座標を0に固定
        movingObject.transform.position = startPosition;
    }

    void Update()
    {
        // オブジェクトを現在の頂点から次の頂点へ移動させる
        if (currentPointIndex < points.Length - 1)
        {
            // 現在の頂点から次の頂点までの距離に基づいて補間
            t += Time.deltaTime * speed / Vector3.Distance(points[currentPointIndex], points[currentPointIndex + 1]);
            Vector3 newPosition = Vector3.Lerp(points[currentPointIndex], points[currentPointIndex + 1], t);

            // z座標を0に固定
            newPosition.z = 0;
            movingObject.transform.position = newPosition;

            // 目的地に到達したら次の頂点へ進む
            if (t >= 1f)
            {
                t = 0f;
                currentPointIndex++;
            }
        }
    }
}
