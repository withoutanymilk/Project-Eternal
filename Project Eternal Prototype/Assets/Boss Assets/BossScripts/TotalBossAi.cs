using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TotalBossAi : MonoBehaviour
{
    //health
    int maxHealth = 300;
    int cHealth = 0;



//For animation things
    public Animator anime;
    //For attacking
    public Transform attackPoint1;
    public Transform attackPoint2;
    public Transform attackPoint3;
    public float attackRange = 1.5f;
    public LayerMask playerLayer;
    public int attackDamage = 10;

    //Pathing Declares
    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;
    public Transform BossGFX;
    Path path;
    int cPoint = 0;
    bool reachedEnd = false;


//STATES
    bool isWaiting = false;
    //Stage 
    bool isStage1 = true;
    bool isStage2 = false;
    bool isStage3 = false;
    //Intros
    bool isIntro1 = false;
    bool isIntro2 = false;
    bool isIntro3 = false;
    //Actions
    bool isWalking = true;
    bool isAttacking = false;
    bool isDead = false;



    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        cHealth = maxHealth;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, .5f);

    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            cPoint = 0;
        }
    }

    // Update is called once per frame
    async void Update()
    {
        if (isStage1) // STAGE 1
        {
            Debug.Log(isAttacking+ " " +reachedEnd+ " " + isWaiting);
            if (isWalking)
            {
                WalkingState();
            }
            else if (isAttacking && reachedEnd && !isWaiting)
            {
               // Debug.Log("STARTING ATTACK");
                anime.SetTrigger("Attack");
                isAttacking = false;
                StartCoroutine(Waitfor(1.2f));
               // isWalking = true;
                //isWaiting = false;

            }

        }
        else if (isStage2)  //STAGE 2
        {
            //stage2
        }
        else if (isStage3) //STAGE 3
        {
            //stage3
        }

    }

    



    //STATES BELOW

    void WalkingState()
    {
        //if not in walk state stop

        if (path == null)
            return;

        if (cPoint >= path.vectorPath.Count)
        {
            //CHANGE STATES WE FOUND TARGET
            reachedEnd = true;
            isAttacking = true;
            isWalking = false;
            return;
        }
        else
        {
            reachedEnd = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[cPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[cPoint]);

        if (distance < nextWaypointDistance)
        {
            cPoint++;
        }

        //Flipping the GFX Left and right
        if (rb.velocity.x >= 0.01f && force.x > 0f)
        {
            BossGFX.localScale = new Vector3(-1.1f, 0.8f, 1f);
        }
        else if (rb.velocity.x <= -0.01f && force.x < 0f)
        {
            BossGFX.localScale = new Vector3(1.1f, 0.8f, 1f);
        }

    }

    void AttackingState()
    {
        anime.ResetTrigger("Attack");
        bool hasHit = false;
        Collider2D[] hitZone1 = Physics2D.OverlapCircleAll(attackPoint1.position, attackRange, playerLayer);
        Collider2D[] hitZone2 = Physics2D.OverlapCircleAll(attackPoint2.position, attackRange, playerLayer);
        Collider2D[] hitZone3 = Physics2D.OverlapCircleAll(attackPoint3.position, attackRange * 0.75f, playerLayer);

        foreach (Collider2D player in hitZone1)
        {
            Debug.Log("PLAYER HIT in Zone 1 NAME : " + player.name);
            player.GetComponent<Health>().DamagePlayer(attackDamage);
            hasHit = true;
        }
        

        if (!hasHit)
        {
            foreach (Collider2D player in hitZone2)
            {
                Debug.Log("PLAYER HIT in Zone 2 NAME : " + player.name);
                player.GetComponent<Health>().DamagePlayer(attackDamage);
                hasHit = true;
            }
        }
        if (!hasHit)
        {
            foreach (Collider2D player in hitZone3)
            {
                Debug.Log("PLAYER HIT in Zone 3 NAME : " + player.name);
                player.GetComponent<Health>().DamagePlayer(attackDamage/2);
                hasHit = true;
            }
        }

        Debug.Log("PASSED");
        anime.SetTrigger("AttackEnd");
        Debug.Log("Returning");
    }



    void DeadState()
    {
        Debug.Log("BOSS IS DEAD");
    }

//EXTRAS

    void TakeDamage(int dmg)
    {
        cHealth -= dmg;


        //play hurt anime



        if (cHealth <= 0)
        {
            DeadState();
        }
    }


    void OnDrawGizmosSelected()
    {
        if (attackPoint1 == null || attackPoint2 == null) { return; }


        Gizmos.DrawWireSphere(attackPoint1.position, attackRange);
        Gizmos.DrawWireSphere(attackPoint2.position, attackRange);
        Gizmos.DrawWireSphere(attackPoint3.position, attackRange * 0.75f);
    }


    private IEnumerator Waitfor(float delay)
    {
        yield return new WaitForSeconds(delay);
        isWalking = true;
    }





    
}
