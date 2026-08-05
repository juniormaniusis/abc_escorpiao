using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Quests;
using UnityEngine;

namespace Assets.Scripts.Quest
{
    public class LimparLixoQuestItemSpawner : IQuestItemSpawner
    {
        private GameObject[] _lixos;
        public LimparLixoQuestItemSpawner()
        {
            _lixos = GameObject.FindObjectsOfType<PilhaDeLixo>(includeInactive: true)
                            .Select(x => x.gameObject)
                            .ToArray();

        }
        public void SpawnItens()
        {
            foreach (var lixo in _lixos)
            {
                lixo.SetActive(true);
            }
        }
    }
}