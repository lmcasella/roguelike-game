using UnityEngine;

// T es reemplazado por la clase que es heredada (ej. GameManager o AudioManager)
// where T : Component significa que T debe ser un componente de Unity, como un MonoBehaviour
public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    // La instancia que se crea en cada clase donde se va a utilizar el Singleton
    private static T _instance;

    // De esta forma se accede a la instancia
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    Debug.LogError($"Singleton: An instance of {typeof(T)} is needed, but not found.");
                }
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;

            // No destruir el objeto al cargar nuevas escenas
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Si la instancia ya existe pero no es esta misma significa que hay un duplicado, entonces se debe eliminar
            if (_instance != this)
            {
                Debug.LogWarning($"Singleton: A duplicate instance of {typeof(T)} was found. Destroying.");
                Destroy(gameObject);
            }
        }
    }
}
