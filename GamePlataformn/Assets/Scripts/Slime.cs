using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Rigidbody2D rig;
    private Animator anim;

    public int Health;
    public float Speed;
    public float radius;

    public LayerMask layer;

    public Transform point;


    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rig.velocity = new Vector2(Speed, rig.velocity.y);
        OnCollision();
    }

    private void OnCollision()
    {
        Collider2D hit = Physics2D.OverlapCircle(point.position, radius, layer);

        if(hit!= null)
        {
            Speed = -Speed;

            if (transform.eulerAngles.y == 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }

        }
    }

    public void OnHit()
    {
        anim.SetTrigger("Hit");
        Health--;

        if (Health <= 0)
        {
            anim.SetTrigger("Dead");
            Destroy(gameObject, 0.55f);
            Speed = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);  
    }
}
