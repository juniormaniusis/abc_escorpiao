using System.Collections.Generic;
using PixelCrushers.DialogueSystem.Wrappers;
using UnityEngine;

namespace Assets.Scripts.Player
{
    [RequireComponent(typeof(PlayerStatus))]
    public class PersonagemManager : MonoBehaviour
    {

        public PersonagemEnum PersonagemAtual { get; private set; } = PersonagemEnum.Andre;
        private readonly Dictionary<PersonagemEnum, PersonagemConfig> _personagens = new()
        {
            { PersonagemEnum.Andre, new PersonagemConfig("Andre", "Player") },
            { PersonagemEnum.Pai, new PersonagemConfig("Pai", "Player") },
            { PersonagemEnum.AgenteZoonozes, new PersonagemConfig("Agente", "Player") }
        };
        public void TrocarPersonagem(PersonagemEnum personagem)
        {
            if (PersonagemAtual == personagem) return;
            PersonagemAtual = personagem;
            var personagemConfig = _personagens[personagem];
            var dialogueActors = GetComponentsInChildren<DialogueActor>(includeInactive: true);

            foreach (var actor in dialogueActors)
            {
                if (actor.name == personagemConfig.Actor)
                {
                    actor.gameObject.SetActive(true);
                }
                else
                {
                    actor.gameObject.SetActive(false);
                }
            }
        }
    }
}