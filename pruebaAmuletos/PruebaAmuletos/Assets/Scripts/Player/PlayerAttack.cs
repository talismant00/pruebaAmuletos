using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float dañoGolpe;
    [SerializeField] private float tiempoEntreAtaques;
    [SerializeField] private float tiempoSiguienteAtaque;

    [SerializeField] private GameObject magia;
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private float retrocesoAmount; // Distancia de retroceso
    [SerializeField] private float retrocesoDuration;
    //private Animator animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (tiempoEntreAtaques > 0)
        //{
        //    tiempoSiguienteAtaque -= Time.deltaTime;
        //}


        //if (Input.GetButtonDown("Fire1") && tiempoSiguienteAtaque <= 0)
        //{
        //    GolpeMele();

        //    tiempoSiguienteAtaque = tiempoEntreAtaques;
        //}
    }

    public void AtaqueCuerpo()
    {
        if (tiempoEntreAtaques > 0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }


        if (tiempoSiguienteAtaque <= 0)
        {
            GolpeMele();

            tiempoSiguienteAtaque = tiempoEntreAtaques;
        }
    }

    public void AtaqueMagia()
    {
        if (tiempoSiguienteAtaque <= 0)
        {
            GolpeMagia();

            tiempoSiguienteAtaque = tiempoEntreAtaques;
        }
    }
    public void GolpeMele()
    {
        // Tu código actual para el golpe melee

        // Agregar retroceso al enemigo
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Enemy"))
            {
                GameObject enemigo = colisionador.gameObject;
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
    private void GolpeMagia()
    {
        Instantiate(magia, controladorDisparo.position, controladorDisparo.rotation);
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}
