using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaVida : MonoBehaviour
{
    //Configuración
    public int vidaMaxima = 3;
    [SerializeField] private int vidaActual;

    void Start()
    {
        // Al empezar, llenamos la vida al máximo
        vidaActual = vidaMaxima;
    }

    // Esta función es la que llamarán los ataques
    public void RecibirDaño(int cantidad)
    {
        vidaActual -= cantidad;
        Debug.Log(gameObject.name + " recibió " + cantidad + " de daño. Vida restante: " + vidaActual);

        if (vidaActual <= 0)
        {
            Morir();
        }
    }

    void Morir()
    {
        Debug.Log(gameObject.name + " ha muerto.");

        // Si es el jugador, reiniciamos nivel o mostramos Game Over
        if (gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false); 
        }
        else
        {
            //si es otra cosa como un enemigo lo destrulle
            Destroy(gameObject);
        }
    }
}