using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class AutoCargaVideo : MonoBehaviour
{
    private VideoPlayer vp;

    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        // Suscribirse al evento de cuando el video termina
        vp.loopPointReached += AlTerminarVideo;
    }

    void AlTerminarVideo(VideoPlayer source)
    {
        int escenaActual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(escenaActual + 1);
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}