using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    //Configuración de Ataque
    public GameObject visualAtaque;      // El objeto hijo 
    public float duracionAtaque = 0.15f; // Cuánto dura el hitbox activo
    public float cooldownAtaque = 0.3f;  // Tiempo de espera entre ataques

    // Estado público para que otros scripts sepan si estamos atacando
    public bool estaAtacando { get; private set; } 

    public void RealizarAtaque()
    {
        if (!estaAtacando)
        {
            StartCoroutine(RutinaAtaque());
        }
    }

    private IEnumerator RutinaAtaque()
    {
        estaAtacando = true;

        // Activar Visual / Hitbox
        if (visualAtaque != null) visualAtaque.SetActive(true);

        //  Esperar duración del golpe
        yield return new WaitForSeconds(duracionAtaque);

        //  Desactivar Visual / Hitbox
        if (visualAtaque != null) visualAtaque.SetActive(false);

        // Esperar Cooldown (recuperación)
        yield return new WaitForSeconds(cooldownAtaque);

        estaAtacando = false;
    }
}