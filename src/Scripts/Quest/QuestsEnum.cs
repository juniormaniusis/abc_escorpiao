using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assets.Scripts.Enums
{
    using QuestName = System.String;
    using QuestVariableName = System.String;
    public class QuestsEnum
    {
        #region Introducao

        public const QuestName Introducao = "Introducao";
        #endregion


        #region Quest1 - Guardar Brinquedos

        public const QuestName GuardarBrinquedos = "Quest1";
        public const QuestVariableName QtdItensGuardadosNoArmario = "Quest1.TotalToysOnShelf";

        #endregion

        #region Quest2 - Limpar Quintal
        public const QuestName LimparLixo = "Quest2.2";
        public const QuestName LimparQuintal = "Quest2";
        public const QuestVariableName QtdPilhasDeLixParaColetar = "Quest2_2.QtdPilhasEntulho";
        public const QuestVariableName QtdPilhasDeLixoColetadas = "Quest2_2.QtdPilhasEntulhoLimpas";
        #endregion

        #region Quest3 - Acontece a Picada
        public const QuestName AconteceAPicada = "Quest3";
        #endregion

    }
}