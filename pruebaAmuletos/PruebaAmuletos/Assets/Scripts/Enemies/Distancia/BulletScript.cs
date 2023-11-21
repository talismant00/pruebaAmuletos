using UnityEngine;
using System.Collections;
using TMPro;


public class BulletScript : MonoBehaviour
{
    GameObject target;
    public float speed;
    Rigidbody2D bulletRB;
    public float retrocesoAmount = 1f; // Cantidad de retroceso al impactar
    public float retrocesoDuration = 0.2f; // Duración del retroceso

    private Transform player;
    public void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
        bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);
        Destroy(this.gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;

            GameObject player = collision.gameObject;
            //enemigo.TomarDaño(dañoGolpe);

            // Calcular la dirección de retroceso
            Vector3 retrocesoDirection = (player.transform.position - transform.position).normalized;

            // Calcular la posición final de retroceso
            Vector3 targetPosition = player.transform.position + retrocesoDirection * retrocesoAmount;


            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(Retroceso(player.transform, targetPosition, retrocesoDuration));
            Destroy(this.gameObject, 1);
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

}

