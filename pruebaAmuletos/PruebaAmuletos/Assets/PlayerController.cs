using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    enum States { Idle, Walk, MeleeAttack, RangeAttack };
    public float speed = 5f;
    Rigidbody2D m_rb;
    States m_state;
    PlayerAttack playerAttack;
    //control debug
    string last_printed_state;

    private Animator animator;

    Vector3 change;
    private Vector3 lastMoveInput = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody2D>();
        m_state = States.Idle;
        playerAttack = GetComponent<PlayerAttack>();
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
                movex = Input.GetAxisRaw("Horizontal");
                movey = Input.GetAxisRaw("Vertical");
                

                if (movex != 0 || movey != 0)
                {
                    lastMoveInput = new Vector3(movex, movey, 0f).normalized;
                    m_state = States.Walk;
                    change = Vector3.zero;
                    change.x = Input.GetAxisRaw("Horizontal");
                    change.y = Input.GetAxisRaw("Vertical");

                    animator.SetFloat("Horizontal", change.x);
                    animator.SetFloat("Vertical", change.y);
                    animator.SetBool("Moving", true);
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
                    animator.SetBool("Moving", false);
                }
                break;

            case States.MeleeAttack:

                if (last_printed_state != "meleattack")
                {
                    last_printed_state = "meleattack";
                    Debug.Log("meleattack");
                }
                if ((m_rb.velocity.y == 0) && (m_rb.velocity.x == 0))
                {
                    m_state = States.Idle;
                }
                
                else if ((m_rb.velocity.y != 0) && (m_rb.velocity.x != 0))
                {
                    m_state = States.Walk;
                }

                
                break;

            case States.RangeAttack:

                if (last_printed_state != "rangeattack")
                {
                    last_printed_state = "rangeattack";
                    Debug.Log("rangeattack");
                }

                if ((m_rb.velocity.y == 0) && (m_rb.velocity.x == 0))
                {
                    m_state = States.Idle;
                }

                else if ((m_rb.velocity.y != 0) && (m_rb.velocity.x != 0))
                {
                    m_state = States.Walk;
                    
                }
                break;


        }

        switch (m_state)
        {
            case States.Idle:

                
                // Animación de idle
                if (Input.GetButtonDown("Fire1"))
                {
                    
                    animator.SetBool("Attacking",true);
                    m_state = States.MeleeAttack;
                    playerAttack.AtaqueCuerpo(); // Activar el ataque melee
                    
                }
                else
                {
                    animator.SetBool("Attacking", false);
                }
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetBool("Spell", true);
                    m_state = States.RangeAttack;
                    playerAttack.AtaqueMagia(); // Activar el ataque melee
                    //m_rb.velocity = Vector2.zero;
                }
                else
                {
                    animator.SetBool("Spell", false);
                }

                break;



            case States.Walk:


                //agafem els inputs
                movex = Input.GetAxisRaw("Horizontal");
                movey = Input.GetAxisRaw("Vertical");

                // Calcular el vector de movimiento
                Vector2 movement = new Vector2(movex, movey) * speed;

                // Aplicar la fuerza al Rigidbody2D para mover al personaje
                m_rb.velocity = movement;

                //animator.SetFloat("Horizontal", movement.x);
                //animator.SetFloat("Vertical", movement.y);
                //animator.SetFloat("Speed", movement.sqrMagnitude);

                
                // Voltear el sprite según la dirección.
                if (movex < 0.0f)
                {
                    // Voltear el sprite hacia la izquierda.
                }
                else if (movex > 0.0f)
                {
                    // Voltear el sprite hacia la derecha.
                }
                if (Input.GetButtonDown("Fire1"))
                {
                    
                    animator.SetFloat("Horizontal", movex);
                    animator.SetFloat("Vertical", movey);

                    animator.SetBool("Attacking", true);
                    m_state = States.MeleeAttack;
                    m_rb.velocity = Vector2.zero;
                    playerAttack.AtaqueCuerpo(); // Activar el ataque melee


                }

                else
                {
                    animator.SetBool("Attacking", false);
                }
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetFloat("Horizontal", movex);
                    animator.SetFloat("Vertical", movey);

                    animator.SetBool("Spell", true);
                    m_state = States.RangeAttack;
                    playerAttack.AtaqueMagia(); // Activar el ataque magico
                    m_rb.velocity = Vector2.zero;
                }
                else
                {
                    animator.SetBool("Spell", false);
                }
                break;

            case States.MeleeAttack:
                // En este estado, se deja que el script PlayerAttack maneje el ataque, así que no se necesita lógica aquí.
                
                break;

            case States.RangeAttack:

                break;
        }


    }
}
