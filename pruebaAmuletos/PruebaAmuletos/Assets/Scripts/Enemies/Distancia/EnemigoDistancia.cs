using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemigoDistancia : MonoBehaviour
{
    public Transform[] points;
    private int destPoint = 0;
    public float speed;

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
        Patrol();
        m_state = States.Walk;
    }
    void Update()
    {
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

                if (distanciaAlJugador <= rangoDisparo && puedeDisparar)
                {
                    m_state = States.RangeAttack;
                }

                if (distanciaAlJugador < distanciaMinima)
                {
                    m_state = States.MantenerRango;
                }

                break;

            case States.MantenerRango:

                if (distanciaAlJugador > distanciaMinima)
                {
                    m_state = States.Walk;
                }

                break;

            case States.RangeAttack:
                
                if (distanciaAlJugador < distanciaMinima)
                {
                    m_state = States.MantenerRango;
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
                
                break;

            case States.MantenerRango:
                animator.SetBool("Moving", true);

                if (distanciaAlJugador < distanciaMinima)
                {
                    MantenerRango();
                }

                break;

            case States.RangeAttack:
                
                if (distanciaAlJugador <= rangoDisparo && puedeDisparar)
                {
                    animator.SetBool("Moving", false);
                    //animator.SetBool("Attack", true);
                    DispararAlJugador();
                    //StartCoroutine(EsperarParaDisparar());
                }

                break;
        }
    }

    


    void DispararAlJugador()
    {
        Debug.Log("¡El enemigo dispara al jugador!");
        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);
    }
    void MantenerRango()
    {
        
        Vector3 direccionRetroceso = (transform.position - jugador.position).normalized;
        transform.position += direccionRetroceso * velocidadRetroceso * Time.deltaTime;
        
    }

    public void Patrol(UnityEngine.AI.NavMeshAgent agent, Transform[] points)
    {
        if (points.Length == 0)
            return;

        int destPoint = UnityEngine.Random.Range(0, points.Length);
        agent.destination = points[destPoint].position;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            Patrol(agent, points);
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

