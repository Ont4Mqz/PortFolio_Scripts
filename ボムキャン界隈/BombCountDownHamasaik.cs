using UnityEngine;
using TMPro;
using System.Collections;

public class BombCountDownHamasaki : MonoBehaviour
{
    public TextMeshProUGUI timerText1;     // 1�ڂ̃^�C�}�[�\���p��TextMeshPro
    public TextMeshProUGUI timerText2;     // 2�ڂ̃^�C�}�[�\���p��TextMeshPro
    public float startDelay = 5f;          // �^�C�}�[�J�n�܂ł̒x������
    public float countdownTime = 10f;      // �J�E���g�_�E���̒����i�b�j

    private bool isCountingDown = false;
    private float currentTime;

    void Start()
    {
        // �Q�[���J�n����w��b����ɃJ�E���g�_�E�����J�n
        StartCoroutine(StartCountdownAfterDelay());
    }

    IEnumerator StartCountdownAfterDelay()
    {
        // �w�肳�ꂽ�x�����Ԃ�ҋ@
        yield return new WaitForSeconds(startDelay);

        // �J�E���g�_�E�����J�n
        isCountingDown = true;
        currentTime = countdownTime;
    }

    void Update()
    {
        if (isCountingDown)
        {
            // ���Ԃ����炷
            currentTime -= Time.deltaTime;

            // �����_��1�ʂ܂ŕ\��
            string formattedTime = currentTime.ToString("F1");
            if (timerText1 != null) timerText1.text = formattedTime;
            if (timerText2 != null) timerText2.text = formattedTime;

            // �J�E���g�_�E�����I���������~
            if (currentTime <= 0)
            {
                currentTime = 0;
                isCountingDown = false;
                if (timerText1 != null) timerText1.text = "0.0";
                if (timerText2 != null) timerText2.text = "0.0";
            }
        }
    }
}
