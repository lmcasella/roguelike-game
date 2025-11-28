using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [SerializeField] private int maxMana = 100;

    private int currentMana;
    private bool isInfinite = false;

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
        if (isInfinite) return true;
        return currentMana >= manaCost;
    }

    // Gastar mana. Debe llamarse despues de checkear si tiene suficiente mana
    public bool SpendMana(int manaCost)
    {
        // 1. Verificar si es infinito
        if (isInfinite) return true;

        // 2. Verificar si alcanza (sin restar todavía)
        if (currentMana >= manaCost)
        {
            // 3. Restamos
            currentMana -= manaCost;

            GameEvents.ReportPlayerManaChanged(currentMana, maxMana);
            return true;
        }

        // 4. No alcanza
        return false;
    }

    public IEnumerator ActivateInfiniteMana(float duration)
    {
        isInfinite = true;
        // TODO: Feedback visual (ej. cambiar color de la barra)
        Debug.Log("¡MANÁ INFINITO ACTIVADO!");

        yield return new WaitForSeconds(duration);

        isInfinite = false;
        Debug.Log("Fin del Maná Infinito");
    }

    // Incrementar el mana maximo (ej. upgrade)
    public void IncrementMana(int amount)
    {
        maxMana += amount;
        currentMana += amount;

        // Notificar nuevo mana
        GameEvents.ReportPlayerManaChanged(currentMana, maxMana);
    }

    public int GetCurrentMana() => currentMana;
    public int GetMaxMana() => maxMana;
    public void SetMaxMana(int value) { maxMana = value; }
    public void SetMana(int value) { currentMana = value; }
}
