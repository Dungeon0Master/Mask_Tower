using UnityEngine;

public class LifeUIManager : MonoBehaviour
{
    public static LifeUIManager Instance;

    public HeartUI[] corazones; // ORDENADOS de IZQUIERDA A DERECHA

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RecibirDaño(int cantidad)
    {
        for (int i = corazones.Length - 1; i >= 0; i--)
        {
            if (!corazones[i].EstaVacio())
            {
                corazones[i].PerderVida();
                break;
            }
        }
    }
}
