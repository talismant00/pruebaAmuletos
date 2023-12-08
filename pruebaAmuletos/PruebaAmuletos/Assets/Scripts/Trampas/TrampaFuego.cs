using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampaFuego : MonoBehaviour
{

    public GameObject fuego;
    // Start is called before the first frame update
    void Start()
    {
        fuego.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Activa la trampa
            fuego.SetActive(true);
        }


    }
}
