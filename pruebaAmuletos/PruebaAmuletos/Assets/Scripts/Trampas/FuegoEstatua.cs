using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuegoEstatua : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Daño Player");
        }

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Daño Enemigo");
        }
    }
    
    void ApagarFuego()
    {
        gameObject.SetActive(false);
    }
}
