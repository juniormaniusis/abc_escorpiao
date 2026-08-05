using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Assets.Scripts.Player
{
    public class Pontuacao
    {
        public UnityEvent<long> OnPontuacaoAlterada = new();
        public long Valor { get; private set; }
        public Pontuacao(long valorInicial)
        {
            Valor = valorInicial;
        }
        internal void Adicionar(long valor)
        {
            Valor += valor;
            OnPontuacaoAlterada?.Invoke(Valor);
        }
        internal void Remover(long valor)
        {
            Valor -= valor;
            OnPontuacaoAlterada?.Invoke(Valor);
        }

        internal void DefinirPontuacaoSalva(long valor)
        {
            Valor = valor;
            OnPontuacaoAlterada?.Invoke(Valor);
        }


    }
}