using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // Player
    [SerializeField] private float smoothSpeed = 0.125f; // Suavidad del seguimiento
    [SerializeField] private Vector3 offset = new Vector3(0,0,-10); // Desplazamiento desde el objetivo

    void LateUpdate()
    {
        if (target == null) return;

        // Posicion deseada: Donde esta el jugador + offset
        Vector3 desiredPosition = target.position + offset;

        // Interpolacion suave (lerp) entre donde esta la camara y donde quiere ir
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Aplicar
        transform.position = smoothPosition;
    }
}
