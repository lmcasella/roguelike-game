using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float interactionRadius = 1.5f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private KeyCode interactKey = KeyCode.F; // Tecla de interaccion

    private IInteractable currentInteractable;

    private void Update()
    {
        CheckForInteractables();

        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            // Interactuar
            currentInteractable.Interact(GetComponent<Player>());
        }
    }

    private void CheckForInteractables()
    {
        // Area de detección
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);

        if (hits.Length > 0)
        {
            // Buscamos el componente que tenga la interfaz Interactable
            IInteractable found = hits[0].GetComponent<IInteractable>();

            if (found != null)
            {
                currentInteractable = found;
                return;
            }
        }

        // Si no encontramos nada
        currentInteractable = null;
    }

    // Dibujo para ver el rango en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
