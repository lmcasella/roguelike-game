using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [Header("Referencias")]
    // Todas las puertas de la sala
    [SerializeField] private List<Door> doors = new List<Door>();
    [SerializeField] private List<Enemy> enemiesInRoom = new List<Enemy>();

    private int activeEnemyCount = 0;
    private bool roomCleared = false;
    private bool roomActive = false;

    // Start is called before the first frame update
    void Start()
    {
        // Abrir puertas hasta que el jugador entre
        OpenDoors();

        //TODO: Logica para spawnear enemigos
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
        Debug.Log("Room Active");

        // 1. Cerrar puertas
        CloseDoors();

        // 2. Activar enemigos y contarlos

        // 3. Suscribirse al evento de muerte de enemigos
        // Importa solo si mueren mientras esta sala esta activa
        GameEvents.OnEnemyDied += OnEnemyDiedHandler;
    }

    private void OnEnemyDiedHandler(Enemy enemy, int scoreValue)
    {
        // Ejecutar funcion solo si la sala esta activa
        if (!roomActive) return;

        if (enemiesInRoom.Contains(enemy))
        {
            activeEnemyCount--;
            Debug.Log($"Enemy died. Remaining: {activeEnemyCount}");
        }

        if (activeEnemyCount <= 0)
        {
            ClearRoom();
        }
    }

    private void ClearRoom()
    {
        roomActive = false;
        roomCleared = true;
        Debug.Log("Room Cleared");

        // 1. Abrir puertas
        OpenDoors();

        // 2. Dejar de escuchar muertes
        GameEvents.OnEnemyDied -= OnEnemyDiedHandler;

        // 3. Evento global para la recompensa
        GameEvents.ReportRoomCleared();
    }

    private void CloseDoors()
    {
        foreach (Door door in doors)
        {
            door.Close();
        }
    }

    private void OpenDoors()
    {
        foreach (Door door in doors)
        {
            door.Open();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameEvents.ReportRoomCleared();
        }
    }
}
