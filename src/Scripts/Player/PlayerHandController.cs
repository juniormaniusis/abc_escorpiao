using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerHandController : MonoBehaviour
    {
        public Transform Posicao { get; set; }
        public GameObject Item { get; private set; }
        public bool Ocupada => Item != null;

        private void Awake()
        {
            RedefinirPosicao();
        }

        public void RedefinirPosicao()
        {
            var posicao = GameObject.FindGameObjectsWithTag("MAO_PLAYER")
                                    .FirstOrDefault(x => x.activeInHierarchy);
            if (posicao == null)
            {
                Debug.LogError("Nenhum objeto com a tag 'MAO_PLAYER' encontrado na cena.");
                return;
            }
            Posicao = posicao.transform;
        }

        public bool Segurar(GameObject item)
        {
            if (item == null)
            {
                Debug.LogError("O item fornecido é nulo.");
                return false;
            }

            if (Ocupada)
            {
                Debug.LogWarning("A mão já está ocupada com outro item.");
                return false;
            }

            Item = item;
            AtualizarTransformacaoItem();
            return true;
        }

        public void Soltar()
        {
            if (!Ocupada)
            {
                Debug.LogWarning("A mão já está vazia.");
                return;
            }

            Item.transform.SetParent(null);
            Item = null;
        }

        private void AtualizarTransformacaoItem()
        {
            Item.transform.SetPositionAndRotation(Posicao.position, Posicao.rotation);
            Item.transform.localScale = Vector3.one; // Reseta a escala para evitar problemas de escala
            Item.transform.SetParent(Posicao);
        }
    }
}