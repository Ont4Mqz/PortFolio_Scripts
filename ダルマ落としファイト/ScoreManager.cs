using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("UI�Q��")]
    [SerializeField] private TextMeshProUGUI scoreText;

    public static int score = 0; //�X�R�A�������z�����߂�static�����܂���

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int points) //DarumaManager����Ă΂��
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI() //UI�ɔ��f����
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    public void ResetScore() //Score���Z�b�g
    {
        score = 0;
        UpdateScoreUI();
    }
}


// --------------------------- ���̃X�N���v�g�̗��� ---------------------------
//
// �� �T�v
// �Q�[���S�̂̃X�R�A�Ǘ����s���X�N���v�g�B
// �X�R�A���Z�A���Z�b�g�AUI�X�V��3�̖��������B
// static�ϐ��𗘗p���ăV�[�����܂����ł��X�R�A��ێ��ł���悤�ɂȂ��Ă���B
//
// �� Start()
// �E�Q�[���J�n���Ɉ�x���� UpdateScoreUI() ���Ă�ŃX�R�A�\�����������B
//
// �� AddScore(int points)
// �EDarumaManager �Ȃǂ���Ă΂��X�R�A���Z���\�b�h�B
// �E�w�肳�ꂽ points �� static �� score �ɉ��Z���AUI���X�V�B
//
// �� UpdateScoreUI()
// �EscoreText ���A�^�b�`����Ă���΁A���݂̃X�R�A�� "Score: XXX" �̌`�ŕ\���B
// �EUI���f��p�̓������\�b�h�i�O������͒��ڌĂ΂Ȃ��j�B
//
// �� ResetScore()
// �E�X�R�A��0�ɖ߂��āAUI�ɂ����f�B
// �E�Q�[�����X�^�[�g�⃊�g���C���ɌĂ΂��z��B
//
// �� �⑫
// �Estatic score �̂��߁A�V�[����؂�ւ��Ă��l���ێ������i�蓮�Ń��Z�b�g���K�v�j�B
// �E�X�R�A��UI�Q�Ƃ̓C���X�y�N�^��Őݒ�B
// �E�P���Ȏd�g�݂����A���̃V�X�e���i�R���{�E�{���Ȃǁj�Ƒg�ݍ��킹�ď_��Ɋg���\�B
//
// ---------------------------------------------------------------------------
