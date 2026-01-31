using UnityEngine;

public class GarraBoss : MonoBehaviour
{
    [Header("Referencia")]
    public Transform boss;

    [Header("Movimiento")]
    public Vector2 offset;
    public float suavidad = 5f;

    void Update()
    {
        if (boss == null) return;

        Vector2 objetivo = (Vector2)boss.position + offset;

        transform.position = Vector2.Lerp(
            transform.position,
            objetivo,
            suavidad * Time.deltaTime
        );
    }
}
