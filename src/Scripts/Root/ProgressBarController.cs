using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public static ProgressBarController Instance { get; private set; }
    public MMProgressBar progressBar;          // Barra de progresso única
    public Camera mainCamera;                  // Câmera principal
    public Vector2 offset = new Vector2(0, 50);  // Offset para posicionar a barra acima do coletável
    public string collectibleTag = "ITEM_COLETAVEL";
    public GameObject player;                  // Referência do player

    // método que verifica se mainCamera é null, se for, busca a câmera principal, 
    // se nao encontrar, busca novamente em 1 segundo
    private void CheckMainCamera()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Invoke(nameof(CheckMainCamera), 1f); // Tenta novamente em 1 segundo
            }
        }
    } 

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // opcional: persistir entre cenas
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (progressBar == null)
            progressBar = GetComponent<MMProgressBar>();

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        // Inicializa com a barra desativada:
        progressBar.gameObject.SetActive(false);
    }

    void Update()
    {
        // Busca todos os coletáveis ativos com a tag definida
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag(collectibleTag);
        GameObject nearestCollectible = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject collectible in collectibles)
        {
            if (!collectible.activeSelf) continue;

            float distance = Vector3.Distance(player.transform.position, collectible.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestCollectible = collectible;
            }
        }

        if (nearestCollectible != null)
        {
            if (!progressBar.gameObject.activeSelf)
                progressBar.gameObject.SetActive(true);

            UpdateProgressBarPosition(nearestCollectible.transform);
        }
        else if (progressBar.gameObject.activeSelf)
        {
            progressBar.gameObject.SetActive(false);
        }
    }

    private void UpdateProgressBarPosition(Transform collectibleTransform)
    {
        // Converte a posição do coletável para coordenadas de tela
        Vector3 screenPosition = MainCamera.WorldToScreenPoint(collectibleTransform.position);

        // Posiciona a barra com base no canvas pai
        Canvas canvas = progressBar.GetComponentInParent<Canvas>();
        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            RectTransform progressBarRect = progressBar.GetComponent<RectTransform>();

            // Usa a câmera se o renderMode não for ScreenSpaceOverlay
            Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : MainCamera;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPosition, uiCamera, out Vector2 localPoint))
            {
                progressBarRect.localPosition = localPoint + offset;
            }
        }
        else
        {
            progressBar.transform.position = screenPosition;
        }
    }

    public void ProgressoConcluido()
    {
        if (progressBar != null)
        {
            
            progressBar.gameObject.SetActive(false);
        }
    }

    public void AtualizarProgresso(float valorNormalizado)
    {

        if (progressBar != null)
        {
            progressBar.UpdateBar01(valorNormalizado);
        }
    }

    public Camera MainCamera
    {
        get
        {
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
                if (mainCamera == null)
                {
                    Debug.LogWarning("Main Camera not found. Retrying in 1 second.");
                    Invoke(nameof(CheckMainCamera), 1f);
                }
            }
            return mainCamera;
        }
    }
}