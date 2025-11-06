// Define que metodos va a tener un objeto dañable
public interface IDamageable
{
    // Variable para obtener la vida actual del objeto
    int CurrentHealth { get; }

    // Funcion para recibir daño
    void TakeDamage(int damageAmount);

    // Funcion para manejar la muerte del objeto
    void Die();
}
