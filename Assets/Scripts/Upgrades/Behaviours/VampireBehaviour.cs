using UnityEngine;

public class VampireBehaviour : MonoBehaviour
{
    private SystemHealth myHealth;

    void Awake()
    {
        myHealth = GetComponent<SystemHealth>();
    }

    void OnEnable()
    {
        GameEvents.OnEnemyDied += HealOnKill;
    }

    void OnDisable()
    {
        GameEvents.OnEnemyDied -= HealOnKill;
    }

    private void HealOnKill(Enemy enemy, int score)
    {
        if (myHealth != null)
        {
            // FIXME: Deberia poder cambiarse desde el Inspector
            myHealth.Heal(5);
        }
    }
}