using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButtom : MonoBehaviour
{
    private Animator anim;
    public Animator barrierAnim;
    public Animator barrierAnim2;

    public float radius;
    public LayerMask layer;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    void OnPressed()
    {
        anim.SetBool("isPressed", true);
        barrierAnim.SetBool("isPressed", true);
        barrierAnim2.SetBool("isPressed", true);
    }

    void OnExit()
    {
        anim.SetBool("isPressed", false);
        barrierAnim.SetBool("isPressed", false);
        barrierAnim2.SetBool("isPressed", false);
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if(collision.gameObject.CompareTag("Stone"))
    //    {
    //        OnPressed();
    //    }     
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Stone"))
    //    {
    //        OnExit();
    //    }
    //}

    private void FixedUpdate()
    {
        OnCollision();
    }

    private void OnCollision()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, radius, layer);

        if (hit != null)
        {
            OnPressed();
            hit = null;
        }
        else
        {
            OnExit();
        }

      
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
