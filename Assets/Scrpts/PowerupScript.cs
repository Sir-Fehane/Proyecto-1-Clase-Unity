using UnityEngine;

public class PowerupScript : MonoBehaviour
{
    SoundScript soundScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundScript = GameObject.FindGameObjectWithTag
        ("soundmanager").GetComponent<SoundScript>();
        Destroy(gameObject, 4);
    }

    void OnCollisionEnter2D(Collision2D collision)
    { 
        if (collision.gameObject.tag == "Player")
        {
            soundScript.PlayAudioPowerUp();   
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
