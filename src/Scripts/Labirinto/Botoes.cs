using Assets.Scripts.Enums;
using Assets.Scripts.GameManager;
using Assets.Scripts.Player;
using PixelCrushers.DialogueSystem;
using PixelCrushers.Wrappers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botoes : MonoBehaviour
{
    string AconteceAPicadaSpawnPointFalha = "aconteceAPicadaSpawnPointFalha";
    string AconteceAPicadaSpawnPointSucesso = "aconteceAPicadaSpawnPointSucesso";
    string Scene = "NeighborhoodScene";

    public void TentarNovamente()
    {
        ReativarPlayer();
        SceneTransition.Load(Scene + "@" + AconteceAPicadaSpawnPointFalha);
        DialogueManager.ShowAlert("Você não chegou a tempo ao hospital! tente novamente.");
        Debug.Log("Tentando novamente...");
    }

    public void BtnContinuar()
    {
        ReativarPlayer();
        QuestLog.SetQuestEntryState(QuestsEnum.AconteceAPicada, 2, QuestState.Success);
        QuestLog.SetQuestEntryState(QuestsEnum.AconteceAPicada, 3, QuestState.Active);
        SceneTransition.Load(Scene + "@" + AconteceAPicadaSpawnPointSucesso);

        DialogueManager.ShowAlert("Muito bem! Você conseguiu salvar o Charlinho!");
        DialogueManager.ShowAlert("Agora, fale com a atendente do hospital.");
        DialogueManager.SendUpdateTracker();
        Debug.Log("Continuando...");
    }

    /// <summary>
    /// Reativa o jogador persistente (DontDestroyOnLoad) que foi desativado ao entrar no labirinto.
    /// </summary>
    private void ReativarPlayer()
    {
        var playerStatus = PlayerStatus.Instance;
        if (playerStatus != null)
        {
            playerStatus.gameObject.SetActive(true);
        }
    }
}
