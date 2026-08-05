using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.GameManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{

    [Header("Audio")]
    public AudioClip OnClickSound;

    [Header("Menu Buttons")]
    public Button playButton;
    public Button creditButton;
    public Button quitButton;

    [Header("Creditos 1 Buttons")]
    public Button cross1Button;
    public Button rightArrowButton;

    [Header("Creditos 2 Buttons")]
    public Button cross2Button;
    public Button leftArrowButton;

    [Header("Menu Screens")]
    public GameObject mainMenuScreen;
    public GameObject creditos1Screen;
    public GameObject creditos2Screen;

    [Header("Game Settings")]
    public string firstLevelName = "NeighborhoodScene"; // Nome da cena do primeiro n�vel

    public virtual void OnClick()
    {
        if (OnClickSound != null)
        {
            AudioSource.PlayClipAtPoint(OnClickSound, Camera.main.transform.position);
        }
    }

    private void Start()
    {
        // Garante que o menu principal est� ativo e as op��es n�o
        if (mainMenuScreen != null) mainMenuScreen.SetActive(true);
        if (creditos1Screen != null) creditos1Screen.SetActive(false);
        if (creditos2Screen != null) creditos2Screen.SetActive(false);

        // Listeners bot�es do menu principal
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);

        if (creditButton != null)
            creditButton.onClick.AddListener(OpenCredit1);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);

        // Listeners bot�es do cr�ditos 1
        if (cross1Button != null)
            cross1Button.onClick.AddListener(CloseCredit);

        if (rightArrowButton != null)
            rightArrowButton.onClick.AddListener(OpenCredit2);

        // Listeners bot�es do cr�ditos 2
        if (cross2Button != null)
            cross2Button.onClick.AddListener(CloseCredit);

        if (leftArrowButton != null)
            leftArrowButton.onClick.AddListener(OpenCredit1);

    }

    // Inicia o jogo carregando o primeiro n�vel
    public void PlayGame()
    {
        Debug.Log("Iniciando o jogo...");
        SceneTransition.LoadRaw(firstLevelName);
    }

    // Abre a tela de creditos 1
    public void OpenCredit1()
    {
        Debug.Log("Abrindo cr�ditos 1...");
        if (mainMenuScreen != null) mainMenuScreen.SetActive(false);
        if (creditos1Screen != null) creditos1Screen.SetActive(true);
        if (creditos2Screen != null) creditos2Screen.SetActive(false);
    }

    // Abre a tela de creditos 2
    public void OpenCredit2()
    {
        Debug.Log("Abrindo cr�ditos 2...");
        if (mainMenuScreen != null) mainMenuScreen.SetActive(false);
        if (creditos1Screen != null) creditos1Screen.SetActive(false);
        if (creditos2Screen != null) creditos2Screen.SetActive(true);
    }

    // Volta para o menu principal a partir das op��es
    public void CloseCredit()
    {
        Debug.Log("Fechando cr�ditos...");
        if (mainMenuScreen != null) mainMenuScreen.SetActive(true);
        if (creditos1Screen != null) creditos1Screen.SetActive(false);
        if (creditos2Screen != null) creditos2Screen.SetActive(false);
    }

    // Sai do jogo
    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");

#if UNITY_EDITOR
        // Se estiver no editor, para o modo Play
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Se for build, fecha o aplicativo
        Application.Quit();
#endif
    }
}
