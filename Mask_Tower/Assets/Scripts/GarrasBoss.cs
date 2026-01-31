using UnityEngine;

public class GarraBoss : MonoBehaviour
{
    public Transform boss;
    public Vector2 offset;
    public float suavidad = 5f;

    void Update()
    {
        Vector2 objetivo = (Vector2)boss.position + offset;
        transform.position = Vector2.Lerp(
            transform.position,
            objetivo,
            suavidad * Time.deltaTime
        );
    }
}
