using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    public float fuerzaDobleSalto = 10f; // Menor fuerza que el salto normal

    // Estado interno
    private bool yaSaltoEnElAire = false; 
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Esta función se llama cuando tocamos el suelo (desde el Controller)
    public void RecargarSalto()
    {
        yaSaltoEnElAire = false;
    }

    // Intenta hacer el doble salto. Devuelve true si lo logró, false si no pudo.
    public bool IntentarDobleSalto()
    {
        // Si ya gastamos el salto, no hacemos nada
        if (yaSaltoEnElAire) return false;

        // 1. Reseteamos la velocidad vertical actual
        rb.velocity = new Vector2(rb.velocity.x, 0);

        // 2. Aplicamos la fuerza 
        rb.AddForce(Vector2.up * fuerzaDobleSalto, ForceMode2D.Impulse);

        // 3. Marcamos que ya se usó
        yaSaltoEnElAire = true;
        
        Debug.Log("¡Doble Salto ejecutado!");
        return true;
    }
}