using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ZombieMove : MonoBehaviour
{
    [Header("�^�[�Q�b�g�̃^�O")]
    public string targetTag = "Player";

    [Header("�ړ����x")]
    public float moveSpeed = 2f;

    [Header("�U�����苗��")]
    public float attackRange = 30f;

    [Header("�A�j���[�V������")]
    public string idleAnimationName = "ZombieIdle";  // �� Idle�ǉ�
    public string walkAnimationName = "ZombieWalk";
    public string attackAnimationName = "ZombieAttack";
    public string hurtAnimationName = "ZombieHurt";
    public string deathAnimationName = "death_01";

    [Header("HP�ݒ�")]
    public int maxHP = 3;

    private int currentHP;
    private Transform target;
    private Animator animator;
    private Slider hpSlider;
    private bool isDead = false;

    void Start()
    {
        FindNewTarget();
        animator = GetComponent<Animator>();

        hpSlider = GetComponentInChildren<Slider>();
        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = maxHP;
            hpSlider.transform.rotation = Quaternion.identity;
        }

        currentHP = maxHP;

        // ������Ԃ�Idle
        PlayAnimation(idleAnimationName);
    }

    void Update()
    {
        if (isDead) return;

        // �^�[�Q�b�g�����Ȃ����Idle��\��
        if (target == null)
        {
            FindNewTarget();
            if (target == null)
            {
                PlayAnimation(idleAnimationName); // �� Idle�Đ�
                return;
            }
        }

        float distance = Vector2.Distance(transform.position, target.position);

        if (distance > attackRange)
        {
            MoveTowardsTarget();
            PlayAnimation(walkAnimationName);
        }
        else
        {
            FaceTarget();
            PlayAnimation(attackAnimationName);
        }

        if (hpSlider != null)
            hpSlider.transform.rotation = Quaternion.identity;
    }

    void FindNewTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(targetTag);
        if (players.Length == 0)
        {
            target = null;
            return;
        }

        GameObject nearest = null;
        float nearestDist = Mathf.Infinity;
        foreach (GameObject p in players)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < nearestDist)
            {
                nearest = p;
                nearestDist = dist;
            }
        }
        target = nearest != null ? nearest.transform : null;
    }

    void MoveTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        FaceDirection(direction);
    }

    void FaceTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        FaceDirection(direction);
    }

    void FaceDirection(Vector2 direction)
    {
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(direction.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    void PlayAnimation(string animationName)
    {
        if (animator != null)
        {
            // ���łɓ����A�j���[�V�������Đ����Ȃ牽�����Ȃ�
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
            {
                animator.Play(animationName);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Ammo"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (hpSlider != null)
            hpSlider.value = currentHP;

        if (currentHP > 0)
        {
            PlayAnimation(hurtAnimationName);
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        PlayAnimation(deathAnimationName);
        StartCoroutine(RemoveAfterDelay(1.25f));
    }

    IEnumerator RemoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    public void AttackHit()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(targetTag);
        if (players.Length == 0) return;

        GameObject nearestPlayer = null;
        float nearestDist = Mathf.Infinity;

        foreach (GameObject p in players)
        {
            float dist = Vector2.Distance(transform.position, p.transform.position);
            if (dist < nearestDist)
            {
                nearestDist = dist;
                nearestPlayer = p;
            }
        }

        if (nearestPlayer != null)
        {
            nearestPlayer.GetComponent<PlayerDebug>()?.TakeDamage(1);
            Debug.Log("�U�����q�b�g�I " + nearestPlayer.name + " �Ƀ_���[�W��^����");
        }
    }
}
