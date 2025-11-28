using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 3f;

    // Daño final que envia PlayerAbilities
    protected int damage;

    private Rigidbody2D rb;

    // Prevenir que al impactar un objeto que está cerca del Player le haga daño igual antes de destruirse
    private bool hasHit = false;

    private GameObject owner;

    public void SetProjSpeed(int value) { speed = value; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Destruir projectil
        //FIXME: En vez de proyectiles deberian ser habilidades con un rango determinado que duren X tiempo, no que sigan de largo por el mapa en una direccion
        Destroy(gameObject, lifetime);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Se mueve hacia adelante
        rb.velocity = transform.right * speed;   
    }

    // Funcion que PlayerAbilities llama para crear el proyectil a indicar cuanto daño hace
    public void Initialize(int damageAmount, GameObject shooter = null)
    {
        this.damage = damageAmount;
        this.owner = shooter;

        if (owner != null)
        {
            Collider2D myCollider = GetComponent<Collider2D>();
            Collider2D ownerCollider = owner.GetComponent<Collider2D>();

            if (myCollider != null && ownerCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, ownerCollider);
            }
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasHit) return;

        if (owner != null && collision.gameObject == owner) return;

        hasHit = true;

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
