using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerAttack playerAttack;
    //movimiento
    [SerializeField] private float velocidadMovimiento = 8f;
    [SerializeField] private float fuerzaSalto = 10f;

    //deteccion de suelo
    [SerializeField] private Transform controladorSuelo; 
    [SerializeField] private float radioDeteccion = 0.2f;
    [SerializeField] private LayerMask EsSuelo; 

    //Habilidades, Al principio todas son falsas (false). Las activaremos al recoger máscaras.
    public bool puedeAtacar = false;      // Nivel 2
    public bool puedeDobleSalto = false;  // Nivel 3
    public bool puedeCorrer = false;        // Nivel 4

    private Rigidbody2D rb;
    private float inputHorizontal;
    public bool Suelo;
    private bool mirandoDerecha = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (playerAttack == null) playerAttack = GetComponent<PlayerAttack>();
    }

    void Update()
    {
        // 1. Input de Movimiento (A = -1, D = 1) 
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        // 2. Input de Salto 
        // Solo permitimos saltar si detectamos suelo
        if (Input.GetButtonDown("Jump") && Suelo)
        {
            Saltar();
        }

     

        //  ATAQUE 
        if (Input.GetButtonDown("Fire1") && puedeAtacar && !playerAttack.estaAtacando)
        {
           playerAttack.RealizarAtaque();
        }
        // VOLTEAR (No permitimos voltear si está atacando)
        if (!playerAttack.estaAtacando)
        {
            if (inputHorizontal > 0 && !mirandoDerecha) Voltear();
            else if (inputHorizontal < 0 && mirandoDerecha) Voltear();
        }
    }

    void FixedUpdate()
    {
        // 4. Detectar si estamos tocando el suelo (fisicas)
        Suelo = Physics2D.OverlapCircle(controladorSuelo.position, radioDeteccion, EsSuelo);

        // 5. Aplicar velocidad horizontal manteniendo la velocidad vertical actual (gravedad)
        rb.velocity = new Vector2(inputHorizontal * velocidadMovimiento, rb.velocity.y);
    }

    void Saltar()
    {
        // Reseteamos la velocidad vertical para un salto consistente
        rb.velocity = new Vector2(rb.velocity.x, 0); 
        rb.AddForce(new Vector2(0, fuerzaSalto), ForceMode2D.Impulse);
    }

    void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    void Atacar()
    {
       
        Debug.Log("¡PUM! Ataque realizado"); 
        if(Suelo) rb.AddForce(new Vector2(mirandoDerecha ? 2 : -2, 0), ForceMode2D.Impulse); 
    }

    public void DesbloquearHabilidad(string nombreHabilidad)
    {
        switch (nombreHabilidad)
        {
            case "Ataque": puedeAtacar = true; break;
            case "DobleSalto": puedeDobleSalto = true; break;
            case "Correr": puedeCorrer = true; break;
        }
    }
}