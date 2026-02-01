using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaSalida : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CargarSiguienteEscena();
        }
    }

    public void CargarSiguienteEscena()
    {
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        int proximaEscena = escenaActual + 1;

        // Si es el último nivel, el siguiente índice debería ser tu cinemática de salida
        if (proximaEscena < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(proximaEscena);
        }
        else
        {
            Debug.Log("¡Juego Terminado!");
            // Aquí podrías volver al menú principal
        }
    }
}