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

    // Referencias opcionales (pueden ser nulas en algunos enemigos)
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D miCollider;

    void Start()
    {
        vidaActual = vidaMaxima;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        miCollider = GetComponent<Collider2D>();
    }

    public void RecibirDaño(int cantidad, Vector2 posicionAtacante)
    {
        if (vidaActual <= 0) return; // Si ya está muerto, ignorar golpes extra

        vidaActual -= cantidad;

        // 1. Animación de Daño (Solo si tiene Animator)
        if (anim != null) anim.SetTrigger("Daño");

        // 2. Retroceso (Solo si tiene Rigidbody y está activado)
        if (aplicaRetroceso && rb != null)
        {
            Vector2 direccionEmpuje = (transform.position - (Vector3)posicionAtacante).normalized;
            rb.velocity = Vector2.zero; 
            rb.AddForce(direccionEmpuje * fuerzaRetroceso + Vector2.up * 2f, ForceMode2D.Impulse);
        }

        Debug.Log(gameObject.name + " recibió daño. Vida: " + vidaActual);

        if (vidaActual <= 0)
        {
            StartCoroutine(MorirConEstilo());
        }
    }

    private IEnumerator MorirConEstilo()
    {
        Debug.Log(gameObject.name + " ha muerto.");

        // 1. Animación de Muerte
        if (anim != null) anim.SetTrigger("Muerte");

        // 2. Desactivar físicas y colisiones (Para que el cadáver no estorbe)
       
        if (rb != null) rb.velocity = Vector2.zero; // Frenar en seco
        if (rb != null) rb.simulated = false;       // Ya no le afecta la gravedad ni choques
        if (miCollider != null) miCollider.enabled = false; // Ya no se puede tocar

        // 3. Desactivar Control (Lógica segura)
      
        PlayerController playerCtrl = GetComponent<PlayerController>();
        if (playerCtrl != null)
        {
            playerCtrl.enabled = false;
        }
        // 4. Esperar a que termine la animación
        yield return new WaitForSeconds(1f); // Ajusta este tiempo a lo que dure tu animación

        // 5. Decisión final: Reiniciar o Destruir
        if (gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Morir()
    {
        if (CompareTag("Boss"))
        {
            GetComponent<BossAbductor>().MorirBoss();
            return;
        }

        Destroy(gameObject);
    }

}