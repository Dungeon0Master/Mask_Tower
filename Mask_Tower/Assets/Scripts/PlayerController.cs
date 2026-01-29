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
    bool mirandoDerecha = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
            if (enSuelo) Saltar(fuerzaSalto);
            else if (puedeDobleSalto && doubleJump != null)
                doubleJump.IntentarDobleSalto();
        }

        bool firePressed = Input.GetButton("Fire1");
        bool fireDown = Input.GetButtonDown("Fire1");
        bool fireUp = Input.GetButtonUp("Fire1");

        // ATAQUE CARGADO (prioridad)
        if (puedeDisparar && ataqueCargado != null)
        {
            ataqueCargado.GestionarCarga(firePressed);
        }

        // ATAQUE BÁSICO (solo si NO se cargó)
        if (fireDown && puedeAtacar && playerAttack != null && !playerAttack.estaAtacando)
        {
            playerAttack.RealizarAtaque();
        }

        // VOLTEO
        if (playerAttack == null || !playerAttack.estaAtacando)
        {
            if (inputHorizontal > 0 && !mirandoDerecha) Voltear();
            else if (inputHorizontal < 0 && mirandoDerecha) Voltear();
        }
    }

    void FixedUpdate()
    {
        enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioDeteccion, layerSuelo);

        if (enSuelo && doubleJump != null)
            doubleJump.RecargarSalto();

        bool atacando = playerAttack != null && playerAttack.estaAtacando;

        if (atacando && enSuelo)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            float velocidad = velocidadCaminar;
            if (puedeCorrer && Input.GetKey(KeyCode.LeftShift)) velocidad = velocidadCorrer;
            rb.velocity = new Vector2(inputHorizontal * velocidad, rb.velocity.y);
        }
    }

    void Saltar(float fuerza)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * fuerza, ForceMode2D.Impulse);
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
