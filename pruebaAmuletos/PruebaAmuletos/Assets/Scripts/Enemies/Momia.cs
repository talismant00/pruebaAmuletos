using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Momia : MonoBehaviour
{
    int DebugAttack=0;
    //Rangos
    public float detectionRange = 5f; // Rango de detección
    public float playerRange = 3f; // Rango de ataque al jugador
    public float AttackRange = 2f; // Rango de ataque al jugador
    //Valores
    public float movementSpeed = 3f; // Velocidad de movimiento
    [SerializeField] private float retrocesoAmount; // Distancia de retroceso
    [SerializeField] private float retrocesoDuration; //Duracion de retroceso
    [SerializeField] private float radioGolpe;
    [SerializeField] private float dañoGolpe;
    [SerializeField] private float tiempoEntreAtaques;
    [SerializeField] private float tiempoSiguienteAtaque;
    //Player
    private string playerTag = "Player"; // Etiqueta del jugador
    private Transform player;
    
    private Rigidbody2D rb;
    [SerializeField] private Transform controladorGolpe;
    public Transform[] puntosDePatrullaje;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag(playerTag).transform;
    }

    private void Update()
    {
        bool isChasing = CheckForPlayer();
        bool isAnotherEnemyNearby = CheckForOtherEnemies();

        if (isChasing && isAnotherEnemyNearby)
        {
            ChasePlayer();
        }
        else if (!isChasing)
        {
            StopChase();
        }
        else
        {
            FleePlayer();
        }
    }

    private bool CheckForPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag(playerTag))
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);
                return distanceToPlayer <= detectionRange;
            }
        }

        return false;
    }

    private bool CheckForOtherEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") && collider.gameObject != gameObject)
            {
                // Si hay otro enemigo cerca, devuelve true
                return true;
            }
        }

        // Si no hay otro enemigo cerca, devuelve false
        return false;
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= AttackRange)
        {
            AttackPlayer();
        }
        else
        {
            DebugAttack = 0;
            rb.velocity = direction * movementSpeed;
        }
    }

    private void StopChase()
    {
        rb.velocity = Vector2.zero;
    }

    private void AttackPlayer()
    {
        if (DebugAttack == 0)
        {
            DebugAttack = 1;
            // Implementa aquí tu lógica de ataque al jugador
            Debug.Log("¡Atacar al jugador!");
        }

        // Agregar retroceso al enemigo
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Player"))
            {
                PlayerController enemigo = colisionador.transform.GetComponent<PlayerController>();
                //enemigo.TomarDaño(dañoGolpe);

                // Calcular la dirección de retroceso
                Vector3 retrocesoDirection = (enemigo.transform.position - transform.position).normalized;

                // Calcular la posición final de retroceso
                Vector3 targetPosition = enemigo.transform.position + retrocesoDirection * retrocesoAmount;

                // Iniciar la corrutina de retroceso
                StartCoroutine(Retroceso(enemigo.transform, targetPosition, retrocesoDuration));
            }
        }


    }

    private void FleePlayer()
    {
        Vector2 direction = (transform.position - player.position).normalized;
        rb.velocity = direction * movementSpeed;
        Debug.Log("huir");
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, playerRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radioGolpe);
    }
}

