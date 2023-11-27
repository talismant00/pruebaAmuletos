using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampaFlechas : MonoBehaviour
{
    public GameObject balaPrefab; // Prefab de la bala
    public Transform puntoDisparo; // Punto de origen del disparo
    public float velocidadBala = 10f; // Velocidad de la bala
    public float tiempoEntreDisparos = 0.5f; // Tiempo entre cada disparo
    private float tiempoUltimoDisparo; // Tiempo del último disparo

    // Update is called once per frame
    void Update()
    {
        // Verificar si ha pasado el tiempo necesario desde el último disparo
        if (Time.time > tiempoUltimoDisparo + tiempoEntreDisparos)
        {
            Disparar(); // Llama al método Disparar
            tiempoUltimoDisparo = Time.time; // Actualiza el tiempo del último disparo
        }
    }

    void Disparar()
    {
        // Instanciar una nueva bala desde el prefab en el punto de disparo
        GameObject nuevaBala = Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);

    }

}
