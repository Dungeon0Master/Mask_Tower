using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ControladorFinal : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        // Nos suscribimos al evento de finalización
        videoPlayer.loopPointReached += CerrarJuego;
    }

    void CerrarJuego(VideoPlayer vp)
    {
        Debug.Log("Saliendo del juego...");
        
        // Esto cierra el archivo .exe o la app compilada
        Application.Quit();

       
    }
}