using System.Collections;
using Assets.Scripts.Enums;
using MoreMountains.Feedbacks;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using SaveSystem = PixelCrushers.SaveSystem;

namespace Assets
{
    public class AmbulanciaController : MonoBehaviour
    {
        [Header("Movimento")] public float velocidadeNormal = 10f;
        public float velocidadeEmergencia = 25f;
        private float velocidadeAtual;

        public Transform pontoDeParada;

        [Header("Semáforo")] public GameObject imagemSemaforoVermelho;

        [Header("Modo Emergência")]
        [Tooltip("Feedback Feel com efeito de sirene (AudioSource), speed lines, camera shake, etc.")]
        [SerializeField]
        private MMF_Player emergenciaFeedback;

        [Tooltip("Feedback para encerrar o modo emergência (opcional - para parar loops)")] [SerializeField]
        private MMF_Player emergenciaFimFeedback;

        [Tooltip("Duração em segundos do modo emergência antes de voltar ao normal")] [SerializeField]
        private float duracaoEmergencia = 8f;

        [Header("Speed Lines (Sensação de Velocidade)")]
        [Tooltip("Particle System de speed lines (linhas de velocidade) - filho da ambulância")]
        [SerializeField]
        private ParticleSystem speedLinesParticles;

        [Header("Vitória / Transição de Cena")]
        [Tooltip("Tempo em segundos antes de carregar a próxima cena após chegar ao destino")]
        [SerializeField]
        private readonly float delayAntesDeCarregar = 3f;

        private bool emEmergencia;
        private float timerEmergencia;
        private bool chegouNoDestino;


        void Start()
        {
            velocidadeAtual = velocidadeNormal;
            if (imagemSemaforoVermelho) imagemSemaforoVermelho.SetActive(false);
            if (speedLinesParticles) speedLinesParticles.Stop();
        }

        void Update()
        {
            // Move em direção ao ponto de parada apenas no eixo X
            if (pontoDeParada)
            {
                float direcaoX = Mathf.Sign(pontoDeParada.position.x - transform.position.x);
                transform.position += new Vector3(direcaoX * velocidadeAtual * Time.deltaTime, 0f, 0f);
            }

            // Verifica se chegou ao ponto de parada (apenas eixo X)
            if (!chegouNoDestino && pontoDeParada && Mathf.Abs(transform.position.x - pontoDeParada.position.x) < 1f)
            {
                velocidadeAtual = 0f;
                chegouNoDestino = true;
                StartCoroutine(VitoriaSequence());
            }

            // Timer do modo emergência
            if (emEmergencia)
            {
                timerEmergencia -= Time.deltaTime;
                if (timerEmergencia <= 0f)
                {
                    DesativarEmergencia();
                }
            }
        }

        // --- MÉTODOS QUE O DIALOGUE SYSTEM VAI CHAMAR ---

        /// <summary>
        /// Chamado quando o player acerta a pergunta.
        /// Ativa o modo emergência: sirene, speed lines e velocidade aumentada.
        /// </summary>
        public void AcertouPergunta()
        {
            AtivarEmergencia();
        }

        public void ErrouPergunta()
        {
            velocidadeAtual = 0f;
            if (imagemSemaforoVermelho) imagemSemaforoVermelho.SetActive(true);
            Debug.Log("Sinal Vermelho! Parando...");
        }

        public void VoltarAAndar()
        {
            velocidadeAtual = velocidadeNormal;
            if (imagemSemaforoVermelho) imagemSemaforoVermelho.SetActive(false);
        }

        public void PararTotalmente()
        {
            velocidadeAtual = 0f;
            if (emEmergencia) DesativarEmergencia();
        }

        // --- MODO EMERGÊNCIA ---

        private void AtivarEmergencia()
        {
            emEmergencia = true;
            timerEmergencia = duracaoEmergencia;
            velocidadeAtual = velocidadeEmergencia;

            // Feedback Feel (sirene, camera shake, etc.)
            emergenciaFeedback?.PlayFeedbacks();

            // Speed lines (partículas de velocidade)
            if (speedLinesParticles) speedLinesParticles.Play();

            // Esconde semáforo caso estivesse ativo
            if (imagemSemaforoVermelho) imagemSemaforoVermelho.SetActive(false);

            Debug.Log("EMERGÊNCIA! Sirene ligada, velocidade aumentada!");
        }

        private void DesativarEmergencia()
        {
            emEmergencia = false;
            velocidadeAtual = velocidadeNormal;

            // Para o feedback de emergência (sirene loop)
            emergenciaFeedback?.StopFeedbacks();
            emergenciaFimFeedback?.PlayFeedbacks();

            // Para speed lines
            if (speedLinesParticles) speedLinesParticles.Stop();

            Debug.Log("Emergência encerrada. Velocidade normal.");
        }

        // --- VITÓRIA: AMBULÂNCIA CHEGOU AO DESTINO ---

        /// <summary>
        /// Sequência de vitória quando a ambulância chega ao hospital.
        /// Replica a lógica do antigo BtnContinuar.
        /// </summary>
        private IEnumerator VitoriaSequence()
        {
            // Para a emergência se estiver ativa
            if (emEmergencia) DesativarEmergencia();

            // Atualiza o estado da quest (mesma lógica do BtnContinuar)
            QuestLog.SetQuestEntryState(QuestsEnum.AconteceAPicada, 2, QuestState.Success);
            QuestLog.SetQuestEntryState(QuestsEnum.AconteceAPicada, 3, QuestState.Active);
            DialogueManager.SendUpdateTracker();

            // Mostra alertas ao jogador
            DialogueManager.ShowAlert("Muito bem! Você conseguiu salvar o Charlinho!");
            DialogueManager.ShowAlert("Agora, fale com a atendente do hospital.");

            Debug.Log("Vitória! Aguardando para carregar cena...");

            // Aguarda antes de carregar a próxima cena
            yield return new WaitForSeconds(delayAntesDeCarregar);

            // Aguarda qualquer diálogo ativo terminar antes de trocar de cena
            while (DialogueManager.isConversationActive)
            {
                yield return new WaitForSeconds(0.5f);
            }

            // Carrega a cena com spawn point de sucesso
            new LuaFunctionsToDialogue().CarregarCena("NeighborhoodScene@aconteceAPicadaSpawnPointSucesso");
        }


        // --- REGISTRO LUA PARA DIALOGUE SYSTEM ---

        void OnEnable()
        {
            // Registra funções para serem chamadas via Lua no Dialogue System
            Lua.RegisterFunction("AmbulanciaAcertouPergunta", this,
                SymbolExtensions.GetMethodInfo(() => AcertouPergunta()));

            Lua.RegisterFunction("AmbulanciaErrouPergunta", this,
                SymbolExtensions.GetMethodInfo(() => ErrouPergunta()));

            Lua.RegisterFunction("AmbulanciaVoltarAAndar", this,
                SymbolExtensions.GetMethodInfo(() => VoltarAAndar()));

            Lua.RegisterFunction("AmbulanciaParar", this,
                SymbolExtensions.GetMethodInfo(() => PararTotalmente()));
        }

        void OnDisable()
        {
            // Desregistra funções ao desabilitar o objeto
            Lua.UnregisterFunction("AmbulanciaAcertouPergunta");
            Lua.UnregisterFunction("AmbulanciaErrouPergunta");
            Lua.UnregisterFunction("AmbulanciaVoltarAAndar");
            Lua.UnregisterFunction("AmbulanciaParar");
        }
    }
}