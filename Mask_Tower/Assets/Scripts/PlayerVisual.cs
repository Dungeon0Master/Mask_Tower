using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
 
    public PlayerController playerController; // Para saber si está en el suelo
    public Rigidbody2D rb;                    // Para saber la velocidad
    public SpriteRenderer spriteRenderer;     // Para cambiar la imagen

   
    public Sprite spriteIdle;   
    public Sprite spriteMove;    
    public Sprite spriteJump;   

    void Update()
    {
        //  Si no está en el suelo, poner sprite de SALTO
        if (!playerController.enSuelo)
        {
            CambiarSprite(spriteJump);
        }
       
        // Mathf.Abs convierte el valor a positivo (para que funcione a izq y der)
       //  Si está en el suelo y se mueve, poner sprite de movimiento
        else if (Mathf.Abs(rb.velocity.x) > 0.1f) 
        {
            CambiarSprite(spriteMove);
        }
        //  Si no pasa nada de lo anterior, poner sprite de IDLE
        else
        {
            CambiarSprite(spriteIdle);
        }
    }

    void CambiarSprite(Sprite nuevoSprite)
    {
        // Solo cambiamos si el sprite es diferente al actual (ahorra rendimiento)
        if (spriteRenderer.sprite != nuevoSprite)
        {
            spriteRenderer.sprite = nuevoSprite;
        }
    }
}
