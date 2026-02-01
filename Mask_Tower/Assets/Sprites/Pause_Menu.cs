using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject pauseCanvas;
    public GameObject menuPrincipal;
    public GameObject controlesCanvas;

    private bool juegoPausado = false;
    private bool mostrandoControles = false;

    void Start()
    {
        pauseCanvas.SetActive(false);
        controlesCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mostrandoControles)
            {
                OcultarControles();
            }
            else
            {
                TogglePausa();
            }
        }
    }

    void TogglePausa()
    {
        if (juegoPausado)
            Reanudar();
        else
            Pausar();
    }

    void Pausar()
    {
        pauseCanvas.SetActive(true);
        menuPrincipal.SetActive(true);
        controlesCanvas.SetActive(false);

        Time.timeScale = 0f;
        juegoPausado = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Reanudar()
    {
        pauseCanvas.SetActive(false);

        Time.timeScale = 1f;
        juegoPausado = false;
        mostrandoControles = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MostrarControles()
    {
        menuPrincipal.SetActive(false);
        controlesCanvas.SetActive(true);
        mostrandoControles = true;
    }

    void OcultarControles()
    {
        controlesCanvas.SetActive(false);
        menuPrincipal.SetActive(true);
        mostrandoControles = false;
    }

    public void SalirDelJuego()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
