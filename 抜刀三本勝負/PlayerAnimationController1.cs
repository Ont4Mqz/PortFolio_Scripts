using UnityEngine;

public class PlayerAnimationController1 : MonoBehaviour
{
    public Animator animator1; // 1つ目のAnimator
    public Animator animator2; // 2つ目のAnimator

    // animator1用メソッド
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

    // animator2用メソッド
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

    // これまで通りキー入力でも動かせます
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

//メソッド呼び出しの時

//public class OtherScript : MonoBehaviour
//{
//    public PlayerAnimationController1 animController;

//    void SomeMethod()
//    {
//        animController.PlayDamaged1(); // Damagedトリガーを発火
//    }
//}
