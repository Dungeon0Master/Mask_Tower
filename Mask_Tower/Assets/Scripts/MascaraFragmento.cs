using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MascaraFragmento : MonoBehaviour
{
    [Header("Configuración")]
    public string habilidadAOtorgar; 
    public GameObject mascaraVisualEnJugador; 

    public Transform puertaSalida;
    public float velocidadCaminataAuto = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            Animator anim = collision.GetComponent<Animator>();
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            if (player != null)
            {
                StartCoroutine(SecuenciaFinalNivel(player, anim, rb));
            }
        }
    }

    private IEnumerator SecuenciaFinalNivel(PlayerController player, Animator anim,Rigidbody2D rb)
    {
      

        rb.gravityScale = 10f;

        yield return new WaitUntil(() => rb.velocity.y == 0);

         player.enabled = false;
        rb.velocity = Vector2.zero;

        rb.gravityScale = 1f; // Restauramos gravedad para después

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
         if (sr != null) sr.enabled = false;

         GetComponent<Collider2D>().enabled = false;

        //  Activar la máscara en el rostro y dar la habilidad 
        if (mascaraVisualEnJugador != null) mascaraVisualEnJugador.SetActive(true);
        player.DesbloquearHabilidad(habilidadAOtorgar);


        //  Ejecutar animación de recoger 
        if (anim != null) anim.SetTrigger("Recoger");
        yield return new WaitForSeconds(1.5f);
    
        

        if (puertaSalida != null)
        {
            
            // Mientras no hayamos llegado a la puerta...
            while (Vector2.Distance(rb.transform.position, puertaSalida.position) > 0.2f)
            {
                // Calculamos dirección hacia la puerta
                float direccion = (puertaSalida.position.x - rb.transform.position.x) > 0 ? 1 : -1;
                
                // Aplicamos velocidad constante
                rb.velocity = new Vector2(direccion * velocidadCaminataAuto, rb.velocity.y);
                
               yield return null; // Esperar al siguiente frame
            }

            // Al llegar, nos detenemos
            rb.velocity = Vector2.zero;
            
        }
    }
}