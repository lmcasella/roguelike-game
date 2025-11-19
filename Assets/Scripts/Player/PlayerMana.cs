using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private int maxMana = 100;

    private int currentMana;

    // Start is called before the first frame update
    void Start()
    {
        currentMana = maxMana;

        GameEvents.ReportPlayerManaChanged(currentMana, maxMana);
    }

    // Update is called once per frame
    void Update()
    {
        // --- Regeneracion de Mana ---
        // Regenerar si no está full de mana
        if (currentMana < maxMana)
        {
            // TODO: Logica de regeneracion de mana al matar enemigos o agarrar items

            // Si se pasa del mana maximo lockear al valor del mana maximo
            if (currentMana > maxMana)
            {
                currentMana = maxMana;
            }

            // Notificar nuevo valor de mana
            GameEvents.ReportPlayerManaChanged(currentMana, maxMana);
        }
    }

    // Checkear si el jugador tiene suficiente mana para tirar una habilidad
    public bool HasEnoughMana(int manaCost)
    {
        return currentMana >= manaCost;
    }

    // Gastar mana. Debe llamarse despues de checkear si tiene suficiente mana
    public void SpendMana(int manaCost)
    {
        currentMana -= manaCost;

        if (currentMana < 0)
        {
            currentMana = 0;
        }

        // Notificar nuevo mana
        GameEvents.ReportPlayerManaChanged(currentMana, maxMana);
    }

    // Incrementar el mana maximo (ej. upgrade)
    public void IncrementMana(int amount)
    {
        maxMana += amount;
        currentMana += amount;

        // Notificar nuevo mana
        GameEvents.ReportPlayerManaChanged(currentMana, maxMana);
    }
}
