using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MascaraFragmento : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Escribe EXACTAMENTE: Ataque, DobleSalto, Correr o Disparo")]
    public string habilidadAOtorgar; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Solo reaccionamos si nos toca el Jugador
        if (collision.CompareTag("Player"))
        {
            // 2. Buscamos el controlador del jugador
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                // 3. Desbloqueamos la habilidad configurada
                player.DesbloquearHabilidad(habilidadAOtorgar);
                
                Debug.Log("¡Fragmento recogido! Habilidad obtenida: " + habilidadAOtorgar);

                
                
                // 5. Destruimos este objeto del mundo
                Destroy(gameObject);
            }
        }
    }
}