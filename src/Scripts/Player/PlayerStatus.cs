using System;
using Assets.Scripts.Enums;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    public class PlayerStatus : MonoBehaviour
    {
        public static PlayerStatus Instance { get; private set; }
        // -- Controllers/Componentes
        public PlayerHandController MaoController { get; private set; }
        public GameObject ItemCarregado => MaoController.Item;
        public Pontuacao Pontuacao => PontuacaoController.Instance?.Pontuacao;
        public PlayerMovement MovimentoController { get; private set; }
        public PontuacaoController PontuacaoController => PontuacaoController.Instance;
        public PlayerCameraController CameraController { get; private set; }
        public PersonagemManager PersonagemManager { get; private set; }
        public AnimacoesController AnimacoesController { get; private set; }

        // -- Estados
        public bool EstaCarregandoItem => MaoController.Ocupada;
        public bool EstaMovendo => MovimentoController?.IsMoving() ?? false;
        public bool QuerExecutarAcao { get; set; } = false;
        public AcoesPossiveisEnum ActionStatus { get; set; } = AcoesPossiveisEnum.SemAcao;

        public bool PodeFazerAcao(AcoesPossiveisEnum acao)
        {
            return acao switch
            {
                AcoesPossiveisEnum.Limpar => !EstaMovendo && !EstaCarregandoItem && PersonagemManager.PersonagemAtual == PersonagemEnum.Pai,
                _ => false,
            };
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                CameraController = GetComponent<PlayerCameraController>();
                MovimentoController = GetComponent<PlayerMovement>();
                MaoController = GetComponent<PlayerHandController>();
                // PontuacaoController = GetComponent<PontuacaoController>();
                PersonagemManager = GetComponent<PersonagemManager>();
                AnimacoesController = GetComponent<AnimacoesController>();

                //TODO: Remover depois de testar
                // Desativado: com esta chamada o jogo começa no meio da quest 2.
                // Comentada para o jogo iniciar no estado inicial, sem nenhuma
                // quest pré-definida. Reative para voltar a testar um trecho
                // específico (ajuste os estados dentro do método).
                // SetQuestStatusForDebug();

            }
            else
            {
                Debug.Log("PlayerStatus instância destruída.");
                Destroy(gameObject);
            }
        }
        // TODO: Remover depois de testar
        private void SetQuestStatusForDebug()
        {
            // Sound Effect by <a href="https://pixabay.com/users/dragon-studio-38165424/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=402324">DRAGON-STUDIO</a> from <a href="https://pixabay.com//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=402324">Pixabay</a>
            //Introducao
            QuestLog.SetQuestState(QuestsEnum.Introducao, QuestState.Success);


            // // guardar brinquedos

            QuestLog.SetQuestState(QuestsEnum.GuardarBrinquedos, QuestState.Unassigned);
            QuestLog.SetQuestState(QuestsEnum.GuardarBrinquedos, QuestState.Active);
            QuestLog.SetQuestEntryState(QuestsEnum.GuardarBrinquedos, 1, QuestState.Active);
            QuestLog.SetQuestEntryState(QuestsEnum.GuardarBrinquedos, 1, QuestState.Success);
            QuestLog.SetQuestEntryState(QuestsEnum.GuardarBrinquedos, 2, QuestState.Active);
            QuestLog.SetQuestEntryState(QuestsEnum.GuardarBrinquedos, 2, QuestState.Success);
            QuestLog.SetQuestState(QuestsEnum.GuardarBrinquedos, QuestState.ReturnToNPC);
            QuestLog.SetQuestState(QuestsEnum.GuardarBrinquedos, QuestState.Success);

            // // quest2
            QuestLog.SetQuestState(QuestsEnum.LimparQuintal, QuestState.Active);

            // // falar com o vizinho, Paulo
            QuestLog.SetQuestEntryState(QuestsEnum.LimparQuintal, 1, QuestState.Active);
            QuestLog.SetQuestEntryState(QuestsEnum.LimparQuintal, 1, QuestState.Success);

            // // falar com o Pai para limpar o quintal (conversa "Quest2: Limpar lixo do quintal",
            // // no NPC "Pai do André"): é ela que ativa o AndreClone e troca o jogador para o Pai
            QuestLog.SetQuestEntryState(QuestsEnum.LimparQuintal, 2, QuestState.Active);
            // QuestLog.SetQuestEntryState(QuestsEnum.LimparQuintal, 2, QuestState.Success);

            // // limpar o quintal: a própria conversa acima põe a entry 3 em "active";
            // // entry 3 em "success" habilita "Sim filho, acabei de limpar o quintal!"
            // // -> "Vou chamar o Charlinho para brincar"
            // QuestLog.SetQuestEntryState(QuestsEnum.LimparQuintal, 3, QuestState.Active);
            // QuestLog.SetQuestEntryState(QuestsEnum.LimparQuintal, 3, QuestState.Success);
            // QuestLog.SetQuestState(QuestsEnum.LimparQuintal, QuestState.Success);


            // // acontece a picada (quest 3)
            // QuestLog.SetQuestState(QuestsEnum.AconteceAPicada, QuestState.Active);

            // // chamar o charlinho p/ brincar
            // QuestLog.SetQuestEntryState(QuestsEnum.AconteceAPicada, 1, QuestState.Success);
            // // ir até o hospital
            // QuestLog.SetQuestEntryState(QuestsEnum.AconteceAPicada, 2, QuestState.Success);
            // // falar com a atendente do hospital
            // QuestLog.SetQuestEntryState(QuestsEnum.AconteceAPicada, 3, QuestState.Active);

            // // jogador começa como André; a conversa com o Pai faz a troca para o Pai
            // TrocarPersonagem(PersonagemEnum.Pai);

            DialogueManager.SendUpdateTracker();
        }

        public void TrocarPersonagem(PersonagemEnum personagem)
        {
            PersonagemManager.TrocarPersonagem(personagem);
            MaoController.RedefinirPosicao();
            MaoController.Soltar();
            AnimacoesController.TrocarPersonagem(personagem);
        }

        public void TrocarPersonagem(string personagem)
        {
            switch (personagem)
            {
                case "Andre":
                    TrocarPersonagem(PersonagemEnum.Andre);
                    break;
                case "Pai":
                    TrocarPersonagem(PersonagemEnum.Pai);
                    break;
                case "AgenteZoonozes":
                    TrocarPersonagem(PersonagemEnum.AgenteZoonozes);
                    break;
                default:
                    Debug.LogWarning($"Personagem desconhecido: {personagem}");
                    break;
            }
        }

        public void Update()
        {
            return;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TrocarPersonagem(PersonagemEnum.Andre);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                TrocarPersonagem(PersonagemEnum.Pai);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                TrocarPersonagem(PersonagemEnum.AgenteZoonozes);
            }
        }

        internal void Comemorar()
        {
            AnimacoesController.Comemorar();
        }
    }
}