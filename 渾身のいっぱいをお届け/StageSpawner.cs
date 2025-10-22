using UnityEngine;

public class StageSpawner : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private int stagecount = 30;
    [SerializeField] private float minXInterval = 500f;
    [SerializeField] private float maxXInterval = 1250f;
    [SerializeField] private float minY = -1000f;
    [SerializeField] private float maxY = 0f;
    [SerializeField] private RectTransform canvasTransform;

    private void Start()
    {
        GeneratePlatforms();
    }

    private void GeneratePlatforms()
    {
        if (platformPrefab == null || canvasTransform == null) return;

        float currentX = 0f;

        for (int i = 0; i < stagecount; i++)
        {
            float xInterval = Random.Range(minXInterval, maxXInterval);
            currentX += xInterval;

            float yPos = Random.Range(minY, maxY);
            Vector2 anchoredPos = new Vector2(currentX, yPos);

            GameObject platform = Instantiate(platformPrefab, canvasTransform);
            platform.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        }
    }
}
