using System.Collections;
using UnityEngine;

public class patrolEnemie : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;

    private int currentWayPoint = 0; // Inicialmente, el enemigo comienza desde el primer punto de patrullaje

    void Start()
    {
        StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            yield return StartCoroutine(MoveToNextWaypoint());
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator MoveToNextWaypoint()
    {
        int nextWayPoint = (currentWayPoint + 1) % wayPoints.Length;
        Vector3 targetPosition = wayPoints[nextWayPoint].position;

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        currentWayPoint = nextWayPoint;
        Flip();
    }

private void Flip()
    {
        if(transform.position.x > wayPoints[currentWayPoint].position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            Debug.Log("Cambia de direccion");
            Flip();
        }
    }
}


