using UnityEngine;

namespace Assets.Scripts.GameManager
{
    public class PersistentObject : MonoBehaviour
    {
        private static PersistentObject instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject); // Destroi novos objetos duplicados
            }
        }
    }
}