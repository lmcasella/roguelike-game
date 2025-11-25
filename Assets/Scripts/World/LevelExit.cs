using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "Level_2";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Obtener referencias
            var health = collision.GetComponent<SystemHealth>();
            var mana = collision.GetComponent<PlayerMana>();
            var stats = collision.GetComponent<PlayerStats>();

            // 2. Guardar en el GameManager
            if (health != null && mana != null)
            {
                GameManager.Instance.SavePlayerState(
                    health.GetCurrentHealth(),
                    health.GetMaxHealth(),
                    mana.GetCurrentMana(),
                    mana.GetMaxMana(),
                    stats
                );
            }

            // 3. Cambiar Escena
            SceneManager.LoadScene(nextSceneName);
        }
    }
}