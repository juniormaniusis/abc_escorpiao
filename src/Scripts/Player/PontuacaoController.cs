using MoreMountains.Feedbacks;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PontuacaoController : MonoBehaviour
    {
        public static PontuacaoController Instance { get; private set; }

        public Pontuacao Pontuacao { get; private set; }
        public MMF_Player GanharPontosFeedbacks;
        public MMF_Player GanharPontosMultiplosDe10Feedbacks;
        public MMF_Player PerderPontosFeedbacks;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.parent = null;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            Pontuacao = new Pontuacao(0);
        }

        public void AdicionarPontos(int qtd = 1, bool playFeedback = true)
        {
            StartCoroutine(AdicionarPontosComFeedback(qtd, playFeedback));
        }

        private System.Collections.IEnumerator AdicionarPontosComFeedback(int qtd, bool playFeedback)
        {
            for (int i = 0; i < qtd; i++)
            {
                Pontuacao.Adicionar(1);

                if (!playFeedback) continue;

                if (
                    Pontuacao.Valor % 10 == 0
                    && qtd > 0
                    && GanharPontosMultiplosDe10Feedbacks != null)
                {
                    GanharPontosMultiplosDe10Feedbacks.PlayFeedbacks();
                    yield break;
                }

                if (GanharPontosFeedbacks != null)
                {
                    GanharPontosFeedbacks.PlayFeedbacks();
                    yield return new WaitForSeconds(0.1f); // Delay de 0.1 segundo entre cada som
                }
            }
        }

        public void RemoverPontos(int qtd = 1, bool playFeedback = true)
        {
            Pontuacao.Remover(qtd);

            if (!playFeedback) return;

            if (PerderPontosFeedbacks != null)
            {
                PerderPontosFeedbacks.PlayFeedbacks();
            }

        }

        public void DefinirPontuacao(long valor)
        {
            Pontuacao.DefinirPontuacaoSalva(valor);
        }
    }
}