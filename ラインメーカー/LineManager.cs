using UnityEngine;

public class LineManager : MonoBehaviour
{
    public LineRenderer lineRenderer;  // LineRenderer��ݒ肷��ϐ�
    public GameObject movingObject;    // �����������ۂ��I�u�W�F�N�g
    public float speed = 5f;           // �I�u�W�F�N�g�̈ړ����x

    private Vector3[] points;          // ���̒��_���W���i�[����z��
    private int currentPointIndex = 0; // ���݂̒��_�C���f�b�N�X
    private float t = 0;               // ��ԗp�̃^�C�}�[

    void Start()
    {
        // LineRenderer�̑S���_���W���擾
        int pointCount = lineRenderer.positionCount;
        points = new Vector3[pointCount];
        for (int i = 0; i < pointCount; i++)
        {
            points[i] = lineRenderer.GetPosition(i);
        }

        // �I�u�W�F�N�g�̏����ʒu���ŏ��̒��_�ɐݒ�
        Vector3 startPosition = points[0];
        startPosition.z = 1; // z���W��0�ɌŒ�
        movingObject.transform.position = startPosition;
    }

    void Update()
    {
        // �I�u�W�F�N�g�����݂̒��_���玟�̒��_�ֈړ�������
        if (currentPointIndex < points.Length - 1)
        {
            // ���݂̒��_���玟�̒��_�܂ł̋����Ɋ�Â��ĕ��
            t += Time.deltaTime * speed / Vector3.Distance(points[currentPointIndex], points[currentPointIndex + 1]);
            Vector3 newPosition = Vector3.Lerp(points[currentPointIndex], points[currentPointIndex + 1], t);

            // z���W��0�ɌŒ�
            newPosition.z = 0;
            movingObject.transform.position = newPosition;

            // �ړI�n�ɓ��B�����玟�̒��_�֐i��
            if (t >= 1f)
            {
                t = 0f;
                currentPointIndex++;
            }
        }
    }
}
