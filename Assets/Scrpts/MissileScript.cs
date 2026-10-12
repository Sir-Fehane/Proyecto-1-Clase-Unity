using System.Collections;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    public float launchDelay = 0.5f; // Tiempo de espera en segundos antes de acelerar
    public float missileSpeed = 3f;   // Velocidad del misil

    void Start()
    {
        // Iniciamos la espera
        StartCoroutine(LaunchWithDelay());
        Destroy(gameObject, 5f);
    }

    private IEnumerator LaunchWithDelay()
    {
        yield return new WaitForSeconds(launchDelay);

        GetComponent<Rigidbody2D>().linearVelocity = transform.up * missileSpeed;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}