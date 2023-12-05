using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RangeEnemy : MonoBehaviour
{
    public float speed;

    public Transform[] puntosPatrulla;
    int indicePuntoActual = 0;

    private float tiempoEspera = 0f;


    public Transform jugador; // Referencia al transform del jugador
    public float rangoDisparo = 5f; // Rango de disparo del enemigo
    public float distanciaMinima = 2.5f; // Distancia mínima que el enemigo debe mantener respecto al jugador
    public float velocidadRetroceso = 2f; // Velocidad a la que el enemigo retrocede
    public GameObject balaPrefab; // Prefab de la bala
    public Transform puntoDisparo; // Punto desde donde saldrá la bala
    public float tiempoEntreDisparos = 2f; // Tiempo entre cada disparo
    private bool puedeDisparar = true; // Indica si el enemigo puede disparar
    string last_printed_state;
    private Animator animator;

    enum States { Idle, Walk, MantenerRango, RangeAttack };
    States m_state;

    private void Start()
    {
        animator = GetComponent<Animator>();
        
        m_state = States.Walk;
    }
    void Update()
    {
        if (tiempoEspera > 0)
        {
            tiempoEspera -= Time.deltaTime;
        }


        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);
        
        switch (m_state)
        {
            case States.Idle:

                //DEBUG
                if (last_printed_state != "idle")
                {
                    last_printed_state = "idle";
                    Debug.Log("idle");
                }

                else if (distanciaAlJugador >= rangoDisparo && distanciaAlJugador > distanciaMinima)
                {
                    m_state = States.Walk;
                }
                
                if (distanciaAlJugador <= rangoDisparo && puedeDisparar)
                {
                    m_state = States.RangeAttack;
                }

                if (distanciaAlJugador < distanciaMinima)
                {
                    m_state = States.MantenerRango;
                }
                break;



            case States.Walk:

                //DEBUG
                if (last_printed_state != "walk")
                {
                    last_printed_state = "walk";
                    Debug.Log("walk");
                }

                if (distanciaAlJugador <= rangoDisparo)
                {
                    m_state = States.RangeAttack;
                }

                if (distanciaAlJugador < distanciaMinima)
                {
                    m_state = States.MantenerRango;
                }
                if (transform.position == Vector3.zero)
                {
                    m_state = States.Idle;
                }

                break;

            case States.MantenerRango:

                if (distanciaAlJugador > distanciaMinima && distanciaAlJugador <= rangoDisparo && puedeDisparar)
                {
                    m_state = States.RangeAttack;
                }
                else if (distanciaAlJugador > distanciaMinima)
                {
                    m_state = States.Walk;
                }
                else if (transform.position == Vector3.zero)
                {
                    m_state = States.Idle;
                }
                

                break;

            case States.RangeAttack:

                if (distanciaAlJugador > rangoDisparo)
                {
                    m_state = States.Walk;
                }
                else if (transform.position == Vector3.zero)
                {
                    m_state = States.Idle;
                }


                break;


        }
        /////////////////////////////////////////////////////////////
        switch (m_state)
        {
            case States.Idle:

                animator.SetBool("Moving", false);

                break;

            case States.Walk:

                animator.SetBool("Moving", true);
                
                if (distanciaAlJugador <= rangoDisparo && puedeDisparar)
                {
                    animator.SetBool("Moving", false);


                    animator.SetBool("Attack", true);
                    //Invoke("DispararAlJugador",0.6f);
                    tiempoEspera = tiempoEntreDisparos;
                    Invoke("ResetAttackState", 0.7f);
                }
                else if (distanciaAlJugador <= distanciaMinima)
                {
                    MantenerRango();
                }
                else
                {
                    MoveToNextPatrolPoint();
                }
                
                break;

            case States.MantenerRango:
                animator.SetBool("Moving", true);

                if (distanciaAlJugador < distanciaMinima)
                {
                    MantenerRango();
                }
                
                break;

            case States.RangeAttack:
                animator.SetBool("Moving", false);

                if (distanciaAlJugador <= rangoDisparo && puedeDisparar && tiempoEspera <= 0)
                {
                    animator.SetBool("Moving", false);

                    animator.SetBool("Attack", true);
                    

                    Vector3 direccionAlJugador = (jugador.position - transform.position).normalized;

                    // Asignar parámetros a la animación según la dirección relativa al jugador
                    animator.SetFloat("Horizontal", direccionAlJugador.x);
                    animator.SetFloat("Vertical", direccionAlJugador.y);

                    // Determinar la dirección principal y establecer la animación de ataque
                    if (Mathf.Abs(direccionAlJugador.x) > Mathf.Abs(direccionAlJugador.y))
                    {
                        animator.SetFloat("Horizontal", Mathf.Sign(direccionAlJugador.x));
                        animator.SetFloat("Vertical", 0);
                    }
                    else if (transform.position == Vector3.zero)
                    {
                        animator.SetBool("Moving", false);
                    }
                    else
                    {
                        animator.SetFloat("Horizontal", 0);
                        animator.SetFloat("Vertical", Mathf.Sign(direccionAlJugador.y));
                    }
                    //Invoke("DispararAlJugador",0.6f);
                    tiempoEspera = tiempoEntreDisparos;
                    Invoke("ResetAttackState", 0.7f);
                }
                else if (puedeDisparar && tiempoEspera <= 0)
                {
                    animator.SetBool("Moving", false);
                    animator.SetBool("Attack", true);
                    //DispararAlJugador();
                    tiempoEspera = tiempoEntreDisparos;
                    Invoke("ResetAttackState", 0.7f);
                }


                break;
        }
    }

    void ResetAttackState()
    {
        animator.SetBool("Attack", false);
    }



    public void DispararAlJugador()
    {
        Debug.Log("¡El enemigo dispara al jugador!");
        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);


        // Obtener la dirección entre el enemigo y el jugador
        Vector3 direccionAlJugador = (jugador.position - transform.position).normalized;

        //Asignar parámetros a la animación según la dirección relativa al jugador
        //animator.SetFloat("Horizontal", direccionAlJugador.x);
        //animator.SetFloat("Vertical", direccionAlJugador.y);

        //Determinar la dirección principal y establecer la animación de ataque
        //if (Mathf.Abs(direccionAlJugador.x) > Mathf.Abs(direccionAlJugador.y))
        //{
        //    animator.SetFloat("Horizontal", Mathf.Sign(direccionAlJugador.x));
        //    animator.SetFloat("Vertical", 0);
        //}
        //else if (transform.position == Vector3.zero)
        //{
        //    animator.SetBool("Moving", false);
        //}
        //else
        //{
        //    animator.SetFloat("Horizontal", 0);
        //    animator.SetFloat("Vertical", Mathf.Sign(direccionAlJugador.y));
        //}


    }
    void MantenerRango()
    {

        Vector3 direccionRetroceso = (transform.position - jugador.position).normalized;
        transform.position += direccionRetroceso * velocidadRetroceso * Time.deltaTime;
        if (direccionRetroceso != Vector3.zero)
        {
            // Si se está moviendo, activa la animación de movimiento
            animator.SetBool("Moving", true);
            animator.SetFloat("Horizontal", direccionRetroceso.x);
            animator.SetFloat("Vertical", direccionRetroceso.y);
        }
        else if (transform.position == Vector3.zero)
        {
            animator.SetBool("Moving", false);
        }

    }


    void MoveToNextPatrolPoint()
    {
        if (puntosPatrulla.Length > 0)
        {
            Vector3 objetivo = puntosPatrulla[indicePuntoActual].position;
            float distanciaAlObjetivo = Vector3.Distance(transform.position, objetivo);

            if (distanciaAlObjetivo > 0.1f)
            {
                // Mover hacia el punto de patrulla
                Vector3 direccion = (objetivo - transform.position).normalized;
                transform.position += direccion * speed * Time.deltaTime;
                animator.SetFloat("Horizontal", direccion.x);
                animator.SetFloat("Vertical", direccion.y);

            }
            else
            {
                // Cambiar al siguiente punto de patrulla
                indicePuntoActual = (indicePuntoActual + 1) % puntosPatrulla.Length;
            }
        }
    }

    //System.Collections.IEnumerator EsperarParaDisparar()
    //{
    //    puedeDisparar = false;
    //    yield return new WaitForSeconds(tiempoEntreDisparos);
    //    puedeDisparar = true;
    //}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDisparo);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanciaMinima);
    }
}
