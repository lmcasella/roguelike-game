using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Referencias")]
    // Todas las puertas de la sala
    [SerializeField] private List<Door> doors = new List<Door>();

    [Header("Configuracion de Spawners")]
    [Tooltip("Puntos vacíos en la escena donde aparecerán los enemigos")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Tooltip("Los prefabs de enemigos que pueden salir (Orcos, Esqueletos...)")]
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();

    [Header("Reglas del Nivel")]
    [SerializeField] private int totalEnemiesToKill = 10; // Meta para abrir la puerta y activar recompensas
    [SerializeField] private int maxActiveEnemies = 3;    // Límite enemigos en simultáneo
    [SerializeField] private float spawnInterval = 2f;    // Tiempo entre apariciones

    private List<Enemy> activeEnemies = new List<Enemy>(); // Lista dinámica
    private int enemiesKilledCount = 0;
    private int enemiesSpawnedCount = 0;
    private bool roomActive = false;
    private bool roomCleared = false;
    private float nextSpawnTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Abrir puertas hasta que el jugador entre
        OpenDoors();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el jugador entra y la sala no se limpio ni se activo
        if (collision.CompareTag("Player") && !roomActive && !roomCleared)
        {
            ActivateRoom();
        }
    }

    public void ActivateRoom()
    {
        roomActive = true;
        enemiesKilledCount = 0;
        enemiesSpawnedCount = 0;

        Debug.Log($"Goal: Kill {totalEnemiesToKill} enemies.");

        CloseDoors();

        // Suscribirse al evento global de muerte
        GameEvents.OnEnemyDied += OnEnemyDiedHandler;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameEvents.ReportRoomCleared();
        }

        if (!roomActive) return;

        // LOGICA DE SPAWNEO
        // 1. ¿Ya ganamos? Si ya spawneamos todos los necesarios, no spawnear más
        if (enemiesSpawnedCount >= totalEnemiesToKill) return;

        // 2. ¿Hay espacio? (Menos enemigos vivos que el máximo)
        if (activeEnemies.Count < maxActiveEnemies)
        {
            // 3. ¿Pasó el tiempo de cooldown?
            if (Time.time >= nextSpawnTime)
            {
                SpawnEnemy();
                nextSpawnTime = Time.time + spawnInterval;
            }
        }
    }

    private void SpawnEnemy()
    {
        // Seguridad: Chequear que esté configurado qué y dónde spawnear
        if (spawnPoints.Count == 0 || enemyPrefabs.Count == 0) return;

        // 1. Elegir un punto al azar
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        // 2. Elegir un enemigo al azar
        GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        // 3. Crear el enemigo
        GameObject newEnemyObj = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

        // 4. Registrarlo en la lista local
        Enemy enemyScript = newEnemyObj.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            activeEnemies.Add(enemyScript);
            enemiesSpawnedCount++;
            Debug.Log($"Spawned Enemy ({enemiesSpawnedCount}/{totalEnemiesToKill})");
        }
    }

    private void OnEnemyDiedHandler(Enemy enemy, int scoreValue)
    {
        if (!roomActive) return;

        // Verificar si el enemigo que murió pertenece a ESTA sala
        if (activeEnemies.Contains(enemy))
        {
            // Lo sacamos de la lista de activos para liberar el esacio
            activeEnemies.Remove(enemy);

            // Sumamos al contador de muertes
            enemiesKilledCount++;
            Debug.Log($"Enemy Killed. Progress: {enemiesKilledCount}/{totalEnemiesToKill}");

            // Chequeo de Victoria
            if (enemiesKilledCount >= totalEnemiesToKill)
            {
                ClearRoom();
            }
        }
    }

    private void ClearRoom()
    {
        roomActive = false;
        roomCleared = true;
        Debug.Log("Room Cleared");

        OpenDoors();
        GameEvents.OnEnemyDied -= OnEnemyDiedHandler;
        GameEvents.ReportRoomCleared();
    }

    private void CloseDoors()
    {
        foreach (Door door in doors) door.Close();
    }

    private void OpenDoors()
    {
        foreach (Door door in doors) door.Open();
    }
}
