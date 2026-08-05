using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Creditos_1 : MonoBehaviour
{

    [Header("Audio")]
    public AudioClip OnClickSound;

    [Header("Menu Buttons")]
    public Button playButton;
    public Button optionsButton;
    public Button quitButton;

    [Header("Menu Screens")]
    public GameObject mainMenuScreen;
    public GameObject optionsScreen;

    [Header("Game Settings")]
    public string firstLevelName = "Level1"; // Nome da cena do primeiro nível

    public virtual void OnClick()
    {
        if (OnClickSound != null)
        {
            AudioSource.PlayClipAtPoint(OnClickSound, Camera.main.transform.position);
        }
    }

    private void Start()
    {
        // Garante que o menu principal está ativo e as opções não
        if (mainMenuScreen != null) mainMenuScreen.SetActive(true);
        if (optionsScreen != null) optionsScreen.SetActive(false);

        // Adiciona listeners aos botões
        if (playButton != null)
            playButton.onClick.AddListener(PlayGame);

        if (optionsButton != null)
            optionsButton.onClick.AddListener(OpenOptions);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    // Inicia o jogo carregando o primeiro nível
    public void PlayGame()
    {
        Debug.Log("Iniciando o jogo...");
        SceneManager.LoadScene(firstLevelName);
    }

    // Abre a tela de opções
    public void OpenOptions()
    {
        Debug.Log("Abrindo opções...");
        if (mainMenuScreen != null) mainMenuScreen.SetActive(false);
        if (optionsScreen != null) optionsScreen.SetActive(true);
    }

    // Volta para o menu principal a partir das opções
    public void CloseOptions()
    {
        Debug.Log("Fechando opções...");
        if (optionsScreen != null) optionsScreen.SetActive(false);
        if (mainMenuScreen != null) mainMenuScreen.SetActive(true);
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
