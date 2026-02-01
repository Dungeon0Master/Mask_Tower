using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SistemaVida : MonoBehaviour
{
    [Header("Configuración")]
    public int vidaMaxima = 3;
    [SerializeField] private int vidaActual;

    [Header("Feedback")]
    public bool aplicaRetroceso = true;
    [SerializeField] private float fuerzaRetroceso = 5f;

    // Referencias opcionales
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D miCollider;

    [SerializeField] private BossAbductor scriptBoss;

    void Start()
    {
        vidaActual = vidaMaxima;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        miCollider = GetComponent<Collider2D>();
        
        // Intentamos obtener el script del Boss automáticamente
        scriptBoss = GetComponent<BossAbductor>();
    }

    public void RecibirDaño(int cantidad, Vector2 posicionAtacante)
    {
        if (vidaActual <= 0) return; // Si ya está muerto, ignorar

        
        // Si este objeto es un Boss Y nos dice que NO es vulnerable...
        if (scriptBoss != null && !scriptBoss.EsVulnerable())
        {
            Debug.Log("¡Ataque bloqueado! El Boss es invulnerable.");
            return; // ¡Salimos de la función sin restar vida!
        }
       

        vidaActual -= cantidad;

        // Animación de Daño
        if (anim != null) anim.SetTrigger("Daño");

        // Retroceso
        if (aplicaRetroceso && rb != null)
        {
            Vector2 direccionEmpuje = (transform.position - (Vector3)posicionAtacante).normalized;
            // Parche de seguridad por si están en la misma posición exacta
            if (direccionEmpuje == Vector2.zero) direccionEmpuje = Vector2.up; 
            
            rb.velocity = Vector2.zero; 
            rb.AddForce(direccionEmpuje * fuerzaRetroceso + Vector2.up * 2f, ForceMode2D.Impulse);
        }

        Debug.Log(gameObject.name + " recibió daño. Vida: " + vidaActual);

        if (vidaActual <= 0)
        {
            // MUERTE DIFERENCIADA
            if (scriptBoss != null)
            {
                // Si es el Boss, dejamos que SU script maneje la muerte (cinemática, loot, etc.)
                scriptBoss.MorirBoss();
            }
            else
            {
                // Si es el Player o un enemigo normal, usamos la muerte estándar
                StartCoroutine(MorirConEstilo());
            }
        }
    }

    private IEnumerator MorirConEstilo()
    {
        Debug.Log(gameObject.name + " ha muerto.");

        if (anim != null) anim.SetTrigger("Muerte");

        // Desactivar físicas
        if (rb != null) 
        {
            rb.velocity = Vector2.zero;
            rb.simulated = false; 
        }
        if (miCollider != null) miCollider.enabled = false;

        // Desactivar Control si es el Player
        PlayerController playerCtrl = GetComponent<PlayerController>();
        if (playerCtrl != null) playerCtrl.enabled = false;

        yield return new WaitForSeconds(1f);

        if (gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}