using UnityEngine;

public class SineWaveMovement : MonoBehaviour
{
    // �֍s�̕��ƃX�s�[�h��Inspector�Őݒ�ł���悤��
    [SerializeField] private float amplitude = 1.0f; // �֍s�̕�
    [SerializeField] private float frequency = 1.0f; // �֍s�̃X�s�[�h
    [SerializeField] private float moveSpeed = 1.0f; // ���ւ̐i�ރX�s�[�h

    private Vector3 startPosition;

    void Start()
    {
        // �����ʒu���L�^
        startPosition = transform.position;
    }

    void Update()
    {
        // ���ɐi�ޓ���
        float leftMovement = moveSpeed * Time.deltaTime;
        transform.position -= new Vector3(leftMovement, 0, 0);

        // �֍s�̓���
        float wave = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, startPosition.y + wave, transform.position.z);
    }
}
