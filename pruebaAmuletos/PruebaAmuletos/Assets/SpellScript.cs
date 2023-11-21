using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
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
        target = GameObject.FindGameObjectWithTag("Enemy");
        Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
        bulletRB.velocity = new Vector2(moveDir.x, moveDir.y);
        Destroy(this.gameObject, 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;

            GameObject enemy = collision.gameObject;
            //enemigo.TomarDaño(dañoGolpe);

            // Calcular la dirección de retroceso
            Vector3 retrocesoDirection = (enemy.transform.position - transform.position).normalized;

            // Calcular la posición final de retroceso
            Vector3 targetPosition = enemy.transform.position + retrocesoDirection * retrocesoAmount;


            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(Retroceso(enemy.transform, targetPosition, retrocesoDuration));
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
