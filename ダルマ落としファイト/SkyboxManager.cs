using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    [Header("Skybox Settings")]
    [SerializeField] private Material easySkybox;
    [SerializeField] private Material normalSkybox;
    [SerializeField] private Material hardSkybox;

    void Start()
    {
        UpdateSkybox();
    }

    public void UpdateSkybox() // 難易度に応じてSkyboxを変更
    {
        if (DifficultyLevel.easy)
        {
            RenderSettings.skybox = easySkybox;
        }
        else if (DifficultyLevel.normal)
        {
            RenderSettings.skybox = normalSkybox;
        }
        else if (DifficultyLevel.hard)
        {
            RenderSettings.skybox = hardSkybox;
        }

        DynamicGI.UpdateEnvironment();
    }
}
