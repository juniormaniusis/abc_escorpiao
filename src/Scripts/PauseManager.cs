using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{

    public Button pauseButton;

    [Header("Pause Menu")]
    public GameObject pauseMenuPanel;
    public Button resumeButton;
    public Button controlButton;
    public Button mainMenuButton;

    [Header("Exit Menu")]
    public GameObject exitMenuPanel;
    public Button continueButton;
    public Button exitButton;

    [Header("Comand Panel")]
    public GameObject comandPanel;
    public Button crossButton;

    [Header("Audio Settings")]
    public AudioSource backgroundMusic;
    public float pausedMusicVolume = 0.3f;

    private bool isPaused = false;
    private float originalMusicVolume;

    void Start()
    {
        // Garante que o jogo começa despausado
        isPaused = false;
        Time.timeScale = 1f;

        // Garante que o menu de pausa está desativado inicialmente
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
        if (exitMenuPanel != null)
            exitMenuPanel.SetActive(false);
        if (comandPanel != null)
            comandPanel.SetActive(false);

        // Configura os listeners dos botões
        if (pauseButton != null)
            pauseButton.onClick.AddListener(TogglePause);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (controlButton != null)
            controlButton.onClick.AddListener(OpenControlPanel);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OpenExitMenu);

        if (continueButton != null)
            continueButton.onClick.AddListener(ReturnMenuPanel);

        if (exitButton != null)
            exitButton.onClick.AddListener(ReturnToMainMenu);

        if (crossButton != null)
            crossButton.onClick.AddListener(ReturnMenuPanel);

        // Salva o volume original da música
        if (backgroundMusic != null)
            originalMusicVolume = backgroundMusic.volume;
    }

    void Update()
    {
        // Permite pausar/despausar com a tecla ESC também
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
            PauseGame();
        else
            ResumeGame();
    }

    void PauseGame()
    {
        // Para o tempo do jogo
        Time.timeScale = 0f;

        // Ativa o menu de pausa
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        // Reduz o volume da música
        if (backgroundMusic != null)
            backgroundMusic.volume = pausedMusicVolume;

        // Opcional: Desativa o botão de pausa quando o menu está aberto
        if (pauseButton != null)
            pauseButton.gameObject.SetActive(false);
    }

    public void ResumeGame()
    {
        // Restaura o tempo do jogo
        Time.timeScale = 1f;

        // Desativa o menu de pausa
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        // Restaura o volume da música
        if (backgroundMusic != null)
            backgroundMusic.volume = originalMusicVolume;

        // Reativa o botão de pausa
        if (pauseButton != null)
            pauseButton.gameObject.SetActive(true);

        isPaused = false;
    }

    public void OpenControlPanel()
    {
        Debug.Log("Abrindo os controles...");
        // Ativa o painel de comandos
        if (comandPanel != null)
        {
            comandPanel.SetActive(true);
            pauseMenuPanel.SetActive(false);
            exitMenuPanel.SetActive(false);
        }
    }

    public void OpenExitMenu()
    {
        Debug.Log("Abrindo confirmação de saída...");
        // Ativa o menu de pausa
        if (exitMenuPanel != null)
        {
            exitMenuPanel.SetActive(true);
            pauseMenuPanel.SetActive(false);
            comandPanel.SetActive(false);
        }
    }

    public void ReturnMenuPanel()
    {
        Debug.Log("Retornando ao menu de pausa...");
        // Ativa o menu de pausa
        if (pauseMenuPanel != null)
        {
            exitMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(true);
            comandPanel.SetActive(false);
        }
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("Retornando ao menu principal...");
        // Restaura o timeScale antes de mudar de cena
        Time.timeScale = 1f;

        // Carrega a cena do menu principal
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}
