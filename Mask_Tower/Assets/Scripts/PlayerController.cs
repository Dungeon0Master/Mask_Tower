using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Módulos (Referencias)")]
    public PlayerAttack playerAttack;
    public DoubleJump doubleJump;
    public PlayerChargedAttack ataqueCargado; 

    [Header("Configuración Movimiento")]
    [SerializeField] private float velocidadCaminar = 8f;
    [SerializeField] private float velocidadCorrer = 14f; 
    [SerializeField] private float fuerzaSalto = 10f;

    [Header("Detección de Suelo")]
    [SerializeField] private Transform controladorSuelo; 
    [SerializeField] private float radioDeteccion = 0.2f;
    [SerializeField] private LayerMask layerSuelo; 

    [Header("Habilidades Desbloqueables")]
    public bool puedeAtacar = false;      // Nivel 2
    public bool puedeDobleSalto = false;  // Nivel 3
    public bool puedeCorrer = false;      // Nivel 4
    public bool puedeDisparar = false;

    // Variables internas
    private Rigidbody2D rb;
    private float inputHorizontal;
    public bool enSuelo; 
    private bool mirandoDerecha = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Autocompletado de seguridad
        if (ataqueCargado == null) ataqueCargado = GetComponent<PlayerChargedAttack>();
        if (playerAttack == null) playerAttack = GetComponent<PlayerAttack>();
        if (doubleJump == null) doubleJump = GetComponent<DoubleJump>(); 
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            if (enSuelo)
            {
                Saltar(fuerzaSalto); // Salto Normal
            }
            else if (puedeDobleSalto && doubleJump != null)
            {
                doubleJump.IntentarDobleSalto(); // Doble Salto Modular
            }
        }

       
        if (Input.GetButtonDown("Fire1") && puedeAtacar && !playerAttack.estaAtacando)
        {
            playerAttack.RealizarAtaque();
          
        }

        // --- 3. ATAQUE CARGADO ---
        if (puedeDisparar && ataqueCargado != null)
        {
            ataqueCargado.GestionarCarga(Input.GetButton("Fire1"));
        }

        
        if (!playerAttack.estaAtacando)
        {
            if (inputHorizontal > 0 && !mirandoDerecha) Voltear();
            else if (inputHorizontal < 0 && mirandoDerecha) Voltear();
        }
    }

    void FixedUpdate()
    {
        // Detección de suelo robusta
        enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioDeteccion, layerSuelo);

        // Recarga de Doble Salto (Optimizado: si toca suelo, recarga siempre)
        if (enSuelo && doubleJump != null) 
        {
            doubleJump.RecargarSalto();
        }

        // FÍSICAS DE MOVIMIENTO
        
        // Prioridad 1: Si ataca en el suelo, se queda quieto
        if (playerAttack.estaAtacando && enSuelo)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            // Prioridad 2: Movimiento Normal vs Correr
            float velocidadActual = velocidadCaminar;

            
            if (puedeCorrer && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
            {
                velocidadActual = velocidadCorrer;
            }

            rb.velocity = new Vector2(inputHorizontal * velocidadActual, rb.velocity.y);
        }
    }

    void Saltar(float fuerza)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); 
        rb.AddForce(new Vector2(0, fuerza), ForceMode2D.Impulse);
    }

    void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
       
        transform.Rotate(0f, 180f, 0f);
    }

    public void DesbloquearHabilidad(string nombreHabilidad)
    {
        switch (nombreHabilidad)
        {
            case "Ataque": puedeAtacar = true; break;
            case "DobleSalto": puedeDobleSalto = true; break;
            case "Correr": puedeCorrer = true; break;
            case "Disparo": puedeDisparar = true; break; // Faltaba este caso
        }
    }
}