using System.Collections;
using PixelCrushers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.GameManager
{
    /// <summary>
    /// Tela de transição única e consistente para todas as trocas de cena do jogo.
    ///
    /// Substitui a antiga "barra de load" do MoreMountains (que parecia bug e fugia
    /// do design system) por um fade suave com o logo do jogo no centro. O overlay
    /// vive em DontDestroyOnLoad, então cobre o frame 0 da cena nova — eliminando
    /// também o "flash" do player que aparecia ao abrir cenas (ver FadeIntro).
    ///
    /// Uso:
    ///   SceneTransition.Load("PlayerHouseScene@spawn");  // troca com save/spawnpoint (SaveSystem)
    ///   SceneTransition.LoadRaw("MainMenuScene");        // troca simples (SceneManager)
    ///
    /// O logo é opcional: basta colocar um sprite chamado "SceneTransitionLogo"
    /// dentro de qualquer pasta Resources/. Sem ele, mostra só o fade.
    /// </summary>
    public class SceneTransition : MonoBehaviour
    {
        // ---- Aparência (design system) ----
        private static readonly Color BackgroundColor = new Color(0.05f, 0.06f, 0.11f, 1f); // azul-noite da marca
        private const float FadeOutDuration = 0.35f; // escurece
        private const float FadeInDuration = 0.45f;  // clareia na cena nova
        private const float MinDisplayTime = 0.6f;   // tempo mínimo do logo na tela (evita "piscar")
        private const float SettleFrames = 2;        // frames extras p/ Cinemachine/Timeline assentarem

        private const string LogoResourceName = "SceneTransitionLogo";
        private const int SortingOrder = 32760;       // acima de qualquer UI da cena

        private static SceneTransition _instance;

        private CanvasGroup _group;
        private RectTransform _logo;
        private bool _isTransitioning;

        // Handshake de revelação: enquanto _holdCount > 0, o overlay continua preto
        // (não faz fade-in). Uma cena com intro/cutscene chama Hold() no Awake e
        // Release() quando já posicionou tudo (ex.: player escondido e cutscene no ar),
        // evitando o "flash" do player no spawn antes da animação assumir.
        private static int _holdCount;
        private const float MaxHoldTime = 6f; // trava de segurança: revela mesmo sem Release()

        public static bool IsTransitioning => _instance != null && _instance._isTransitioning;

        /// <summary>Segura a tela preta após o load até o próximo Release() (ou timeout).</summary>
        public static void Hold() => _holdCount++;

        /// <summary>Libera o fade-in segurado por Hold().</summary>
        public static void Release() { if (_holdCount > 0) _holdCount--; }

        // Cria o overlay automaticamente em qualquer cena, sem precisar colocá-lo nas cenas.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            EnsureInstance();
        }

        private static SceneTransition EnsureInstance()
        {
            if (_instance != null) return _instance;

            var go = new GameObject("SceneTransition");
            DontDestroyOnLoad(go);
            _instance = go.AddComponent<SceneTransition>();
            _instance.BuildUI();
            return _instance;
        }

        private void BuildUI()
        {
            var canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.transform.SetParent(transform, false);

            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = SortingOrder;

            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;

            _group = canvasGo.AddComponent<CanvasGroup>();
            _group.alpha = 0f;
            _group.blocksRaycasts = false;
            _group.interactable = false;

            // Fundo cobrindo a tela inteira.
            var bgGo = new GameObject("Background", typeof(Image));
            bgGo.transform.SetParent(canvasGo.transform, false);
            var bg = bgGo.GetComponent<Image>();
            bg.color = BackgroundColor;
            Stretch(bg.rectTransform);

            // Logo central (opcional).
            var logoSprite = Resources.Load<Sprite>(LogoResourceName);
            if (logoSprite != null)
            {
                var logoGo = new GameObject("Logo", typeof(Image));
                logoGo.transform.SetParent(canvasGo.transform, false);
                var logoImg = logoGo.GetComponent<Image>();
                logoImg.sprite = logoSprite;
                logoImg.preserveAspect = true;
                logoImg.raycastTarget = false;
                _logo = logoImg.rectTransform;
                _logo.anchorMin = _logo.anchorMax = new Vector2(0.5f, 0.5f);
                _logo.pivot = new Vector2(0.5f, 0.5f);
                _logo.anchoredPosition = Vector2.zero;
                _logo.sizeDelta = new Vector2(logoSprite.rect.width, logoSprite.rect.height);
            }
        }

        private static void Stretch(RectTransform rt)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        // ---- API pública ----

        /// <summary>Troca de cena com SaveSystem (preserva save e respeita "Cena@spawnpoint").</summary>
        public static void Load(string sceneNameAndSpawnpoint)
        {
            if (string.IsNullOrEmpty(sceneNameAndSpawnpoint)) return;
            var t = EnsureInstance();
            if (t._isTransitioning) return;
            t.StartCoroutine(t.Run(sceneNameAndSpawnpoint, saveAware: true));
        }

        /// <summary>Troca de cena simples via SceneManager (menus, telas sem save).</summary>
        public static void LoadRaw(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName)) return;
            var t = EnsureInstance();
            if (t._isTransitioning) return;
            t.StartCoroutine(t.Run(sceneName, saveAware: false));
        }

        private IEnumerator Run(string target, bool saveAware)
        {
            _isTransitioning = true;
            _group.blocksRaycasts = true;
            _holdCount = 0; // zera; a cena de destino pode chamar Hold() no Awake durante o load

            float shownAt;

            // 1) Escurece.
            yield return Fade(0f, 1f, FadeOutDuration);
            shownAt = Time.unscaledTime;

            // 2) Carrega.
            if (saveAware)
            {
                // "Cena@spawnpoint" -> só o nome da cena para casar com o sceneLoaded.
                string targetScene = target.Split('@')[0];
                bool loaded = false;
                void OnLoaded(Scene s, LoadSceneMode m)
                {
                    if (m == LoadSceneMode.Single || s.name == targetScene) loaded = true;
                }
                SceneManager.sceneLoaded += OnLoaded;
                SaveSystem.LoadScene(target);
                while (!loaded) yield return null;
                SceneManager.sceneLoaded -= OnLoaded;
            }
            else
            {
                var op = SceneManager.LoadSceneAsync(target);
                while (op != null && !op.isDone) yield return null;
            }

            // 3) Deixa a cena nova assentar (Cinemachine/Timeline) e respeita o tempo mínimo.
            for (int i = 0; i < SettleFrames; i++) yield return null;
            float remaining = MinDisplayTime - (Time.unscaledTime - shownAt);
            if (remaining > 0f) yield return new WaitForSecondsRealtime(remaining);

            // 3b) Se a cena pediu para segurar (intro/cutscene), espera o Release() ou o timeout.
            float holdStart = Time.unscaledTime;
            while (_holdCount > 0 && (Time.unscaledTime - holdStart) < MaxHoldTime)
                yield return null;
            _holdCount = 0;

            // 4) Clareia na cena nova.
            yield return Fade(1f, 0f, FadeInDuration);

            _group.blocksRaycasts = false;
            _isTransitioning = false;
        }

        private IEnumerator Fade(float from, float to, float duration)
        {
            if (duration <= 0f)
            {
                _group.alpha = to;
                yield break;
            }

            float t = 0f;
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                _group.alpha = Mathf.Lerp(from, to, t / duration);
                yield return null;
            }
            _group.alpha = to;
        }

        private void Update()
        {
            // Pulso suave do logo enquanto a tela está visível: sinaliza "carregando"
            // de forma viva, sem barra de progresso.
            if (_logo == null || _group == null || _group.alpha <= 0.01f) return;
            float pulse = 1f + 0.04f * Mathf.Sin(Time.unscaledTime * 3f);
            _logo.localScale = new Vector3(pulse, pulse, 1f);
        }
    }
}
