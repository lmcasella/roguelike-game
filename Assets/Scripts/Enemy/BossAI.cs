using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Heredamos del Charger para tener la habilidad de embestir
public class BossAI : EnemyMelee_Charger
{
    [Header("Boss Spawning Phase")]
    [SerializeField] private List<GameObject> minionPrefabs; // Adds
    [SerializeField] private Transform[] spawnPoints; // Spawns de adds (GameObjects vacíos)
    [SerializeField] private float timeBetweenWaves = 10f;

    private bool isMinionPhase = false;
    private float waveTimer = 0f;
    private List<GameObject> activeMinions = new List<GameObject>();

    protected override void Update()
    {
        // 1. Chequeo de minions vivos
        CheckMinionsStatus();

        // 2. Si estamos en fase de espera (minions muertos), contamos el tiempo
        if (isMinionPhase)
        {
            waveTimer -= Time.deltaTime;

            if (waveTimer <= 0)
            {
                SpawnWave();
                isMinionPhase = false; // Volvemos a pelear
            }
        }

        // 3. Ejecutar la lógica normal (Perseguir/Cargar)
        base.Update();
    }

    private void CheckMinionsStatus()
    {
        // Limpiamos la lista de minions muertos
        activeMinions.RemoveAll(minion => minion == null);

        // Si no quedan minions y NO estamos ya esperando, activamos el timer
        if (activeMinions.Count == 0 && !isMinionPhase)
        {
            Debug.Log("Boss: ¡Minions muertos! Esperando refuerzos...");
            isMinionPhase = true;
            waveTimer = timeBetweenWaves;
        }
    }

    private void SpawnWave()
    {
        Debug.Log("Boss: ¡Invocando refuerzos!");

        // Spawnear 3 enemigos
        for (int i = 0; i < 3; i++)
        {
            // Elegir enemigo al azar
            GameObject randomEnemy = minionPrefabs[Random.Range(0, minionPrefabs.Count)];

            // Elegir punto al azar
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject newMinion = Instantiate(randomEnemy, randomPoint.position, Quaternion.identity);
            activeMinions.Add(newMinion);
        }
    }
}