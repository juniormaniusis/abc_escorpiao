using Assets.Scripts.GameManager;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Permite ao jogador pular a cutscene (Timeline + Dialogue System) 
/// com confirmação em dois toques para evitar pulo acidental.
/// 
/// Setup:
/// 1. Adicione este componente a qualquer GameObject na cena da cutscene
/// 2. Arraste a referência do PlayableDirector no Inspector
/// 3. Configure o nome da cena destino (sceneName)
/// 4. (Opcional) Crie um Text UI para o prompt e arraste no campo skipPromptText
/// </summary>
public class SkipCutscene : MonoBehaviour
{
    [Header("Referências")]
    [Tooltip("PlayableDirector da Timeline da cutscene")]
    [SerializeField] private PlayableDirector director;

    [Header("Configurações")]
    [Tooltip("Nome da cena a carregar ao pular a cutscene")]
    [SerializeField] private string sceneName = "LabirintoV2";

    [Tooltip("Tecla para pular a cutscene")]
    [SerializeField] private KeyCode skipKey = KeyCode.Escape;

    [Tooltip("Tempo (em segundos) que o jogador tem para confirmar o pulo")]
    [SerializeField] private float confirmTimeout = 2f;

    [Tooltip("Se o jogador persistente deve estar ativo na cena destino")]
    [SerializeField] private bool playerAtivoNaCenaDestino = false;

    [Header("UI (Opcional)")]
    [Tooltip("Texto UI para mostrar o prompt de confirmação. Se vazio, será criado automaticamente.")]
    [SerializeField] private Text skipPromptText;

    private bool waitingForConfirmation = false;
    private float confirmTimer = 0f;
    private bool isSkipping = false;

    // UI criada automaticamente caso não tenha sido atribuída
    private GameObject autoCreatedUI;

    private void Start()
    {
        if (director == null)
        {
            director = GetComponent<PlayableDirector>();
        }

        // Se não há Text atribuído, cria um automaticamente
        if (skipPromptText == null)
        {
            CreateAutoUI();
        }

        HidePrompt();
    }

    private void Update()
    {
        if (isSkipping) return;

        if (Input.GetKeyDown(skipKey))
        {
            if (waitingForConfirmation)
            {
                // Segundo toque: confirmar o pulo
                SkipNow();
            }
            else
            {
                // Primeiro toque: mostrar prompt de confirmação
                StartConfirmation();
            }
        }

        // Contagem regressiva do timeout de confirmação
        if (waitingForConfirmation)
        {
            confirmTimer -= Time.unscaledDeltaTime;
            if (confirmTimer <= 0f)
            {
                CancelConfirmation();
            }
        }
    }

    private void StartConfirmation()
    {
        waitingForConfirmation = true;
        confirmTimer = confirmTimeout;
        ShowPrompt();
    }

    private void CancelConfirmation()
    {
        waitingForConfirmation = false;
        HidePrompt();
    }

    private void SkipNow()
    {
        isSkipping = true;
        HidePrompt();

        // 1. Para a Timeline
        if (director != null && director.state == PlayState.Playing)
        {
            director.Stop();
        }

        // 2. Para o diálogo ativo (Dialogue System for Unity)
        if (DialogueManager.isConversationActive)
        {
            DialogueManager.StopConversation();
        }

        // 3. Controla o estado do player persistente e carrega a cena
        if (!playerAtivoNaCenaDestino)
        {
            var playerStatus = Assets.Scripts.Player.PlayerStatus.Instance;
            if (playerStatus != null)
            {
                playerStatus.gameObject.SetActive(false);
            }

            var vcam = GameObject.FindGameObjectWithTag("VCAM_PLAYER");
            if (vcam != null)
            {
                vcam.SetActive(false);
            }
        }

        SceneTransition.LoadRaw(sceneName);
    }

    #region UI

    private void ShowPrompt()
    {
        if (skipPromptText != null)
        {
            string keyName = skipKey.ToString();
            skipPromptText.text = $"Pressione [{keyName}] novamente para pular";
            skipPromptText.gameObject.SetActive(true);
        }
    }

    private void HidePrompt()
    {
        if (skipPromptText != null)
        {
            skipPromptText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Cria um Canvas + Text simples para o prompt de skip,
    /// caso o usuário não tenha atribuído um no Inspector.
    /// </summary>
    private void CreateAutoUI()
    {
        // Canvas
        autoCreatedUI = new GameObject("SkipCutsceneCanvas");
        var canvas = autoCreatedUI.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // Garante que fica na frente
        autoCreatedUI.AddComponent<CanvasScaler>();
        autoCreatedUI.AddComponent<GraphicRaycaster>();

        // Text
        var textObj = new GameObject("SkipPromptText");
        textObj.transform.SetParent(autoCreatedUI.transform, false);

        skipPromptText = textObj.AddComponent<Text>();
        skipPromptText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        skipPromptText.fontSize = 24;
        skipPromptText.color = Color.white;
        skipPromptText.alignment = TextAnchor.LowerCenter;

        // Sombra para legibilidade
        var shadow = textObj.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.8f);
        shadow.effectDistance = new Vector2(1, -1);

        // Outline para legibilidade extra
        var outline = textObj.AddComponent<UnityEngine.UI.Outline>();
        outline.effectColor = new Color(0, 0, 0, 0.6f);
        outline.effectDistance = new Vector2(1, -1);

        // Posicionar na parte inferior da tela
        var rectTransform = skipPromptText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 0.1f);
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }

    #endregion

    private void OnDestroy()
    {
        if (autoCreatedUI != null)
        {
            Destroy(autoCreatedUI);
        }
    }
}
