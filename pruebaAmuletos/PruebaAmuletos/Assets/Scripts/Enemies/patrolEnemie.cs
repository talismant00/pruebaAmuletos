using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class patrolEnemie : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float speed;
    [SerializeField] private float waitTime;

    private int currentWayPoint;
    private bool isWaiting;

    void Update()
    {
        if(transform.position != wayPoints[currentWayPoint].position)
        {
            transform.position = Vector2.MoveTowards(transform.position, wayPoints[currentWayPoint].position, speed * Time.deltaTime);
        }
        if(!isWaiting)
        {
            Debug.Log("Esperando en el waypoint " + currentWayPoint);
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        currentWayPoint++;

        if(currentWayPoint == wayPoints.Length)
        {
            currentWayPoint = 0;
        }
        isWaiting = false;

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
}
