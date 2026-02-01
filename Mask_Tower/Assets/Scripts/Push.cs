using UnityEngine;
using UnityEngine.SceneManagement;

public class Push : MonoBehaviour
{
    void Update()
    {
        // Detecta cualquier tecla o clic del mouse
        if (Input.anyKeyDown)
        {
            int siguienteEscena = SceneManager.GetActiveScene().buildIndex + 1;

            // Verificamos que exista una escena después de esta
            if (siguienteEscena < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(siguienteEscena);
            }
        }
    }
}