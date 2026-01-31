using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaSalida : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el jugador toca la luz de la puerta tras la caminata...
        if (other.CompareTag("Player"))
        {
            
            
            
            Time.timeScale = 0;
        }
    }
}