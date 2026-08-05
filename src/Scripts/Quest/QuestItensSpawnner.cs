using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using Assets.Scripts.Quest;
using UnityEngine;
namespace Assets.Scripts.Quests
{
    public class QuestItensSpawnner : MonoBehaviour
    {
        public void HandleQuestEvent(string questName)
        {
            Debug.Log($"HandleQuestEvent: {questName}");
            var questItemSpawner = GetQuestItemSpawner(questName);

            if (questItemSpawner == null)
            {
                Debug.LogWarning($"No IQuestItemSpawner found for quest: {questName}");
                return;
            }
            questItemSpawner.SpawnItens();
        }

        private IQuestItemSpawner GetQuestItemSpawner(string questName) => questName switch
        {
            QuestsEnum.GuardarBrinquedos => new GuardarBrinquedosQuestItemSpawner(),
            QuestsEnum.LimparLixo => new LimparLixoQuestItemSpawner(),
            QuestsEnum.LimparQuintal => new FaleComVizinhoQuestItemSpawner(),
            _ => null
        };

        class FaleComVizinhoQuestItemSpawner : IQuestItemSpawner
        {
            public void SpawnItens()
            {
                // ativa o personagem do Pai do André.
            }
        }


    }


}