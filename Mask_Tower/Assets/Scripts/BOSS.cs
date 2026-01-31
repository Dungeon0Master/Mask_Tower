using UnityEngine;
using System.Collections;

public class BossAbductor : MonoBehaviour
{
    public enum EstadoBoss
    {
        Centro,
        CargandoDerecha,
        CargandoIzquierda,
        LluviaFuego
    }

    [Header("Referencias")]
    public SistemaVida vida;

    [Header("Posiciones Clave")]
    public Transform puntoCentroArriba;
    public Transform puntoDerecha;
    public Transform puntoIzquierda;

    [Header("Movimiento")]
    public float velocidadMovimiento = 4f;

    [Header("Tiempos")]
    public float tiempoCarga = 3.5f;
    public float tiempoLluvia = 4f;

    [Header("Lluvia de Fuego")]
    public GameObject prefabFuego;
    public Transform puntoSpawnTecho1;
    public float intervaloFuego = 0.4f;

    [Header("Vulnerabilidad")]
    public Collider2D hitboxVulnerable;
    private bool vulnerable;

    [Header("Muerte")]
    public GameObject fragmentoMascara;

    private EstadoBoss estadoActual = EstadoBoss.Centro;
    private SpriteRenderer sr;
    private bool enAccion = false;
    public bool EsVulnerable() => vulnerable;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        transform.position = puntoCentroArriba.position;
        StartCoroutine(BucleBoss());
    }

    IEnumerator BucleBoss()
    {
        while (vida != null)
        {
            yield return MoverA(puntoDerecha.position);
            yield return FaseCarga();

            yield return MoverA(puntoCentroArriba.position);

            yield return MoverA(puntoIzquierda.position);
            yield return FaseCarga();

            yield return MoverA(puntoCentroArriba.position);

            yield return FaseLluviaFuego();
        }
    }

    IEnumerator MoverA(Vector2 destino)
    {
        while (Vector2.Distance(transform.position, destino) > 0.1f)
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
        float tiempo = 0f;
        bool parpadeo = false;

        while (tiempo < tiempoCarga)
        {
            tiempo += Time.deltaTime;

            // Parpadeo visual
            parpadeo = !parpadeo;
            sr.color = parpadeo ? Color.white : Color.red;
            hitboxVulnerable.enabled = true;
            vulnerable = true;

            yield return new WaitForSeconds(0.2f);
            hitboxVulnerable.enabled = false;
            vulnerable = false;
        }

        sr.color = Color.white;
    }

    IEnumerator FaseLluviaFuego()
    {
        float tiempo = 0f;

        while (tiempo < tiempoLluvia)
        {
            tiempo += intervaloFuego;

            Vector2 pos = puntoSpawnTecho1.position;
            pos.x += Random.Range(-6f, 6f);

            Instantiate(prefabFuego, pos, Quaternion.identity);

            yield return new WaitForSeconds(intervaloFuego);
        }
    }

    public void MorirBoss()
    {
        StopAllCoroutines();
        StartCoroutine(Muerte());
    }

    IEnumerator Muerte()
    {
        // Disolver fake
        for (float a = 1; a > 0; a -= 0.05f)
        {
            sr.color = new Color(1, 1, 1, a);
            yield return new WaitForSeconds(0.05f);
        }

        Instantiate(fragmentoMascara, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
