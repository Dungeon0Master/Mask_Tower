using UnityEngine;
using System.Collections;

public class BossAbductor : MonoBehaviour
{
    [Header("Referencias")]
    public SistemaVida vida;
    public SpriteRenderer spriteRenderer;
    private Animator anim; // Referencia al Animator

    [Header("Posiciones Clave")]
    public Transform puntoCentroArriba;
    public Transform puntoDerecha;
    public Transform puntoIzquierda;

    [Header("Música")]
public MusicManager musicManager;

    [Header("Movimiento")]
    public float velocidadMovimiento = 4f;

    public Transform puntoDisparoCarga; // Desde donde sale el ataque frontal
    public GameObject prefabProyectilCarga; // El objeto que dispara

    [Header("Tiempos")]
    public float tiempoCarga = 3.5f;
    public float tiempoLluvia = 4f;
    public float tiempoIntro = 2f; // Tiempo para dejar que termine la animación de aparecer

    [Header("Vulnerabilidad")]
    public Collider2D hitboxVulnerable;

    [Header("Lluvia de Fuego")]
    public GameObject prefabFuego;
    public Transform puntoSpawnTecho;
    public float intervaloFuego = 0.4f;



    [Header("Muerte")]
    public GameObject fragmentoMascara;

    public GameObject[] objetosParaActivar;    
    public GameObject[] objetosParaDesactivar;

    private bool vulnerable = false;
    private bool animacionPausada = false; // Bandera interna de control

    void Awake()
    {
        if (vida == null) vida = GetComponent<SistemaVida>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Obtenemos el Animator automáticamente
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        transform.position = puntoCentroArriba.position;
        // En lugar de ir directo al bucle, hacemos la intro
        StartCoroutine(RutinaIntro());
    }

    IEnumerator RutinaIntro()
    {
        // 1. Disparar animación de aparecer
        if (anim != null) anim.SetTrigger("Aparecer");

        // 2. Esperar a que termine la animación o grito inicial
        yield return new WaitForSeconds(tiempoIntro);

        // 3. Ahora sí, ¡que empiece la pelea!
        StartCoroutine(BucleBoss());
    }

    IEnumerator BucleBoss()
    {
        while (vida != null)
        {
            // Derecha
            yield return MoverA(puntoDerecha.position);
            yield return FaseCarga();

            // Centro
            yield return MoverA(puntoCentroArriba.position);

            // Izquierda
            yield return MoverA(puntoIzquierda.position);
            yield return FaseCarga();

            // Centro
            yield return MoverA(puntoCentroArriba.position);

            // Lluvia
            yield return FaseLluviaFuego();
        }
    }

    IEnumerator MoverA(Vector2 destino)
    {
        if (anim != null) anim.SetTrigger("Mover");

        
        float direccionX = destino.x - transform.position.x;
        Vector3 escala = transform.localScale;

        if (direccionX < 0) 
        {
            // Vamos a la DERECHA -> Escala positiva
            escala.x = Mathf.Abs(escala.x); 
        }
        else if (direccionX  > 0)
        {
            // Vamos a la IZQUIERDA -> Escala negativa (invertida)
            escala.x = -Mathf.Abs(escala.x);
        }
        transform.localScale = escala;
    

        while (Vector2.Distance(transform.position, destino) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                destino,
                velocidadMovimiento * Time.deltaTime
            );
            yield return null;
        }
    }
    IEnumerator FaseCarga()
{
    // 1. Fase de Carga (Vulnerable)
    if (anim != null) anim.SetTrigger("Cargar");

    vulnerable = true;
    if (hitboxVulnerable != null) hitboxVulnerable.enabled = true;

    float t = 0f;
    bool parpadeo = false;

    // El jefe parpadea en rojo mientras carga
    while (t < tiempoCarga)
    {
        t += 0.2f;
        parpadeo = !parpadeo;
        spriteRenderer.color = parpadeo ? Color.red : Color.white;
        yield return new WaitForSeconds(0.2f);
    }

    // 2. Ejecución del Ataque (Rasguño)
    spriteRenderer.color = Color.white;
    vulnerable = false;
    if (hitboxVulnerable != null) hitboxVulnerable.enabled = false;

    if (anim != null) anim.SetTrigger("Atacar");

    yield return new WaitForSeconds(1.0f); 
}

    IEnumerator FaseLluviaFuego()
    {
        // Activamos animación de Ataque
        if (anim != null) anim.SetTrigger("Lluvia");

        float t = 0f;
        while (t < tiempoLluvia)
        {
            t += intervaloFuego;

            Vector2 pos = puntoSpawnTecho.position;
            pos.x += Random.Range(-6f, 6f);

            Instantiate(prefabFuego, pos, Quaternion.identity);

            yield return new WaitForSeconds(intervaloFuego);
        }
    }

    public bool EsVulnerable()
    {
        return vulnerable;
    }

    public void MorirBoss()
    {
        StopAllCoroutines();
        StartCoroutine(Muerte());
    }

    IEnumerator Muerte()
    {
        // Activamos animación de Muerte
        if (anim != null) anim.SetTrigger("Muerte");

        if(musicManager != null) musicManager.ActivarMusicaAmbiente();

        foreach (GameObject obj in objetosParaActivar)
        {
            if (obj != null) obj.SetActive(true);
        }

        foreach (GameObject obj in objetosParaDesactivar)
        {
            if (obj != null) obj.SetActive(false);
        }

        for (float a = 1; a > 0; a -= 0.05f)
        {
            spriteRenderer.color = new Color(1, 1, 1, a);
            yield return new WaitForSeconds(0.05f);
        }

        if (fragmentoMascara != null)
            Instantiate(fragmentoMascara, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}