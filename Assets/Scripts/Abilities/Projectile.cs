using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 0.4f;

    // Daño final que envia PlayerAbilities
    protected int damage;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Destruir projectil despues de 5 segundos
        //FIXME: En vez de proyectiles deberian ser habilidades con un rango determinado que duren X tiempo, no que sigan de largo por el mapa en una direccion
        Destroy(gameObject, lifetime);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Se mueve hacia adelante
        rb.velocity = transform.up * speed;   
    }

    // Funcion que PlayerAbilities llama para crear el proyectil a indicar cuanto daño hace
    public void Initialize(int damageAmount)
    {
        this.damage = damageAmount;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Buscar si lo que chocó tiene un sistema de vida
        SystemHealth health = collision.gameObject.GetComponent<SystemHealth>();

        if (health != null)
        {
            // 2. Si tiene, hacerle daño
            health.DealDamage(damage);
        }

        // 3. Destruir proyectil al impactar
        Destroy(gameObject);
    }
}
