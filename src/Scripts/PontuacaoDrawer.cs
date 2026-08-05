using Assets.Scripts.Player;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PontuacaoDrawer : MonoBehaviour
{
    private static PontuacaoDrawer instance;
    // private PlayerStatus playerStatus;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private MMF_Player AtualizarPontuacaoFeedback;
    [SerializeField] private MMF_Player AtualizarPontuacaoFeedbackMultiplosDe10;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();
    }

    void Initialize()
    {
        if (PontuacaoController.Instance != null && PontuacaoController.Instance.Pontuacao != null)
        {
            PontuacaoController.Instance.Pontuacao.OnPontuacaoAlterada.RemoveListener(HandlePontuacaoChanged);
            PontuacaoController.Instance.Pontuacao.OnPontuacaoAlterada.AddListener(HandlePontuacaoChanged);
            HandlePontuacaoChanged(0);
        }
    }

    private void HandlePontuacaoChanged(long pontuacao)
    {
        try
        {
            if (text == null)
            {
                Debug.LogError("Objeto com a tag QTD_MACAS não encontrado!!! 123==.");

                // procura o gameobject com a tag QTD_MACAS
                GameObject qtdMacasObject = GameObject.FindGameObjectWithTag("QTD_MACAS");
                if (qtdMacasObject != null)
                {
                    text = qtdMacasObject.GetComponent<TMPro.TextMeshProUGUI>();
                    if (text == null)
                    {
                        Debug.LogError("Componente TextMeshProUGUI não encontrado no objeto com a tag QTD_MACAS.");
                        return;
                    }
                }
                else
                {
                    Debug.LogError("Objeto com a tag QTD_MACAS não encontrado.");
                    return;
                }
            }
            if (PontuacaoController.Instance?.Pontuacao == null) return;
            text.text = PontuacaoController.Instance.Pontuacao.Valor.ToString();
            if (PontuacaoController.Instance.Pontuacao.Valor % 10 == 0 && PontuacaoController.Instance.Pontuacao.Valor > 0)
            {
                AtualizarPontuacaoFeedbackMultiplosDe10?.PlayFeedbacks();
            }
            else
            {
                AtualizarPontuacaoFeedback?.PlayFeedbacks();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Erro ao atualizar pontuação: {e.Message}");
            if (text == null)
            {
                // procura o gameobject com a tag QTD_MACAS
                GameObject qtdMacasObject = GameObject.FindGameObjectWithTag("QTD_MACAS");
                if (qtdMacasObject != null)
                {
                    text = qtdMacasObject.GetComponent<TMPro.TextMeshProUGUI>();
                    if (text != null)
                    {
                        text.text = "0"; // Define um valor padrão se o componente não for encontrado   

                    }
                    else
                    {
                        Debug.LogError("Componente TextMeshProUGUI não encontrado no objeto com a tag QTD_MACAS.");
                    }
                }
            }
            text.ForceMeshUpdate();
        }
    }
}