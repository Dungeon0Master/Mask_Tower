using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistenciaUI : MonoBehaviour
{
   private static PersistenciaUI instancia;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}