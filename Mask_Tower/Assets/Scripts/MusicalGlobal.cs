using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalGlobal : MonoBehaviour
{
    private static MusicalGlobal instancia;

    void Awake()
    {
        // Si ya existe una instancia de música, destruimos esta nueva
        if (instancia != null && instancia != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Si es la primera vez, la marcamos como principal y pedimos que no se destruya
        instancia = this;
        DontDestroyOnLoad(this.gameObject);
    }
}