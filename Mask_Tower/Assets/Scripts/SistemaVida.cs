using UnityEngine;
using UnityEngine.SceneManagement;

public class SistemaVida : MonoBehaviour
{
    [Header("Configuración")]
    public int vidaMaxima = 3;
    [SerializeField] private int vidaActual;
    
    [Header("Configuración de Retroceso")]
    public bool aplicaRetroceso = true; 
    [SerializeField] private float fuerzaRetroceso = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        vidaActual = vidaMaxima;
        rb = GetComponent<Rigidbody2D>();
    }

    public void RecibirDaño(int cantidad, Vector2 posicionAtacante)
    {
        vidaActual -= cantidad;
        
        // Solo aplicamos empuje si la casilla está marcada Y tenemos Rigidbody
        if (aplicaRetroceso && rb != null)
        {
            // Calculamos dirección
            Vector2 direccionEmpuje = (transform.position - (Vector3)posicionAtacante).normalized;
            
            // Reseteamos velocidad para golpe seco
            rb.velocity = Vector2.zero;
            
            // Empujón
            rb.AddForce(direccionEmpuje * fuerzaRetroceso + Vector2.up * 2f, ForceMode2D.Impulse);
        }

        Debug.Log(gameObject.name + " recibió daño. Vida: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
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