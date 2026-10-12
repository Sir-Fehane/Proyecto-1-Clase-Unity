using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    // Registro de misiles activos en pantalla
    public static HashSet<MissileScript> activeMissiles = new HashSet<MissileScript>();

    public static int ActiveCount
    {
        get
        {
            activeMissiles.RemoveWhere(m => m == null);
            return activeMissiles.Count;
        }
    }

    public float launchDelay = 0.5f; // Tiempo de espera en segundos antes de acelerar
    public float missileSpeed = 3.5f;   // Velocidad del misil

    void OnEnable()
    {
        activeMissiles.Add(this);
    }

    void OnDisable()
    {
        activeMissiles.Remove(this);
    }

    void Start()
    {
        // Iniciamos la espera
        StartCoroutine(LaunchWithDelay());
        Destroy(gameObject, 5f);
    }

    private IEnumerator LaunchWithDelay()
    {
        yield return new WaitForSeconds(launchDelay);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.up * missileSpeed;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}