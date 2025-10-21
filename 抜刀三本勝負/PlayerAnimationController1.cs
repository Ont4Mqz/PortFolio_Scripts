using UnityEngine;

public class PlayerAnimationController1 : MonoBehaviour
{
    public Animator animator1; // 1�ڂ�Animator
    public Animator animator2; // 2�ڂ�Animator

    // animator1�p���\�b�h
    public void PlaySlash1()
    {
        animator1.SetTrigger("Slash");
    }

    public void PlayDamaged1()
    {
        animator1.SetTrigger("Damaged");
    }

    public void PlayDeath1()
    {
        animator1.SetTrigger("Death");
    }

    // animator2�p���\�b�h
    public void PlaySlash2()
    {
        animator2.SetTrigger("Slash");
    }

    public void PlayDamaged2()
    {
        animator2.SetTrigger("Damaged");
    }

    public void PlayDeath2()
    {
        animator2.SetTrigger("Death");
    }

    // ����܂Œʂ�L�[���͂ł��������܂�
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        PlaySlash1();
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        PlayDamaged1();
    //    }
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        PlayDeath1();
    //    }
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        PlaySlash2();
    //    }
    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        PlayDamaged2();
    //    }
    //    if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        PlayDeath2();
    //    }
    //}
}

//���\�b�h�Ăяo���̎�

//public class OtherScript : MonoBehaviour
//{
//    public PlayerAnimationController1 animController;

//    void SomeMethod()
//    {
//        animController.PlayDamaged1(); // Damaged�g���K�[�𔭉�
//    }
//}
