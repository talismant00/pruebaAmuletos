using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class EnemigoDistanciaOG : MonoBehaviour
{
    public Transform jugador; // Referencia al transform del jugador
    public float rangoDisparo = 5f; // Rango de disparo del enemigo
    public float distanciaMinima = 2.5f; // Distancia mínima que el enemigo debe mantener respecto al jugador
    public float velocidadRetroceso = 2f; // Velocidad a la que el enemigo retrocede
    public GameObject balaPrefab; // Prefab de la bala
    public Transform puntoDisparo; // Punto desde donde saldrá la bala
    public float tiempoEntreDisparos = 2f; // Tiempo entre cada disparo
    private bool puedeDisparar = true; // Indica si el enemigo puede disparar

    void Update()
    {
        float distanciaAlJugador = Vector3.Distance(transform.position, jugador.position);

        if (distanciaAlJugador <= rangoDisparo && puedeDisparar)
        {
            DispararAlJugador();
            StartCoroutine(EsperarParaDisparar());
        }

        if (distanciaAlJugador < distanciaMinima)
        {
            Vector3 direccionRetroceso = (transform.position - jugador.position).normalized;
            transform.position += direccionRetroceso * velocidadRetroceso * Time.deltaTime;
        }
    }

    void DispararAlJugador()
    {
        Debug.Log("¡El enemigo dispara al jugador!");
        GameObject bala = Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);
    }

    System.Collections.IEnumerator EsperarParaDisparar()
    {
        puedeDisparar = false;
        yield return new WaitForSeconds(tiempoEntreDisparos);
        puedeDisparar = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangoDisparo);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanciaMinima);
    }
}

