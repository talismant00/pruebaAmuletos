using UnityEngine;
using System.Collections;


public class BulletScript : MonoBehaviour
{
    GameObject target;
    public float speed;
    Rigidbody2D bulletRB;
    public float retrocesoAmount = 1f; // Cantidad de retroceso al impactar
    public float retrocesoDuration = 0.2f; // Duración del retroceso

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
        Vector2 direction = (collision.transform.position - transform.position).normalized;

        Vector2 knockback = direction * retrocesoAmount;

        
        
    }

    
}

