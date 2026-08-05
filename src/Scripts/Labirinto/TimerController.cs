using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManager;
using Assets.Scripts.Player;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerControler : MonoBehaviour
{
    [Header("Time")] public Image timer_bg;
    public Image timer_fg;
    float time_remaining;
    bool timer_controll = true;
    public float max_time = 20f;
    private bool gameEnded = false;

    [Header("Audio")] public AudioSource audioSource;
    public AudioClip backgroundMusic;
    public AudioClip victoryMusic;
    public AudioClip lostMusic;

    [Header("Trigger finish")] public GameObject finishFail;
    public GameObject finishSuccess;
    public bool isFinishFail = false;
    public bool isFinishSuccess = false;

    [Header("Game Over")] public GameObject game_over_panel;
    public Button retryButton;
    public string previousScene = "NeighborhoodScene"; // Nome da próxima anterior

    [Header("Game Win")] public GameObject success_panel;
    public Button continueButton;
    public string nextScene = "NeighborhoodScene"; // Nome da próxima cena

    void Awake()
    {
        DialogueManager.SetDialoguePanel(false, immediate: true);
    }

    void Start()
    {
        time_remaining = max_time;

        isFinishFail = DialogueLua.GetVariable("Quest3_1.FailedMission").AsBool;
        isFinishSuccess = DialogueLua.GetVariable("Quest3_1.SuccessMission").AsBool;

        // Toca a música de fundo ao iniciar a fase
        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }

        // Garante que os painéis de sucesso e falha estão desativados inicialmente
        if (game_over_panel != null) game_over_panel.SetActive(false);
        if (success_panel != null) success_panel.SetActive(false);

        // Listeners botões dos painéis de jogo
        if (retryButton != null)
            retryButton.onClick.AddListener(RetryGame);

        if (continueButton != null)
            continueButton.onClick.AddListener(NextScene);

        // Verifica se será ativo objeto de falha ou sucesso
        if (isFinishFail)
        {
            finishFail.SetActive(true);
            finishSuccess.SetActive(false);
        }
        else if (isFinishSuccess)
        {
            finishFail.SetActive(false);
            finishSuccess.SetActive(true);
        }
        else
        {
            finishFail.SetActive(false);
            finishSuccess.SetActive(false);
        }
    }

    void Update()
    {
        if (gameEnded) return;
        if (time_remaining > 0 && timer_controll)
        {
            time_remaining -= Time.deltaTime;
            timer_fg.fillAmount = time_remaining / max_time;
        }
        else if (time_remaining <= 0 && timer_controll)
        {
            Debug.Log("fail");
            GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (gameEnded) return;

        Debug.Log("success");
        GameWin();
    }

    public void GameOver()
    {
        game_over_panel.SetActive(true);
        timer_controll = false;
        gameEnded = true;
        // Para a música de fundo e toca a música de derrota
        PlayMusic(lostMusic, false);
    }

    private void RetryGame()
    {
        ReativarPlayer();
        // Carrega a cena anterior
        SceneTransition.LoadRaw(previousScene);
    }

    public void GameWin()
    {
        success_panel.SetActive(true);
        timer_controll = false;
        gameEnded = true;
        // Para a música de fundo e toca a música de vitória
        PlayMusic(victoryMusic, false);
    }

    private void NextScene()
    {
        ReativarPlayer();
        // Carrega a próxima cena
        SceneTransition.LoadRaw(nextScene);
    }

    private void PlayMusic(AudioClip clip, bool loop)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }

    /// <summary>
    /// Reativa o jogador persistente (DontDestroyOnLoad) que foi desativado ao entrar no labirinto.
    /// </summary>
    private void ReativarPlayer()
    {
        var playerStatus = PlayerStatus.Instance;
        if (playerStatus != null)
        {
            playerStatus.gameObject.SetActive(true);
        }
    }
}
