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
    private int destPoint = 0;
    private Transform player;
    private enum State { Patrolling, Chasing, Waiting }
    private State state;

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
                if (player == null || Vector2.Distance(transform.position, player.position) >= detectionRange)
                {

                    state = State.Patrolling;
                }
                break;
            case State.Waiting:
                if (player != null && Vector2.Distance(transform.position, player.position) < detectionRange)
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
        // Actualiza las variables del animator
        animator.SetFloat("Horizontal", newPos.x - transform.position.x);
        animator.SetFloat("Vertical", newPos.y - transform.position.y);
        animator.SetBool("Moving", true);
        // Reduce el valor aquí para que el enemigo se acerque más al punto
        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            state = State.Waiting;
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
        // Actualiza las variables del animator
        animator.SetFloat("Horizontal", newPos.x - transform.position.x);
        animator.SetFloat("Vertical", newPos.y - transform.position.y);
        animator.SetBool("Moving", true);

    }
}
