using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Heredamos del Charger para tener la habilidad de carga
public class BossAI : EnemyMelee_Charger
{
    [Header("Boss Spawning Phase")]
    [SerializeField] private List<GameObject> minionPrefabs; // Adds
    private List<Transform> spawnPoints = new List<Transform>(); // Adds spawn
    [SerializeField] private float timeBetweenWaves = 10f;

    private bool isMinionPhase = false;
    private float waveTimer = 0f;
    private List<GameObject> activeMinions = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        // Buscar automaticamente los puntos de spawn de minions
        GameObject[] foundPoints = GameObject.FindGameObjectsWithTag("BossMinionSpawn");

        foreach (GameObject obj in foundPoints)
        {
            spawnPoints.Add(obj.transform);
        }

        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("BossAI: No encontré puntos de spawn con el tag 'BossMinionSpawn'");
        }
    }

    protected override void Update()
    {
        // 1. Chequeo de minions vivos
        CheckMinionsStatus();

        // 2. Si estamos en fase de espera, contar el tiempo
        if (isMinionPhase)
        {
            waveTimer -= Time.deltaTime;

            if (waveTimer <= 0)
            {
                SpawnWave();
                isMinionPhase = false; // Salir de la fase de espera
            }
        }

        // 3. Ejecutar la lógica normal (Perseguir/Cargar)
        base.Update();
    }

    private void CheckMinionsStatus()
    {
        // Limpiar la lista de minions muertos
        activeMinions.RemoveAll(minion => minion == null);

        // Si no quedan minions y no estamos ya esperando, activamos el timer
        if (activeMinions.Count == 0 && !isMinionPhase)
        {
            Debug.Log("Boss: Minions muertos. Esperando minions...");
            isMinionPhase = true;
            waveTimer = timeBetweenWaves;
        }
    }

    private void SpawnWave()
    {
        if (spawnPoints.Count == 0) return;

        Debug.Log("Boss: Invocando minions");

        // Spawnear 3 enemigos
        for (int i = 0; i < 3; i++)
        {
            // Elegir enemigo al azar
            GameObject randomEnemy = minionPrefabs[Random.Range(0, minionPrefabs.Count)];

            // Elegir punto al azar
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            GameObject newMinion = Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
            activeMinions.Add(newMinion);
        }
    }
}