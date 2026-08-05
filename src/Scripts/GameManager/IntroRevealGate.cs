using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.GameManager
{
    /// <summary>
    /// Coloque em uma cena que abre com cutscene/intro (ex.: NeighborhoodScene "Chegando
    /// na Cidade"). Segura a tela de transição preta (<see cref="SceneTransition"/>) após o
    /// load até a cutscene assumir o controle — isto é, até o player de gameplay ser
    /// desativado e a animação entrar em cena. Isso elimina o "flash" em que o player
    /// aparece no spawn por uma fração de segundo antes de ser teleportado para o início
    /// da animação.
    ///
    /// Se a cutscene nunca assumir (ou demorar demais), libera por timeout — e o
    /// <see cref="SceneTransition"/> ainda tem sua própria trava de segurança.
    /// </summary>
    public class IntroRevealGate : MonoBehaviour
    {
        [Tooltip("Tempo máximo (s) segurando o preto antes de revelar de qualquer forma.")]
        [SerializeField] private float maxWait = 5f;

        private bool _held;
        private bool _released;
        private bool _seenPlayerActive;
        private float _t0;

        private void Awake()
        {
            SceneTransition.Hold();
            _held = true;
            _t0 = Time.unscaledTime;
        }

        private void Update()
        {
            if (_released) return;

            // Só procura o player ATIVO (FindObjectOfType ignora inativos).
            var playerActive = Object.FindObjectOfType<PlayerInputHandler>() != null;
            if (playerActive) _seenPlayerActive = true;

            // Revela quando a cutscene escondeu o player (player de gameplay desativado)
            // ou quando o tempo de segurança estourou.
            bool cutsceneTookOver = _seenPlayerActive && !playerActive;
            bool timedOut = (Time.unscaledTime - _t0) >= maxWait;
            if (cutsceneTookOver || timedOut) ReleaseNow();
        }

        private void ReleaseNow()
        {
            _released = true;
            if (_held)
            {
                SceneTransition.Release();
                _held = false;
            }
            enabled = false;
        }

        private void OnDestroy()
        {
            if (_held && !_released) SceneTransition.Release();
        }
    }
}
