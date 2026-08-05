using UnityEngine;
using Assets.Scripts.Player;
using MoreMountains.Feedbacks;
using PixelCrushers.DialogueSystem;
using Assets.Scripts.Enums;
using System.Collections;
using static PixelCrushers.DialogueSystem.Usable;

public class PilhaDeLixo : MonoBehaviour
{
    [Header("Configurações de coleta")]
    [SerializeField] private float tempoParaColetar = 2f;
    [SerializeField] private float distanciaParaColetar = 5f;

    [Header("Feedbacks")]
    [SerializeField] private MMF_Player feedbackColetando;
    [SerializeField] private MMF_Player feedbackColetado;



    private PlayerStatus playerStatus;
    private Coroutine coletaCoroutine;
    private bool coletado;

    [Header("Usable")]
    [SerializeField] private string overrideName = "Pilha de Lixo";
    [SerializeField] private string overrideUseMessage = "Coletar Pilha de Lixo";

    private Usable usable;

    private void CompletaColeta()
    {
        FinalizarColeta();
        var bar = ProgressBarController.Instance.progressBar;
        bar.OnBarMovementIncreasingStop.RemoveListener(CompletaColeta);
        bar.gameObject.SetActive(false);
        coletaCoroutine = null;

    }

    private void Start()
    {
        playerStatus = PlayerStatus.Instance;
        // adiciona o Usable ao gameobject

        if (gameObject.GetComponent<Usable>() == null)
        {
            usable = gameObject.AddComponent<Usable>();

            usable.overrideName = overrideName;

            usable.maxUseDistance = distanciaParaColetar;


            var colliderTrigger = gameObject.AddComponent<BoxCollider>();
            colliderTrigger.isTrigger = true;
            colliderTrigger.size = new Vector3(distanciaParaColetar, 1, distanciaParaColetar);

            usable.events ??= new UsableEvents();

            usable.events.onUse.AddListener(MensagemColeta);
        }

    }

    private void MensagemColeta()
    {
        if (!PlayerStatus.Instance.PodeFazerAcao(AcoesPossiveisEnum.Limpar))
        {
            DialogueManager.Bark("BARK_ENTULHO", PlayerStatus.Instance.transform);
        }
    }

    private void Update()
    {
        if (PodeColetar())
        {
            if (coletaCoroutine == null)
                IniciarColeta();
        }
        else if (coletaCoroutine != null)
        {
            PararColeta();
        }

    }

    private bool PodeColetar()
    {
        return !coletado
            && playerStatus.PodeFazerAcao(AcoesPossiveisEnum.Limpar)
            && playerStatus.QuerExecutarAcao
            && Vector3.Distance(transform.position, playerStatus.transform.position) <= distanciaParaColetar;
    }

    private void IniciarColeta()
    {
        playerStatus.ActionStatus = AcoesPossiveisEnum.Limpar;
        feedbackColetando?.PlayFeedbacks();
        var bar = ProgressBarController.Instance.progressBar;
        bar.gameObject.SetActive(true);
        bar.UpdateBar01(0f);
        bar.OnBarMovementIncreasingStop.AddListener(CompletaColeta); // agora só esta instância
        coletaCoroutine = StartCoroutine(ColetaCoroutine());
    }

    private IEnumerator ColetaCoroutine()
    {
        float elapsed = 0f;
        var bar = ProgressBarController.Instance;

        while (elapsed < tempoParaColetar)
        {
            if (!PodeColetar())
                yield break;

            elapsed += Time.deltaTime;
            bar.AtualizarProgresso(Mathf.Clamp01(elapsed / tempoParaColetar));
            yield return null;
        }

        // concluiu
        // bar.AtualizarProgresso(1f);
        feedbackColetando?.StopFeedbacks();
        // dispara evento de conclusão
    }

    private void PararColeta()
    {
        StopCoroutine(coletaCoroutine);
        coletaCoroutine = null;
        feedbackColetando?.StopFeedbacks();
        var bar = ProgressBarController.Instance.progressBar;
        bar.OnBarMovementIncreasingStop.RemoveListener(CompletaColeta);
        ResetEstado();
    }

    private void FinalizarColeta()
    {
        coletado = true;
        playerStatus.ActionStatus = AcoesPossiveisEnum.SemAcao;
        feedbackColetado?.PlayFeedbacks();
        IncrementaProgressoQuest();

        if (QuestCompleta())
        {
            QuestLog.SetQuestEntryState(QuestsEnum.LimparQuintal, 3, QuestState.Success);
            QuestLog.SetQuestState(QuestsEnum.LimparQuintal, QuestState.ReturnToNPC);
            DialogueManager.ShowAlert("Muito bem! Volte e fale com o André.");
        }
    }

    public bool QuestCompleta()
    {
        int qtdPilhasColetadas = DialogueLua.GetVariable(QuestsEnum.QtdPilhasDeLixoColetadas).AsInt;
        int qtdPilhasParaColetar = DialogueLua.GetVariable(QuestsEnum.QtdPilhasDeLixParaColetar).AsInt;

        return qtdPilhasColetadas >= qtdPilhasParaColetar;

    }

    private void ResetEstado()
    {
        playerStatus.ActionStatus = AcoesPossiveisEnum.SemAcao;
        ProgressBarController.Instance?.AtualizarProgresso(0f);
    }

    private void IncrementaProgressoQuest()
    {
        int qtd = DialogueLua.GetVariable(QuestsEnum.QtdPilhasDeLixoColetadas).AsInt;
        DialogueLua.SetVariable(QuestsEnum.QtdPilhasDeLixoColetadas, ++qtd);
        DialogueManager.SendUpdateTracker();
        PontuacaoController.Instance.AdicionarPontos(5);
    }
}