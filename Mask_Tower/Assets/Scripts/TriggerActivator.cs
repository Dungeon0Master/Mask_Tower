using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    [Header("Configuración de Objetos")]
    [Tooltip("Objetos que se activarán (ej. el enemigo)")]
    public List<GameObject> objetosAActivar;

    [Tooltip("Objetos que se desactivarán (ej. la plataforma)")]
    public List<GameObject> objetosADesactivar;

    [Header("Ajustes del Trigger")]
   
    private bool yaSeUso = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("aqui se toco");
        // Verificamos que sea el jugador y que el trigger no se haya usado ya
        if (!yaSeUso && other.CompareTag("Player"))
        {
            EjecutarAccion();
        }
    }

    private void EjecutarAccion()
    {
        yaSeUso = true;

        // Activar elementos de la lista
        foreach (GameObject obj in objetosAActivar)
        {
            if (obj != null) obj.SetActive(true);
        }

        // Desactivar elementos de la lista
        foreach (GameObject obj in objetosADesactivar)
        {
            if (obj != null) obj.SetActive(false);
        }

        // Desactivamos este objeto (la placa invisible) para que no se repita
        Debug.Log("Trigger activado y desactivado permanentemente.");
        this.gameObject.SetActive(false);
    }
}