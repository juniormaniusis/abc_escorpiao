using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManager;
using escorpiao.Assets.Scripts.Enums;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class NovoJogoButton : BaseButton
{
    public override void OnClick()
    {
        base.OnClick();
        // Novo jogo = início frio (sem save/spawnpoint), igual ao load simples original.
        SceneTransition.LoadRaw(ScenesEnum.Neighbourhood);
    }
}
