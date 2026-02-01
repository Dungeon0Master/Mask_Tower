using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilSimple : MonoBehaviour
{
    [Header("Configuración")]
    public int daño = 1;
    public string tagObjetivo = "Player"; 
    public float tiempoDeVida = 5f; // Seguridad para que no caigan para siempre

    void Start()
    {
        // Autodestrucción después de X segundos si no toca nada
        Destroy(gameObject, tiempoDeVida);
    }

    // Usamos OnTriggerEnter2D porque queremos que atraviese al jugador, no que lo empuje
    void OnTriggerEnter2D(Collider2D otro)
    {
        // 1. ¿Tocamos al objetivo?
        if (otro.CompareTag(tagObjetivo))
        {
            SistemaVida vidaObjetivo = otro.GetComponent<SistemaVida>();
            if (vidaObjetivo != null)
            {
                // Le pasamos la posición del proyectil para calcular el retroceso
                vidaObjetivo.RecibirDaño(daño, transform.position);
            }
            // Destruimos el proyectil al impactar
            Destroy(gameObject);
        }
        // ¿Tocamos el suelo? Destruirse también.
        else if (otro.CompareTag("Suelo")) // Asegúrate de que tu suelo tenga este Tag
        {
            // Aquí podrías instanciar un efecto de explosión pequeño
            Destroy(gameObject);
        }
    }
}