using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemigoMele : MonoBehaviour
{
    public Transform[] points;
    public float speed;
    public float detectionRange;
    public float waitTime;
    public float attackRange;
    public int attackDamage;
    public float attackDelay;
    private float lastAttackTime;
    private int destPoint = 0;
    private Transform player;
    private enum State { Patrolling, Chasing, Waiting, Attacking }
    private State state;

    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        state = State.Patrolling;
    }

    void Update()
    {
        switch (state)
        {
            case State.Patrolling:
                Patrol();

                if (player != null && Vector2.Distance(transform.position, player.position) < detectionRange)
                {
                    state = State.Chasing;
                }
                break;
            case State.Chasing:
                Chase();
                if (Vector2.Distance(transform.position, player.position) >= detectionRange)
                {
                    state = State.Patrolling;
                }
                else if (Vector2.Distance(transform.position, player.position) <= attackRange)
                {
                    state = State.Attacking;
                }
                break;
            case State.Waiting:
                if (player != null && Vector2.Distance(transform.position, player.position) < detectionRange)
                {
                    state = State.Chasing;
                }
                break;
            case State.Attacking:
                Attack();
                if (Vector2.Distance(transform.position, player.position) >= attackRange)
                {
                    state = State.Chasing;
                }
                break;
        }
    }

    void Patrol()
    {
        if (points.Length == 0)
            return;

        Vector2 target = points[destPoint].position;
        Vector2 newPos = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        GetComponent<Rigidbody2D>().MovePosition(newPos);
        animator.SetFloat("Horizontal", newPos.x - transform.position.x);
        animator.SetFloat("Vertical", newPos.y - transform.position.y);
        animator.SetBool("Moving", true);
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            state = State.Waiting;
            animator.SetBool("Moving", false);
            StartCoroutine(WaitAtPoint());
        }
    }

    IEnumerator WaitAtPoint()
    {
        yield return new WaitForSeconds(waitTime);
        destPoint = (destPoint + 1) % points.Length;
        state = State.Patrolling;
    }

    void Chase()
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        GetComponent<Rigidbody2D>().MovePosition(newPos);
        animator.SetFloat("Horizontal", newPos.x - transform.position.x);
        animator.SetFloat("Vertical", newPos.y - transform.position.y);
        animator.SetBool("Moving", true);
    }

    void Attack()
    {
        if (Time.time > lastAttackTime + attackDelay)
        {
            
            animator.SetBool("Attack", true);
            animator.SetBool("Moving", false);


            Debug.Log("Atacando al player");
            
            // Aquí es donde dañarías al jugador
            // player.GetComponent<Player>().TakeDamage(attackDamage);
            lastAttackTime = Time.time;
        }
        else
        {
            animator.SetBool("Attack", false);
        }


    }

    public IEnumerator Retroceso(Transform target, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 initialPosition = target.position;

        while (elapsedTime < duration)
        {
            target.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.position = targetPosition;
    }

    
    void OnDrawGizmos()
    {
        // Dibuja el rango de detección
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Dibuja el rango de ataque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
