using UnityEngine;

public class EnemyVariantScript : MonoBehaviour
{
    Vector2 velocity;
    float next_move_time;
    float next_spawn_eball;
    int par_impar;
    public GameObject eball;
    int hits;
    SoundScript soundScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundScript = GameObject.FindGameObjectWithTag
        ("soundmanager").GetComponent<SoundScript>();
        par_impar = 1;
        next_move_time = Time.time;
        next_spawn_eball = Time.time;
        hits = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > next_spawn_eball)
        {
            Instantiate(eball, transform.position, transform.rotation);

            next_spawn_eball += Random.Range(1, 5);
        }

        if (Time.time > next_move_time)
        {
            if (par_impar %2 == 0)
            {
                velocity.x = .08f;
            }
            else
            {
                velocity.x = -.08f;
            }

            par_impar++;
            next_move_time += 2;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        int damage = 0;

        if (collision.gameObject.CompareTag("bullet"))
        {
            damage = 1;
        }
        else if (collision.gameObject.CompareTag("missile"))
        {
            damage = 5;
        }
        else
        {
            return; // Si choca con otra cosa que no sea bala ni misil, no hace nada
        }

        
        hits += damage;

        Destroy(collision.gameObject);

        if (hits >= 5)
        {
            soundScript.PlayAudioEnemyDestroy();

            GameManager.score+=5;
            int max = PlayerPrefs.GetInt("max");
            if (GameManager.score > max)
            {
                PlayerPrefs.SetInt("max", GameManager.score);
                PlayerPrefs.Save();
            }

            Destroy(gameObject);
        }
        else
        {
            soundScript.PlayAudioEnemyShieldCollision();
        }
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().position += (Vector2)(transform.up * 0.01f) + new Vector2(velocity.x, 0);
    }
}
