using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    private AudioSource musicaAmbiente; // Se buscará por código
    public AudioSource musicaBoss;      // Este sí está en la escena (el del jefe)
    public float velocidadTransicion = 0.5f;

    void Start()
    {
        // Buscamos el objeto que viene de escenas anteriores por su Tag
        GameObject objetoMusicaCargada = GameObject.FindWithTag("MusicaGlobal");

        if (objetoMusicaCargada != null)
        {
            musicaAmbiente = objetoMusicaCargada.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("No se encontró el objeto con el Tag 'MusicaGlobal'. Asegúrate de que el objeto de música anterior lo tenga.");
        }
    }

    public void ActivarMusicaBoss()
    {
        if (musicaAmbiente != null && musicaBoss != null)
            StartCoroutine(Transicion(musicaAmbiente, musicaBoss));
    }

    public void ActivarMusicaAmbiente()
    {
        if (musicaAmbiente != null && musicaBoss != null)
            StartCoroutine(Transicion(musicaBoss, musicaAmbiente));
    }

    IEnumerator Transicion(AudioSource saliente, AudioSource entrante)
    {
        // Si el entrante es el del jefe y no está sonando, lo activamos
        if (!entrante.isPlaying) entrante.Play();
        
        float volInicialSaliente = saliente.volume;
        float t = 0;

        while (t < 1.0f)
        {
            t += Time.deltaTime * velocidadTransicion;
            
            // Bajamos uno y subimos el otro
            saliente.volume = Mathf.Lerp(volInicialSaliente, 0, t);
            entrante.volume = Mathf.Lerp(0, 1, t);
            
            yield return null;
        }

        saliente.Pause(); // Pausamos la ambiental para que no gaste recursos
        saliente.volume = 0;
        entrante.volume = 1;
    }
}