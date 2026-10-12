using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    Vector2 asteroid_velocity;

    public List<Sprite> sprites;
    int hits;
    SoundScript soundScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundScript = GameObject.FindGameObjectWithTag
        ("soundmanager").GetComponent<SoundScript>();

        float escala = Random.Range(.02f, .09f);
        transform.localScale = new Vector2(escala, escala);

        float velocidad = Random.Range(.014f, .04f);

        asteroid_velocity.y = -velocidad;
        hits = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward);
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
            damage = 4;
        }
        else
        {
            return;
        }

        Destroy(collision.gameObject);
        hits += damage;
        if (hits >= 3)
        {
            soundScript.PlayAudioExplosion();
            GameManager.score+=2;

            int max = PlayerPrefs.GetInt("max");
            if (GameManager.score > max)
            {
                PlayerPrefs.SetInt("max", GameManager.score);
                PlayerPrefs.Save(); // Guarda el récord en disco inmediatamente
            }

            Destroy(gameObject);
        }
        else
        {
            soundScript.PlayAudioExplosion();
            int spriteIndex = hits - 1;
            if (spriteIndex >= 0 && spriteIndex < sprites.Count)
            {
                GetComponent<SpriteRenderer>().sprite = sprites[spriteIndex];
            }
        }
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().position += asteroid_velocity;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
