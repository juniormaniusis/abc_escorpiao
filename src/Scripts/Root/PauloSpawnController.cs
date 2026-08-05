using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement; // Adicione este namespace

public class PauloSpawnController : MonoBehaviour
{
    public GameObject personagem;
    public string nomeDaCenaParaSpawn;
    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
        OnSceneChanged(SceneManager.GetActiveScene(), SceneManager.GetActiveScene());
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    void OnSceneChanged(Scene current, Scene next)
    {
        // Verifica se a nova cena é a cena desejada para o spawn
        if (next.name == nomeDaCenaParaSpawn && QuestLog.GetQuestState(QuestsEnum.GuardarBrinquedos) == QuestState.Success)
        {
            // Verifica se o personagem já está ativo
            if (personagem != null && !personagem.activeInHierarchy)
            {
                // Ativa o personagem
                personagem.SetActive(true);

            }
            else
            {
                Debug.LogWarning("O personagem já está ativo ou não foi definido.");
            }
        }
        else
        {
            // Desativa o personagem se a cena não for a desejada
            if (personagem != null && personagem.activeInHierarchy)
            {
                Debug.Log($"Desativando personagem {personagem.name} porque a cena atual é {next.name} e não é a cena de spawn.");
                personagem.SetActive(false);
            }
        }
    }

}
