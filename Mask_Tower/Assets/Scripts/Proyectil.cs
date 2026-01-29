using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 10f;
    public float vidaUtil = 3f;

    private Vector2 direccion = Vector2.right;

    public void SetDireccion(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion.normalized;
    }

    void Start()
    {
        Destroy(gameObject, vidaUtil);
    }

    void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }
}
