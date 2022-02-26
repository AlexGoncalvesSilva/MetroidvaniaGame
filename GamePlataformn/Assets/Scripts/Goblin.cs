using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{

    private Rigidbody2D rig;

    private bool isDead;

    private bool isFront;
    public bool isRight;

    private Vector2 direction;

    public Transform pointFront;
    public Transform pointBack;

    public float Speed;
    public float maxVision;
    public float stopDistance;
    public int Health;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();


        if (isRight)
        {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.right;
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 180);
            direction = Vector2.left;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    void FixedUpdate()
    {
        GetPlayer();
        OnMove();
    }

    void OnMove()
    {

        if (!isFront && !isDead)
        {
            rig.velocity = new Vector2(0, rig.velocity.y);
        }

        if (isFront && !isDead)
        {
          
            if (isRight)
            {
                transform.eulerAngles = new Vector2(0, 0);
                direction = Vector2.right;
                rig.velocity = new Vector2(Speed, rig.velocity.y);
                anim.SetInteger("Transition", 1);
            }
            else
            {
                transform.eulerAngles = new Vector2(0, 180);
                direction = Vector2.left;
                rig.velocity = new Vector2(-Speed, rig.velocity.y);
                anim.SetInteger("Transition", 1);
            }
        }
    }

    void GetPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(pointFront.position, direction, maxVision);


        if (hit.collider != null && !isDead)
        {
            if (hit.transform.CompareTag("Player"))
            {
                isFront = true;

                float distance = Vector2.Distance(transform.position, hit.transform.position);

                if (distance <= stopDistance)
                {
                  
                    isFront = false;
                    rig.velocity = Vector2.zero;
                    hit.transform.GetComponent<Player>().OnHit();

                    anim.SetInteger("Transition", 2);
                }
            }
        }

        RaycastHit2D hitBack = Physics2D.Raycast(pointBack.position, -direction, maxVision);

        if (hitBack.collider != null)
        {
            if (hitBack.transform.CompareTag("Player"))
            {
                isRight = !isRight;
                isFront = true;
            }
        }

    }

    public void OnHit()
    {
        anim.SetTrigger("Hit");
        Health--;

        if (Health <= 0)
        {
            isDead = true;
            anim.SetTrigger("Dead");
            Destroy(gameObject, 1f);
            Speed = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawRay(pointFront.position, direction * maxVision); 
    }

}
