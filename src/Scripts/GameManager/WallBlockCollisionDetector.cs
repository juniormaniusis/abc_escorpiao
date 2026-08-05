using PixelCrushers.DialogueSystem;
using UnityEngine;
namespace Assets.Scripts.GameManager
{
    namespace Assets.Scripts.GameManager
    {
        public class WallBlockCollisionDetector : MonoBehaviour
        {
            void OnTriggerEnter(Collider other)
            {
                // Verifica se o player entrou em contato
                if (other.CompareTag("Player"))
                {
                    DialogueManager.ShowAlert("Você ainda não pode passar por aqui.", 1);
                }
            }
        }
    }
}