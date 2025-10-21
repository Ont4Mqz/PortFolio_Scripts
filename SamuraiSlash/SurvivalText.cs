using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SurvivalText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
 
    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = SurvivalTime._survivalTime.ToString("F1") + "•b";   
    }
}
