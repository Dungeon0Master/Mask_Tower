using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    [Header("Configuración")]
    public float fuerzaDobleSalto = 8f; // A veces es mejor que sea un poco menor al salto normal

  
    
    // Estado interno
    private bool yaUsoDobleSalto = false; 
    private Rigidbody2D rb;
    private AudioSource audioSource;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>(); // Necesario si vas a usar sonidos
    }

    // Se llama desde el PlayerController cuando toca el suelo (OnCollisionEnter2D)
    public void RecargarSalto()
    {
        yaUsoDobleSalto = false;
    }

    // Intenta hacer el doble salto. Devuelve true si lo logró.
    public bool IntentarDobleSalto()
    {
        // 1. Si ya gastamos el salto, cancelamos
        if (yaUsoDobleSalto) return false;

        rb.velocity = new Vector2(rb.velocity.x, 0);

        // 3. Aplicamos la fuerza de impulso
        rb.AddForce(Vector2.up * fuerzaDobleSalto, ForceMode2D.Impulse);

        // 4. Marcamos que ya se usó para que no salte infinito
        yaUsoDobleSalto = true;
        
        // 5. Ejecutar efectos (si existen)
    
        Debug.Log("¡Doble Salto ejecutado!");
        return true;
    }

}