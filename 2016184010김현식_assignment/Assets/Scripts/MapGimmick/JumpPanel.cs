using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPanel : MonoBehaviour
{
    [Header("스프링 파워")]
    [SerializeField] float power = 0;

    [Header("스프링 애니메이터")]
    [SerializeField] Animator animator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, power), ForceMode2D.Impulse);

            if (animator.GetBool("IsPlay"))
            {
                animator.SetBool("IsPlay", false);
                animator.SetBool("IsPlay", true);
            }
            else
                animator.SetBool("IsPlay", true);

            Invoke("SetBoolFalse", 0.2f);
        }
    }

    void SetBoolFalse()
    {
        animator.SetBool("IsPlay", false);
    }

}
