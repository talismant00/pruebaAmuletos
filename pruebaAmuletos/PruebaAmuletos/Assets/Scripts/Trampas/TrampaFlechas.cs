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
    private float tiempoUltimoDisparo; // Tiempo del �ltimo disparo
    private bool trampaActivada;
    // Update is called once per frame
    private void Start()
    {
        trampaActivada = false;
    }
    void Update()
    {
        // Verificar si ha pasado el tiempo necesario desde el �ltimo disparo
        if (Time.time > tiempoUltimoDisparo + tiempoEntreDisparos && trampaActivada == true)
        {
            Disparar(); // Llama al m�todo Disparar
           
            tiempoUltimoDisparo = Time.time; // Actualiza el tiempo del �ltimo disparo
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            trampaActivada = true;
        }
        else if (!other.gameObject.CompareTag("Player"))
        {
            trampaActivada = false;
        }
    }
    
    void Disparar()
    {
        // Instanciar una nueva bala desde el prefab en el punto de disparo
        GameObject nuevaBala = Instantiate(balaPrefab, puntoDisparo.position, puntoDisparo.rotation);

        // Instanciar dos nuevas balas desde el prefab en el punto de disparo
        GameObject nuevaBala1 = Instantiate(balaPrefab, puntoDisparo.position + new Vector3(-1f, 0f, 0f), puntoDisparo.rotation);
        GameObject nuevaBala2 = Instantiate(balaPrefab, puntoDisparo.position + new Vector3(1f, 0f, 0f), puntoDisparo.rotation);
    }

}
