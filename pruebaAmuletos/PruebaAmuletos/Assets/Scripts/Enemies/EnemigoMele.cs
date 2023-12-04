using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemigoMele : MonoBehaviour
{

    [SerializeField] private float retrocesoAmount; // Distancia de retroceso
    [SerializeField] private float retrocesoDuration;
    public Transform[] points; //Puntos Patrullaje
    public float speed;
    public float detectionRange; //Rango de deteccion del player
    public float waitTime;
    public float attackRange;
    public int attackDamage;
    public float attackDelay;
    [SerializeField] private float radioGolpe;
    [SerializeField] private Transform controladorGolpe;
    private float lastAttackTime;
    private int destPoint = 0;
    private Transform player;
    private enum State { Patrolling, Chasing, Waiting, Attacking, Hit }
    private State state;

    private Animator animator;
    private Rigidbody2D rb;

    string last_printed_state;

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
                if (last_printed_state != "Patrolling")
                {
                    last_printed_state = "Patrolling";
                    Debug.Log("Patrolling");
                }
               
                if (player != null && Vector2.Distance(transform.position, player.position) < detectionRange)
                {
                    state = State.Chasing;
                }
                else if (Vector2.Distance(transform.position, player.position) <= attackRange)
                {
                    state = State.Attacking;
                }
                break;
            case State.Chasing:
                if (last_printed_state != "Chasing")
                {
                    last_printed_state = "Chasing";
                    Debug.Log("Chasing");
                }
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
                if (last_printed_state != "Waiting")
                {
                    last_printed_state = "Waiting";
                    Debug.Log("Waiting");
                }
                if (player != null && Vector2.Distance(transform.position, player.position) < detectionRange)
                {
                    state = State.Chasing;
                }
                else if (Vector2.Distance(transform.position, player.position) <= attackRange)
                {
                    state = State.Attacking;
                }
                break;

            case State.Hit:
                
                break;

            case State.Attacking:
                
                if (Vector2.Distance(transform.position, player.position) > attackRange)
                {
                    state = State.Chasing;
                }
                
                break;
        }

        switch (state)
        {
            case State.Patrolling:
                
                animator.SetBool("Moving", true);
                Patrol();

                break;
            case State.Chasing:
                
                animator.SetBool("Moving", true);
                Chase();
                
                break;

            case State.Waiting:
                animator.SetBool("Moving", false);
                break;

            case State.Hit:
                // Ejecutar animación y lógica para el golpe recibido
                animator.SetTrigger("Hit");
                // Agrega aquí la lógica adicional para manejar el golpe recibido
                break;

            case State.Attacking:
                
                animator.SetBool("Attack", true);
                animator.SetBool("Moving", false);
                Invoke("ResetAttackState", 0.9f);
                //Invoke("Attack", 0.65f);
                

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
        //animator.SetBool("Moving", true);
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

            //animator.SetBool("Attack", true);
            animator.SetBool("Moving", false);


            Debug.Log("Atacando al player");

            // Aquí es donde dañarías al jugador
            //player.GetComponent<Player>().TakeDamage(attackDamage);
            lastAttackTime = Time.time;

            Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
            foreach (Collider2D colisionador in objetos)
            {
                if (colisionador.CompareTag("Player"))
                {
                    PlayerController player = colisionador.transform.GetComponent<PlayerController>();
                    //enemigo.TomarDaño(dañoGolpe);

                    // Calcular la dirección de retroceso
                    Vector3 retrocesoDirection = (player.transform.position - transform.position).normalized;

                    // Calcular la posición final de retroceso
                    Vector3 targetPosition = player.transform.position + retrocesoDirection * retrocesoAmount;

                    // Iniciar la corrutina de retroceso
                    StartCoroutine(Retroceso(player.transform, targetPosition, retrocesoDuration));
                }
            }
        }
        else
        {
            Invoke("ResetAttackState", 0.9f);
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
    void ResetAttackState()
    {
        animator.SetBool("Attack", false);
    }
    public void RecibirGolpe()
    {
        // Inicia la animación de golpe
        animator.SetBool("Hit", true);
        // Agrega cualquier lógica adicional para manejar el golpe recibido
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
