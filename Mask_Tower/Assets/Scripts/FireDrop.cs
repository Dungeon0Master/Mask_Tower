using UnityEngine;

public class FireDrop : MonoBehaviour
{
    public float velocidadCaida = 8f;
    public float vida = 5f;

    void Start()
    {
        Destroy(gameObject, vida);
    }

    void Update()
    {
        transform.Translate(Vector2.down * velocidadCaida * Time.deltaTime);
    }
}
