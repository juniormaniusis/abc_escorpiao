using System;
using PixelCrushers;
using UnityEngine;
namespace Assets.Scripts.Player
{
    [AddComponentMenu("")] // Use wrapper instead.
    public class PlayerScoreSaver : Saver
    {
        [Serializable]
        public class PlayerScoreData
        {
            public long pontuacao;
        }

        // private PlayerStatus playerStatus;

        public override void Awake()
        {
            base.Awake();
            /*
            playerStatus = PlayerStatus.Instance;
            if (playerStatus == null)
            {
                Debug.LogError("PlayerStatus instance not found.");
            }
            */
        }

        public override string RecordData()
        {
            if (PontuacaoController.Instance == null) return string.Empty;
            var data = new PlayerScoreData
            {
                pontuacao = PontuacaoController.Instance.Pontuacao.Valor
            };
            return SaveSystem.Serialize(data);
        }

        public override void ApplyData(string s)
        {
            if (PontuacaoController.Instance == null) return;
            if (!string.IsNullOrEmpty(s))
            {
                var data = SaveSystem.Deserialize<PlayerScoreData>(s);
                if (data != null)
                {
                    PontuacaoController.Instance.DefinirPontuacao(data.pontuacao);
                }
            }
        }
    }
}