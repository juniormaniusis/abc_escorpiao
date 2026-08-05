using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerMenu : MonoBehaviour
{
    private static AudioManagerMenu instance;

    private void Awake()
    {
        // Implementação de Singleton para manter a música entre cenas
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Registra para evento de mudança de cena
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Você pode adicionar lógica aqui para mudar a música dependendo da cena
        // Por exemplo, parar a música quando sair do menu
        if (scene.name != "MainMenu")
        {
            // Opção 1: Parar a música
            // GetComponent<AudioSource>().Stop();

            // Opção 2: Diminuir o volume gradualmente
            // StartCoroutine(FadeOut(2.0f));
        }
    }

    // Método para fade out da música
    private System.Collections.IEnumerator FadeOut(float duration)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
    }

    private void OnDestroy()
    {
        // Limpa o evento quando o objeto for destruído
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
