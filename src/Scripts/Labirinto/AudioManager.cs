using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Singleton para acesso fácil

    public AudioClip backgroundMusic;    // Música de fundo normal
    public AudioClip victoryMusic;       // Música quando a fase termina bem
    public AudioClip lostMusic;          // Música quando a fase termina mal

    public float fadeSpeed = 1.0f;       // Velocidade da transição entre músicas

    private AudioSource audioSource;

   
}