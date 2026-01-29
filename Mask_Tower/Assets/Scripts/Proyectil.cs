using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 10f;
    public float vidaUtil = 3f; // Se autodestruye a los 3 segs

    void Start()
    {
        Destroy(gameObject, vidaUtil); // Limpieza automática
    }

    void Update()
    {
        transform.Translate(Vector2.right * velocidad * Time.deltaTime);
    }
}