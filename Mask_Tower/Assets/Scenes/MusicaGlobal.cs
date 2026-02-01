using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicaGlobal : MonoBehaviour
{
    private static MusicaGlobal instancia;

    void Awake()
    {
        // Patrón Singleton: Si ya existe una música, destruye esta nueva
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // Esto hace que no muera al cambiar de nivel
        }
        else
        {
            Destroy(gameObject);
        }
    }
}