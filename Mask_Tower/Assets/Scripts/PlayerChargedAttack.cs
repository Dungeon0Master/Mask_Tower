using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargedAttack : MonoBehaviour
{
    //Configuracion
    public GameObject prefabProyectil; 
    public Transform puntoDisparo;     // Un objeto vacío hijo desde donde sale el disparo
    public float tiempoParaCargar = 1.5f;
    
    private float tiempoPresionado = 0f;
    private bool estaCargado = false;

    // Visual simple para saber que está cargado (Greyboxing)
    private SpriteRenderer miSprite;

    void Awake()
    {
        miSprite = GetComponent<SpriteRenderer>();
    }

    public void GestionarCarga(bool botonPresionado)
    {
        if (botonPresionado)
        {
            // Estamos manteniendo el botón
            tiempoPresionado += Time.deltaTime;

            if (tiempoPresionado >= tiempoParaCargar && !estaCargado)
            {
                estaCargado = true;
                
                miSprite.color = Color.yellow; // Feedback visual temporal
            }
        }
        else
        {
            // Soltamos el botón
            if (estaCargado)
            {
                Disparar();
            }
            // Reseteamos todo
            tiempoPresionado = 0;
            estaCargado = false;
            miSprite.color = Color.white; // Volver a color normal
        }
    }

    void Disparar()
    {
        if (prefabProyectil != null && puntoDisparo != null)
        {
            Instantiate(prefabProyectil, puntoDisparo.position, puntoDisparo.rotation);
        }
    }
}