using System;
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Assets.Scripts.Player;

[Serializable]
public class Trilha
{
    public string trilhaPercorridaVariable;
    [SerializeField, Tooltip("A origem da trilha, quando não o PlayerStatus.")]
    private Transform _origem;
    public Transform origem
    {
        get
        {
            if (_origem == null && origemPlayer)
            {
                Debug.LogWarning("Origem não definida, usando PlayerStatus como origem.");
                _origem = origemPlayer ? PlayerStatus.Instance.transform : null;
            }
            return _origem;
        }
        set => _origem = value;
    }
    public Transform destino;
    public Condition condicao;

    [Tooltip("Se a origem é a partir do player, ou se é um ponto fixo no mundo.")]
    public bool origemPlayer = true;

    public bool PodeGerar(Transform interactor)
    {
        return condicao.IsTrue(interactor);
    }

    public void MarcarComoPercorrida()
    {
        if (trilhaPercorridaVariable != null)
        {
            DialogueLua.SetVariable(trilhaPercorridaVariable, true);
        }
    }
    public bool Percorrida
    {
        get
        {
            return DialogueLua.GetVariable(trilhaPercorridaVariable, false);
        }
    }
}
