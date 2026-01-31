using UnityEngine;
using System.Collections;

public class BossAbductor : MonoBehaviour
{
    [Header("Referencias")]
    public SistemaVida vida;
    public SpriteRenderer spriteRenderer;

    [Header("Posiciones Clave")]
    public Transform puntoCentroArriba;
    public Transform puntoDerecha;
    public Transform puntoIzquierda;

    [Header("Movimiento")]
    public float velocidadMovimiento = 4f;

    [Header("Tiempos")]
    public float tiempoCarga = 3.5f;
    public float tiempoLluvia = 4f;

    [Header("Vulnerabilidad")]
    public Collider2D hitboxVulnerable;

    [Header("Lluvia de Fuego")]
    public GameObject prefabFuego;
    public Transform puntoSpawnTecho;
    public float intervaloFuego = 0.4f;

    [Header("Muerte")]
    public GameObject fragmentoMascara;

    private bool vulnerable = false;

    void Awake()
    {
        if (vida == null)
            vida = GetComponent<SistemaVida>();

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
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
        vulnerable = true;
        if (hitboxVulnerable != null)
            hitboxVulnerable.enabled = true;

        float t = 0f;
        bool parpadeo = false;

        while (t < tiempoCarga)
        {
            t += 0.2f;
            parpadeo = !parpadeo;
            spriteRenderer.color = parpadeo ? Color.red : Color.white;
            yield return new WaitForSeconds(0.2f);
        }

        spriteRenderer.color = Color.white;

        vulnerable = false;
        if (hitboxVulnerable != null)
            hitboxVulnerable.enabled = false;
    }

    IEnumerator FaseLluviaFuego()
    {
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
