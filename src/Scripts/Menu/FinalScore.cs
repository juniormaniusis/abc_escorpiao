using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using PixelCrushers.DialogueSystem;
using Assets.Scripts.GameManager;
using Assets.Scripts.Player;

public class FinalScore : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip OnClickSound;
    public AudioClip backgroundMusic;
    public AudioSource audioSource;

    [Header("Score")]
    public TMP_Text scoreText;
    public long targetScore;
    public float duration = 2f;

    [Header("Button")]
    public Button menuButton;

    [Header("Screen")]
    public GameObject finalScoreScreen;



    [Header("Next Scene")]
    public string nextScene = "MainMenu"; // Nome da cena do menu principal

    public virtual void OnClick()
    {
        if (OnClickSound != null)
        {
            AudioSource.PlayClipAtPoint(OnClickSound, Camera.main.transform.position);
        }
    }

    private void Start()
    {
        if (finalScoreScreen != null) finalScoreScreen.SetActive(true);



        if (menuButton != null)
            menuButton.onClick.AddListener(MainMenuButton);

        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }


        targetScore = PontuacaoController.Instance?.Pontuacao != null ? PontuacaoController.Instance.Pontuacao.Valor : 483L;

        StartCoroutine(CountScore());
    }

    private System.Collections.IEnumerator CountScore()
    {
        float elapsed = 0f;
        int currentScore = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            currentScore = Mathf.RoundToInt(Mathf.Lerp(0, targetScore, t));
            scoreText.text = currentScore.ToString();
            yield return null;
        }

        scoreText.text = targetScore.ToString();
    }

    public void Update()
    {
        // a cada 0.1 segundos emite um som

    }

    // Vai para tela de menu inicial
    public void MainMenuButton()
    {
        Debug.Log("Abre o menu inicial");
        SceneTransition.LoadRaw(nextScene);
    }
}
