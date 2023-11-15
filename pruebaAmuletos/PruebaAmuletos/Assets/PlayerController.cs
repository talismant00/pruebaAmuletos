using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum States { Idle, Walk };
    public float speed = 5f;
    Rigidbody2D m_rb;
    States m_state;

    //control debug
    string last_printed_state;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_state = States.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        float movex;
        float movey;

        switch (m_state)
        {
            case States.Idle:

                //DEBUG
                if (last_printed_state != "idle")
                {
                    last_printed_state = "idle";
                    Debug.Log("idle");
                }


                //agafem els inputs
                movex = Input.GetAxis("Horizontal");
                movey = Input.GetAxis("Vertical");

                if (movex != 0 || movey != 0)
                {
                    m_state = States.Walk;

                }
                else if ((m_rb.velocity.y != 0) && (m_rb.velocity.x != 0))
                {
                    m_state = States.Walk;
                }
                break;

            case States.Walk:

                //DEBUG
                if (last_printed_state != "walk")
                {
                    last_printed_state = "walk";
                    Debug.Log("walk");
                }

                if ((m_rb.velocity.y == 0) && (m_rb.velocity.x == 0))
                {
                    m_state = States.Idle;
                }
                break;


        }

        switch (m_state)
        {
            case States.Idle:
                // Animación de idle
                break;



            case States.Walk:


                //agafem els inputs
                movex = Input.GetAxis("Horizontal");
                movey = Input.GetAxis("Vertical");

                // Calcular el vector de movimiento
                Vector2 movement = new Vector2(movex, movey) * speed;

                // Aplicar la fuerza al Rigidbody2D para mover al personaje
                m_rb.velocity = movement;


                // Voltear el sprite según la dirección.
                if (movex < 0.0f)
                {
                    // Voltear el sprite hacia la izquierda.
                }
                else if (movex > 0.0f)
                {
                    // Voltear el sprite hacia la derecha.
                }


                break;
        }


    }
}
