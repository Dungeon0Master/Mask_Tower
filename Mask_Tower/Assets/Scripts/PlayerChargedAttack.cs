using UnityEngine;

public class PlayerChargedAttack : MonoBehaviour
{
  
    public GameObject prefabProyectil; 
    public Transform puntoDisparo;    
    public float tiempoParaCargar = 1.5f;
    
    public Color colorCargaLista = Color.red;
    private float tiempoPresionado = 0f;
    private bool estaCargado = false;
    private bool disparando = false; //  Filtro de seguridad

    private SpriteRenderer miSprite;
    private Animator anim;

    void Awake()
    {
        miSprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>(); 
    }

    public void GestionarCarga(bool botonPresionado)
    {
        if (anim != null) anim.SetBool("Cargando", botonPresionado);

        if (botonPresionado)
        {
            // Lógica de carga...
            tiempoPresionado += Time.deltaTime;
            if (tiempoPresionado >= tiempoParaCargar && !estaCargado)
            {
                estaCargado = true;
                miSprite.color = Color.yellow; 
            }
        }
        else // Soltamos botón
        {
            if (estaCargado)
            {
                PrepararDisparo();
            }
            // Reset
            tiempoPresionado = 0;
            estaCargado = false;
            miSprite.color = Color.white; 
        }
    }

    void PrepararDisparo()
    {
        disparando = true; 
        // Como usamos la misma animación de ataque, reutilizamos el trigger "atacar"
        if (anim != null) anim.SetTrigger("atacar"); 
    }

    public void EventoGolpe()
    {
        // VERIFICACIÓN: Solo disparamos si venimos de una carga
        if (disparando)
        {
            DispararProyectil();
            disparando = false; // APAGAMOS EL FILTRO (Reset inmediato tras disparar)
        }
    }

    void DispararProyectil()
    {
        if (prefabProyectil != null && puntoDisparo != null)
        {
            // 1. Instanciamos el proyectil
            GameObject nuevoProyectil = Instantiate(prefabProyectil, puntoDisparo.position, puntoDisparo.rotation);
            
            // 2. Obtenemos el script del proyectil
            Proyectil scriptProyectil = nuevoProyectil.GetComponent<Proyectil>();

            float dirX = transform.localScale.x >= 0 ? 1f : -1f;

            if (scriptProyectil != null)
            {
                // Le pasamos la dirección correcta (1,0) o (-1,0)
                scriptProyectil.SetDireccion(new Vector2(dirX, 0));
                
            }
        }
    }
}