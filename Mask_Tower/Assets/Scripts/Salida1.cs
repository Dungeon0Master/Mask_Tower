using UnityEngine;
using UnityEngine.SceneManagement;

public class PuertaSalidaOutro : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [Tooltip("El nombre exacto de la escena de salida")]
    public string nombreEscenaOutro = "Outro 1";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CargarOutro();
        }
    }

    public void CargarOutro()
    {
        // Verificamos si la escena existe en los Build Settings antes de cargar
        if (Application.CanStreamedLevelBeLoaded(nombreEscenaOutro))
        {
            SceneManager.LoadScene(nombreEscenaOutro);
        }
        else
        {
            Debug.LogError("Error: No se pudo encontrar la escena '" + nombreEscenaOutro + "'. Asegúrate de que esté añadida en Build Settings.");
        }
    }
}