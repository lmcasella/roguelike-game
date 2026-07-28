using UnityEngine;

public class VampireBehaviour : MonoBehaviour
{
    private SystemHealth myHealth;
    private int healAmount;

    void Awake()
    {
        myHealth = GetComponent<SystemHealth>();
    }

    public void Initialize(int amount)
    {
        healAmount = amount;
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
            myHealth.Heal(healAmount);
        }
    }
}