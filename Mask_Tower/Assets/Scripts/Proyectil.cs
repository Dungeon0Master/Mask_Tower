using UnityEngine;

public class Proyectil : MonoBehaviour
{
    public float velocidad = 10f;
    public float vidaUtil = 3f;

    private Vector2 direccion = Vector2.right;

    public void SetDireccion(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion.normalized;
        Vector3 escala = transform.localScale;

        if (direccion.x < 0) 
        {
            // Si va a la izquierda, forzamos la escala X a negativa
            escala.x = -Mathf.Abs(escala.x);
        }
        else if (direccion.x > 0)
        {
            // Si va a la derecha, forzamos la escala X a positiva
            escala.x = Mathf.Abs(escala.x);
        }

        transform.localScale = escala;
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
