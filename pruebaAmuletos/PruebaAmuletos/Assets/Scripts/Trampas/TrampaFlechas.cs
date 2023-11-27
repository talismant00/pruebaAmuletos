using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrampaFlechas : MonoBehaviour
{
    public GameObject balaPrefab; // Prefab de la bala
    public Transform puntoDisparo; // Punto de origen del disparo
    public float velocidadBala = 10f; // Velocidad de la bala
    public float tiempoEntreDisparos = 0.5f; // Tiempo entre cada disparo
    private float tiempoUltimoDisparo; // Tiempo del último disparo
    
    // Update is called once per frame
    private void Start()
    {
        
    }
    void Update()
    {
        //// Verificar si ha pasado el tiempo necesario desde el último disparo
        //if (Time.time > tiempoUltimoDisparo + tiempoEntreDisparos && trampaActivada == true)
        //{
        //    Disparar(); // Llama al método Disparar

        //    tiempoUltimoDisparo = Time.time; // Actualiza el tiempo del último disparo
        //}
    }
    void Disparar()
    {
        // Verificar si ha pasado el tiempo necesario desde el último disparo
        if (Time.time > tiempoUltimoDisparo + tiempoEntreDisparos || tiempoUltimoDisparo == 0f)
        {
            // Instanciar una nueva bala desde el prefab en el punto de disparo
            GameObject nuevaBala = Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);
            // Instanciar dos nuevas balas desde el prefab en el punto de disparo
            GameObject nuevaBala1 = Instantiate(balaPrefab, puntoDisparo.position + new Vector3(-1f, 0f, 0f), puntoDisparo.rotation);
            GameObject nuevaBala2 = Instantiate(balaPrefab, puntoDisparo.position + new Vector3(1f, 0f, 0f), puntoDisparo.rotation);

            tiempoUltimoDisparo = Time.time; // Actualiza el tiempo del último disparo
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Disparar(); // Llama al método Disparar
        }
    }
}
