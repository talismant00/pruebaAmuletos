using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flecha : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad de la bala

    public float retrocesoAmount = 1f; // Cantidad de retroceso al impactar
    public float retrocesoDuration = 0.2f; // Duraci�n del retroceso
    // Update is called once per frame
    void Update()
    {
        // Mover la bala hacia adelante en la direcci�n de su eje Z (eje de adelante)
        transform.Translate(Vector2.down * velocidad * Time.deltaTime);
        Destroy(this.gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Vector2 direction = (collision.transform.position - transform.position).normalized;

            //GameObject player = collision.gameObject;
            ////enemigo.TomarDa�o(da�oGolpe);

            //// Calcular la direcci�n de retroceso
            //Vector3 retrocesoDirection = (player.transform.position - transform.position).normalized;

            //// Calcular la posici�n final de retroceso
            //Vector3 targetPosition = player.transform.position + retrocesoDirection * retrocesoAmount;


            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            //StartCoroutine(Retroceso(player.transform, targetPosition, retrocesoDuration));
            Destroy(this.gameObject, 1);
        }
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }


    }
    //public IEnumerator Retroceso(Transform target, Vector3 targetPosition, float duration)
    //{
    //    float elapsedTime = 0f;
    //    Vector3 initialPosition = target.position;

    //    while (elapsedTime < duration)
    //    {
    //        target.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    target.position = targetPosition;
    //}
}
