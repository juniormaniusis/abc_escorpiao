using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;

public class CollectibleProgressBarManager : MonoBehaviour
{
    public MMProgressBar progressBar;      // Referência à barra de progresso única
    public Camera mainCamera;              // Referência à câmera principal
    public Transform player;               // Referência do player
    public float activationDistance = 5f;  // Distância máxima para ativar a barra
    public Vector2 offset;                 // Offset da barra em relação ao coletável

    private Canvas progressCanvas;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Obter o canvas que contém a barra
        progressCanvas = progressBar.GetComponentInParent<Canvas>();

        // Inicia desativando a barra
        progressBar.gameObject.SetActive(false);
    }

    void Update()
    {
        // Encontra todos os coletáveis com a tag ITEM_COLETAVEL
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("ITEM_COLETAVEL");
        GameObject closestCollectible = null;
        float closestDistance = Mathf.Infinity;

        // Procura o coletável mais próximo do player dentro da distância de ativação
        foreach (GameObject item in collectibles)
        {
            float distance = Vector3.Distance(player.position, item.transform.position);
            if (distance < closestDistance && distance <= activationDistance)
            {
                closestDistance = distance;
                closestCollectible = item;
            }
        }

        if (closestCollectible != null)
        {
            // Ativa a barra e atualiza sua posição acima do coletável
            if (!progressBar.gameObject.activeSelf)
                progressBar.gameObject.SetActive(true);

            UpdateProgressBarPosition(closestCollectible.transform);
        }
        else
        {
            // Desativa a barra se nenhum coletável estiver próximo
            if (progressBar.gameObject.activeSelf)
                progressBar.gameObject.SetActive(false);
        }
    }

    private void UpdateProgressBarPosition(Transform target)
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position);
        if (progressCanvas != null)
        {
            RectTransform canvasRect = progressCanvas.GetComponent<RectTransform>();
            RectTransform progressBarRect = progressBar.GetComponent<RectTransform>();

            // Usa a câmera se o canvas não for do tipo ScreenSpaceOverlay, senão null
            Camera uiCamera = progressCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera;
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
}