
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Módulos")]
    public PlayerAttack playerAttack;
    public DoubleJump doubleJump;
    public PlayerChargedAttack ataqueCargado;

    [Header("Movimiento")]
    [SerializeField] float velocidadCaminar = 8f;
    [SerializeField] float velocidadCorrer = 14f;
    [SerializeField] float fuerzaSalto = 10f;

    [Header("Suelo (OverlapCircle)")]
    [SerializeField] Transform controladorSuelo; // Asigna aquí el objeto vacío en los pies
    [SerializeField] float radioDeteccion = 0.2f; // Tamaño de la bolita
    [SerializeField] LayerMask layerSuelo;       // Selecciona la capa "Ground"

    [Header("Habilidades")]
    public bool puedeAtacar;
    public bool puedeDobleSalto;
    public bool puedeCorrer;
    public bool puedeDisparar;

    Rigidbody2D rb;
    Animator anim;

    float inputHorizontal;
    public bool enSuelo;
    bool mirandoDerecha = true;

    private bool estaCargandoAtaque = false;
    private bool estaSaltando = false; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerAttack ??= GetComponent<PlayerAttack>();
        doubleJump ??= GetComponent<DoubleJump>();
        ataqueCargado ??= GetComponent<PlayerChargedAttack>();
    }

    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        // --- SALTO ---
        if (Input.GetButtonDown("Jump"))
        {
            if (enSuelo && !estaSaltando)
            {
                estaSaltando = true;
                Debug.Log("Salto iniciado");
                if (anim != null) anim.SetTrigger("IniciarSalto");
                
                // OPCIONAL: Si la animación falla mucho, descomenta esto para saltar instantáneo:
                // EventoImpulsoSalto(); 
            }
            else if (puedeDobleSalto && doubleJump != null && !enSuelo)
            {
                if(doubleJump.IntentarDobleSalto())
                {
                    if (anim != null) anim.SetTrigger("DobleSalto");
                }
            }
        }

        // --- ATAQUES ---
        bool ataqueNormalDown = Input.GetButtonDown("Fire1"); 
        bool ataqueCargadoPresionado = Input.GetButton("Fire2"); 

        if (puedeDisparar && ataqueCargado != null)
            ataqueCargado.GestionarCarga(ataqueCargadoPresionado);

        if (ataqueNormalDown && puedeAtacar && playerAttack != null && !playerAttack.estaAtacando)
            playerAttack.RealizarAtaque();

        // --- VOLTEO ---
        if (playerAttack == null || !playerAttack.estaAtacando)
        {
            if (!estaSaltando) 
            {
                if (inputHorizontal > 0 && !mirandoDerecha) Voltear();
                else if (inputHorizontal < 0 && mirandoDerecha) Voltear();
            }
        }
        estaCargandoAtaque = ataqueCargadoPresionado;

        // ANIMACIONES
        if(anim != null)
        {
            anim.SetFloat("VelocidadX", Mathf.Abs(rb.velocity.x));
            anim.SetBool("EnSuelo", enSuelo);
        }
    }

    void FixedUpdate()
    {
        // 1. DETECCIÓN DE SUELO (Restaurada)
        // Usamos la posición del objeto vacío 'controladorSuelo'
        enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioDeteccion, layerSuelo);

        // 2. CORRECCIÓN DEL BUG DE "CONGELADO"
        // Si detectamos suelo y estamos cayendo (o quietos en Y), 
        // forzamos que 'estaSaltando' sea falso.
        // Esto arregla el problema si la animación de salto nunca llamó al evento.
        if (enSuelo && rb.velocity.y <= 0.1f)
        {
            estaSaltando = false;
            if (doubleJump != null) doubleJump.RecargarSalto();
        }

        // 3. MOVIMIENTO
        if (estaCargandoAtaque || estaSaltando) 
        {
            // Se queda quieto en X mientras carga o prepara el impulso
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            float velocidad = velocidadCaminar;
            if (puedeCorrer && Input.GetKey(KeyCode.LeftShift))
                velocidad = velocidadCorrer;

            rb.velocity = new Vector2(inputHorizontal * velocidad, rb.velocity.y);
        }
    }
   
    // Este evento debe ser llamado desde la ANIMACIÓN de salto (al final de la preparación)
    public void EventoImpulsoSalto()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        
        // ¡OJO! Aquí ya NO ponemos 'estaSaltando = false'.
        // Mantenemos 'estaSaltando = true' un momento más para que no patine en el aire.
        // La variable se desactivará sola en FixedUpdate cuando toques el suelo,
        // o puedes hacer una corrutina pequeña si quieres moverte en el aire inmediatamente.
        
        // Si quieres moverte en el aire INMEDIATAMENTE tras el impulso, descomenta esto:
        estaSaltando = false; 
    }

    void Voltear()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    public void DesbloquearHabilidad(string nombreHabilidad)
    {
        switch (nombreHabilidad)
        {
            case "Ataque": puedeAtacar = true; break;
            case "DobleSalto": puedeDobleSalto = true; break;
            case "Correr": puedeCorrer = true; break;
            case "Disparo": puedeDisparar = true; break;
            default: Debug.LogWarning("Habilidad desconocida: " + nombreHabilidad); break;
        }
    }

    // DIBUJAR GIZMOS (Para ver la bolita roja en el editor)
    private void OnDrawGizmos()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorSuelo.position, radioDeteccion);
        }
    }
}