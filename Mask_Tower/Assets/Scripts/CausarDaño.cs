using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CausarDaño : MonoBehaviour
{
    //Configuración de Daño
    public int cantidadDaño = 1;   // Editable en el inspector para cada objeto
   
    public string tagObjetivo;     // ¿A quién daña esto? ("Enemigo" o "Player")

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Verificamos si tocamos al objetivo correcto por su Tag
        if (collision.CompareTag(tagObjetivo))
        {
            // 2. Buscamos si ese objeto tiene el script de vida
            SistemaVida vida = collision.GetComponent<SistemaVida>();

            // 3. Si tiene vida, le hacemos daño
            if (vida != null)
            {
                vida.RecibirDaño(cantidadDaño);
                
                }
            }
        }
    }
