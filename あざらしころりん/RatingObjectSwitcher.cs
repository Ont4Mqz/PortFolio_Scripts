using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RatingObjectSwitcher : MonoBehaviour
{
    public Dropdown categoryDropdown;
    public Dropdown ratingDropdown;

    public GameObject[] result1AObjects;
    public GameObject[] result1BObjects;
    public GameObject[] result1CObjects;
    public GameObject[] result1DObjects;

    public GameObject[] result2AObjects;
    public GameObject[] result2BObjects;
    public GameObject[] result2CObjects;
    public GameObject[] result2DObjects;

    public GameObject[] result3AObjects;
    public GameObject[] result3BObjects;
    public GameObject[] result3CObjects;
    public GameObject[] result3DObjects;

    void Start()
    {
        // ドロップダウンの選択肢を設定
        categoryDropdown.ClearOptions();
        categoryDropdown.AddOptions(new List<string> { "Result 1", "Result 2", "Result 3" });

        ratingDropdown.ClearOptions();
        ratingDropdown.AddOptions(new List<string> { "A", "B", "C", "D" });

        // TimerandResultManagerから現在の設定を反映
        categoryDropdown.value = TimerandResultManager.SelectedCategoryIndex;
        ratingDropdown.value = GetRatingIndex(TimerandResultManager.SelectedRating);

        // 初期表示の設定
        UpdateObjects();

        // ドロップダウンの変更時イベントを登録
        categoryDropdown.onValueChanged.AddListener(delegate { UpdateObjects(); });
        ratingDropdown.onValueChanged.AddListener(delegate { UpdateObjects(); });
    }

    void UpdateObjects()
    {
        // すべてのオブジェクトを非表示にする
        HideAllObjects();

        int categoryIndex = categoryDropdown.value;
        string rating = ratingDropdown.options[ratingDropdown.value].text;

        // 現在選択されているカテゴリと評価に応じてオブジェクトを表示
        switch (categoryIndex)
        {
            case 0: ShowObjectsForRating(rating, result1AObjects, result1BObjects, result1CObjects, result1DObjects); break;
            case 1: ShowObjectsForRating(rating, result2AObjects, result2BObjects, result2CObjects, result2DObjects); break;
            case 2: ShowObjectsForRating(rating, result3AObjects, result3BObjects, result3CObjects, result3DObjects); break;
        }
    }

    void ShowObjectsForRating(string rating, GameObject[] aObjects, GameObject[] bObjects, GameObject[] cObjects, GameObject[] dObjects)
    {
        switch (rating)
        {
            case "A": SetObjectsActive(aObjects, true); break;
            case "B": SetObjectsActive(bObjects, true); break;
            case "C": SetObjectsActive(cObjects, true); break;
            case "D": SetObjectsActive(dObjects, true); break;
        }
    }

    void HideAllObjects()
    {
        SetObjectsActive(result1AObjects, false);
        SetObjectsActive(result1BObjects, false);
        SetObjectsActive(result1CObjects, false);
        SetObjectsActive(result1DObjects, false);

        SetObjectsActive(result2AObjects, false);
        SetObjectsActive(result2BObjects, false);
        SetObjectsActive(result2CObjects, false);
        SetObjectsActive(result2DObjects, false);

        SetObjectsActive(result3AObjects, false);
        SetObjectsActive(result3BObjects, false);
        SetObjectsActive(result3CObjects, false);
        SetObjectsActive(result3DObjects, false);
    }

    void SetObjectsActive(GameObject[] objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(isActive);
        }
    }

    int GetRatingIndex(string rating)
    {
        switch (rating)
        {
            case "A": return 0;
            case "B": return 1;
            case "C": return 2;
            case "D": return 3;
            default: return 0;
        }
    }
}
