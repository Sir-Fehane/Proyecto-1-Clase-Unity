using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Vector2 bullet_velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.up * 10f;

        // Destrucción de seguridad para garantizar que nunca se quede en la escena
        Destroy(gameObject, 3f);

        //bullet_velocity.y = .8f;
        //bullet_velocity.x = .2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBecameInvisible()
    {
        // Si pertenece a un grupo de balas (Bullets) y es la última restante, destruye también el padre
        if (transform.parent != null && transform.parent.childCount <= 1)
        {
            Destroy(transform.parent.gameObject);
        }

        Destroy(gameObject);
    }

    void OnDestroy()
    {
        // Si fue destruida por colisión y era la última restante del grupo, destruye también el padre
        if (transform.parent != null && transform.parent.childCount <= 1)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    void FixedUpdate()
    {
        //GetComponent<Rigidbody2D>().position += bullet_velocity;
    }
}
