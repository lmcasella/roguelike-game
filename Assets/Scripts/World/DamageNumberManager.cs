using System.Collections.Generic;
using UnityEngine;

public class DamageNumberManager : MonoBehaviour
{
    // Singleton para acceso global rápido
    public static DamageNumberManager Instance { get; private set; }

    [Header("Configuración del Pool")]
    [SerializeField] private DamageNumber prefabNumber;
    [SerializeField] private int initialPoolSize = 20;

    [Header("Configuración de Colores")]
    [SerializeField] private Color enemyDamageColor = Color.yellow;
    [SerializeField] private Color playerDamageColor = Color.red;

    // Usamos una cola (Queue) porque es la estructura más eficiente para un Pool (First In, First Out)
    private Queue<DamageNumber> pool = new Queue<DamageNumber>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            DamageNumber newNum = Instantiate(prefabNumber, transform);
            newNum.gameObject.SetActive(false); // Nacen apagados
            pool.Enqueue(newNum);
        }
    }

    public void SpawnDamageNumber(Vector3 position, int amount, bool isPlayer)
    {
        DamageNumber spawnedNum;

        // Si hay números disponibles, sacamos uno de la fila
        if (pool.Count > 0)
        {
            spawnedNum = pool.Dequeue();
        }
        else
        {
            // Escalabilidad: Si el jugador hace demasiado daño de área y se acaban los 20, 
            // el sistema no crashea, simplemente crea uno nuevo sobre la marcha.
            spawnedNum = Instantiate(prefabNumber, transform);
        }

        Color finalColor = isPlayer ? playerDamageColor : enemyDamageColor;

        // Variación aleatoria para que los números no se superpongan exactamente igual
        Vector3 randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.2f, 0.2f), 0f);

        spawnedNum.transform.position = position + randomOffset;
        spawnedNum.gameObject.SetActive(true);
        spawnedNum.Setup(amount, finalColor);
    }

    public void ReturnToPool(DamageNumber numToReturn)
    {
        numToReturn.gameObject.SetActive(false); // Lo apagamos
        pool.Enqueue(numToReturn); // Lo mandamos al final de la fila
    }
}