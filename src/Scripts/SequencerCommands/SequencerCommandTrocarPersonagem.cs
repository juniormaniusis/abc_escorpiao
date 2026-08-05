using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using DG.Tweening;
using Assets.Scripts.Player;


namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandTrocarPersonagem : SequencerCommand
    {
        public void Awake()
        {


            var player = PlayerStatus.Instance;
            var x = GetParameter(0, "Andre");
            player.TrocarPersonagem(x);
            Stop();
        }
    }
}