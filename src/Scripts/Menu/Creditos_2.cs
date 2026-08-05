using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Creditos_2 : MonoBehaviour
{

    [Header("Audio")]
    public AudioClip OnClickSound;

    [Header("Buttons")]
    public Button crossButton;
    public Button backArrowButton;
    public Button quitButton;

    [Header("Screens")]
    public GameObject mainMenuScreen;
    public GameObject creditos1Screen;
    public GameObject creditos2Screen;

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
        // Garante que a tela de créditos está ativo e as outras opções não
        if (creditos2Screen != null) creditos2Screen.SetActive(true);
        if (mainMenuScreen != null) mainMenuScreen.SetActive(false);
        if (creditos1Screen != null) creditos1Screen.SetActive(false);

        // Adiciona listeners aos botões
        if (crossButton != null)
            crossButton.onClick.AddListener(CloseCredits);

        if (backArrowButton != null)
            backArrowButton.onClick.AddListener(OpenOptions);

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
        if (creditos1Screen != null) creditos1Screen.SetActive(true);
    }

    // Volta para o menu principal a partir das opções
    public void CloseCredits()
    {
        Debug.Log("Fechando créditos...");
        if (creditos2Screen != null) creditos2Screen.SetActive(false);
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
