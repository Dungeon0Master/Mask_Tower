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

    [Header("Suelo")]
    [SerializeField] Transform controladorSuelo;
    [SerializeField] float radioDeteccion = 0.2f;
    [SerializeField] LayerMask layerSuelo;

    [Header("Habilidades")]
    public bool puedeAtacar;
    public bool puedeDobleSalto;
    public bool puedeCorrer;
    public bool puedeDisparar;

    Rigidbody2D rb;

    float inputHorizontal;
    public bool enSuelo;
    Animator anim;
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

        // SALTO
        if (Input.GetButtonDown("Jump"))
        {
            if (enSuelo && !estaSaltando)
            {
                // 1. SALTO EN TIERRA (Con carga)
                // En lugar de saltar ya, iniciamos la animación y bloqueamos
                estaSaltando = true; 

                if(anim != null) anim.SetTrigger("IniciarSalto"); 
            }
            else if (puedeDobleSalto && doubleJump != null && !enSuelo)
            {
                // Aquí activamos la animación nueva específica
                if(anim != null) anim.SetTrigger("DobleSalto");
                
                doubleJump.IntentarDobleSalto();
            }
        }

        bool ataqueNormalDown = Input.GetButtonDown("Fire1"); // Click izquierdo
        bool ataqueCargadoPresionado = Input.GetButton("Fire2"); // Click derecho


        // ATAQUE CARGADO (prioridad)
        if (puedeDisparar && ataqueCargado != null)
        {
            ataqueCargado.GestionarCarga(ataqueCargadoPresionado);
        }

        // ATAQUE BÁSICO (solo si NO se cargó)
        if (ataqueNormalDown && puedeAtacar && playerAttack != null && !playerAttack.estaAtacando)
        {
            playerAttack.RealizarAtaque();
        }


        // VOLTEO
        if (playerAttack == null || !playerAttack.estaAtacando)
        {
            // Bloqueamos volteo si estamos cargando el salto para que no se vea raro
            if (!estaSaltando) 
            {
                if (inputHorizontal > 0 && !mirandoDerecha) Voltear();
                else if (inputHorizontal < 0 && mirandoDerecha) Voltear();
            }
        }
        estaCargandoAtaque = ataqueCargadoPresionado;

    }

    void FixedUpdate()
{
    enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioDeteccion, layerSuelo);

    if (enSuelo && doubleJump != null)
        doubleJump.RecargarSalto();

    // 1. Si está cargando ataque O preparando el salto, se queda quieto en X
    if (estaCargandoAtaque || estaSaltando) 
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
    else
    {
        // 2. Movimiento normal solo si NO está saltando/cargando
        float velocidad = velocidadCaminar;

        if (puedeCorrer && Input.GetKey(KeyCode.LeftShift))
            velocidad = velocidadCorrer;

        rb.velocity = new Vector2(inputHorizontal * velocidad, rb.velocity.y);
    }
}
   
   public void EventoImpulsoSalto()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        
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
            case "Ataque":
                puedeAtacar = true;
                break;

            case "DobleSalto":
                puedeDobleSalto = true;
                break;

            case "Correr":
                puedeCorrer = true;
                break;

            case "Disparo":
                puedeDisparar = true;
                break;

            default:
                Debug.LogWarning("Habilidad desconocida: " + nombreHabilidad);
                break;
        }
    }



}
