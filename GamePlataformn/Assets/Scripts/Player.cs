using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rig;

    public float Speed;
    public float JumpForce;
    public float radius;
    float recoveryCount;
    public float recoveryTime;

    private Health healthSystem;

    private bool isJumping;
    private bool doubleJump;
    private bool isAttacking;
    private bool recovery;


    public LayerMask enemyLayer;
    public Animator anim;
    public Transform point;
    private PlayerAudio playerAudio;


    private static Player instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();

        playerAudio = GetComponent<PlayerAudio>();

        healthSystem = GetComponent<Health>();

        recoveryCount = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        Jumnp();
        Attack();
    }

    void FixedUpdate()
    {
        Move();
        recoveryCount += Time.deltaTime;

    }

    public void Move()
    {
        float movement = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(movement * Speed, rig.velocity.y);

        if (movement > 0)
        {
            if (!isJumping && !isAttacking)
            {
                anim.SetInteger("Transition", 1);
            } 
            transform.eulerAngles = new Vector3(0,0,0);
        }
        if (movement < 0)
        {
            if(!isJumping && !isAttacking)
            {
                anim.SetInteger("Transition", 1);
            }
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if(movement == 0 && !isJumping && !isAttacking)
        {
            anim.SetInteger("Transition", 0);
        }
    }

    public void Jumnp()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!isJumping)
            {
                anim.SetInteger("Transition", 2);
                rig.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                isJumping = true;
                doubleJump = true;
                playerAudio.PlaySFX(playerAudio.jumpSound);
            }
            else if (doubleJump)
            {
                anim.SetInteger("Transition", 2);
                rig.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                doubleJump = false;
                playerAudio.PlaySFX(playerAudio.jumpSound); 
            }
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isAttacking = true;
            anim.SetInteger("Transition", 3);
            Collider2D hit = Physics2D.OverlapCircle(point.position, radius, enemyLayer);
            playerAudio.PlaySFX(playerAudio.hitSound);

            if (hit != null)
            {

                if (hit.GetComponent<Slime>())
                {
                    hit.GetComponent<Slime>().OnHit();
                }

                if (hit.GetComponent<Goblin>())
                {
                    hit.GetComponent<Goblin>().OnHit();
                }

                
            }
            StartCoroutine(OnAttack());
        }
       
    }

    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.333f);
         isAttacking = false;
    }

    public void OnHit()
    {


        if (recoveryCount >= recoveryTime)
        {
            Debug.Log("na teoria tá pegando");
            recoveryCount = 0f;
            healthSystem.health--;

            anim.SetTrigger("Hit");
        }

        if (healthSystem.health <= 0)
        {
            anim.SetTrigger("Dead");
            Destroy(gameObject, 1f);    
            GameController.instance.ShowGameOver();
        }
    }


   void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(point.position, radius);
    }

    private void OnCollisionEnter2D(Collision2D colisor)
    {
        if(colisor.gameObject.layer == 6)
        {
            isJumping = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            OnHit();
        }

        if (collision.CompareTag("Coin"))
        {
            playerAudio.PlaySFX(playerAudio.coinSound);
            collision.GetComponent<Animator>().SetTrigger("Hit");
            GameController.instance.GetCoin();
            Destroy(collision.gameObject, 0.4f);
        }

        if (collision.gameObject.layer == 11)
        {
            GameController.instance.ShowGameOver();
        }

        if (collision.gameObject.layer == 12)
        {
            GameController.instance.ShowGameWin();
        }
    }

}
