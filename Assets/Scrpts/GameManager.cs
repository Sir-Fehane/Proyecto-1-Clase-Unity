using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int score;

    [Header("Prefabs de Spawns")]
    public GameObject asteroid;
    public GameObject powerup;
    public GameObject missilepowerup;
    public GameObject heart;
    public GameObject enemyType1;
    public GameObject enemyType2;
    [Header("Probabilidad de Corazón (0 a 1)")]
    [Range(0f, 1f)]
    public float heartSpawnChance = 0.15f; // 15% de probabilidad por defecto

    [Header("Tiempos de Spawn")]
    private float next_time_spawn_asteroid;
    private float next_time_spawn_powerup;
    private float next_time_spawn_enemy;

    void Start()
    {
        score = 0;
        next_time_spawn_asteroid = Time.time + 1f;
        next_time_spawn_enemy = Time.time + 2f;
        next_time_spawn_powerup = Time.time + Random.Range(15f, 25f);

        if (!PlayerPrefs.HasKey("max"))
        {
            PlayerPrefs.SetInt("max", 0);
        }
    }

    void Update()
    {
        SpawnPowerUps();
        SpawnAsteroids();
        SpawnEnemies();
    }

    void SpawnPowerUps()
    {
        if (Time.time > next_time_spawn_powerup)
        {
            // Aparece dentro de los límites jugables de la pantalla
            Vector2 spawnPos = new Vector2(Random.Range(-7f, 7f), Random.Range(-3.5f, 3.5f));

            GameObject selectedItem;

            // El corazón solo aparece con una probabilidad baja (15% por defecto)
            if (heart != null && Random.value < heartSpawnChance)
            {
                selectedItem = heart;
            }
            else
            {
                // El resto de las veces alterna entre el PowerUp normal y el de Misiles
                selectedItem = (Random.value > 0.5f) ? powerup : missilepowerup;
            }

            if (selectedItem != null)
            {
                Instantiate(selectedItem, spawnPos, Quaternion.identity);
            }

            next_time_spawn_powerup = Time.time + Random.Range(15f, 25f);
        }
    }

    void SpawnAsteroids()
    {
        if (Time.time > next_time_spawn_asteroid)
        {
            // 1. Determina si aparece Arriba (Y = 5.5) o Abajo (Y = -5.5)
            bool isTop = Random.value > 0.5f;
            float spawnY = isTop ? 5.5f : -5.5f;
            float spawnX = Random.Range(-7f, 7f);

            GameObject newAsteroid = Instantiate(asteroid, new Vector2(spawnX, spawnY), Quaternion.identity);

            // 2. Modifica el tamaño aleatoriamente
            float scale = Random.Range(0.4f, 1.2f);
            newAsteroid.transform.localScale = new Vector3(scale, scale, 1f);

            // 3. Modifica la velocidad y dirección según su punto de origen
            Rigidbody2D rb = newAsteroid.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float speedY = Random.Range(1.5f, 4f);
                float directionY = isTop ? -speedY : speedY; // Si viene de arriba baja (-), si viene de abajo sube (+)
                float speedX = Random.Range(-0.8f, 0.8f);    // Inclinación leve diagonal

                rb.linearVelocity = new Vector2(speedX, directionY);
            }

            next_time_spawn_asteroid = Time.time + Random.Range(0.8f, 2.5f);
        }
    }

    void SpawnEnemies()
    {
        if (Time.time > next_time_spawn_enemy)
        {
            // 1. Selecciona aleatoriamente entre el Enemigo 1 y el Enemigo 2
            GameObject selectedEnemy = (Random.value > 0.5f) ? enemyType1 : enemyType2;

            // 2. Determina si aparece Arriba o Abajo
            bool isTop = Random.value > 0.5f;
            float spawnY = isTop ? 5.5f : -5.5f;
            float spawnX = Random.Range(-7f, 7f);

            // Si aparece abajo, rotamos 180° la nave para que mire y se mueva hacia arriba
            Quaternion spawnRotation = isTop ? Quaternion.identity : Quaternion.Euler(0, 0, 180f);

            GameObject newEnemy = Instantiate(selectedEnemy, new Vector2(spawnX, spawnY), spawnRotation);

            // 3. Aplica movimiento inicial de acuerdo al extremo
            Rigidbody2D rb = newEnemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float speedY = Random.Range(1f, 3f);
                float directionY = isTop ? -speedY : speedY;
                rb.linearVelocity = new Vector2(0, directionY);
            }

            next_time_spawn_enemy = Time.time + Random.Range(2f, 5f);
        }
    }

    public void BotonGameOver()
    {
        SceneManager.LoadScene(0);
    }
}