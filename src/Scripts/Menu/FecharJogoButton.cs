using System.Collections;
using System.Collections.Generic;
using escorpiao.Assets.Scripts.Enums;
using PixelCrushers;
using UnityEngine;

public class FecharJogoButton : BaseButton
{
    public override void OnClick()
    {
        base.OnClick();
        Application.Quit();
    }
}
