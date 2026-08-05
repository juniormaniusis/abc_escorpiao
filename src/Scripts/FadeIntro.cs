using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Cobre o primeiro instante da NeighborhoodScene com um quadro preto e faz
/// fade-out, escondendo o "flash" do player que ocorre enquanto o CinemachineBrain
/// e o Timeline "Chegando na Cidade" ainda estão se acomodando na transição de cena.
///
/// O Image deve estar salvo na cena já preto e opaco (alpha 1) para cobrir o
/// frame 0. Este script segura o preto por um curto período e depois revela a
/// animação suavemente.
/// </summary>
[RequireComponent(typeof(Image))]
public class FadeIntro : MonoBehaviour
{
    [Tooltip("Tempo (s) que a tela fica totalmente preta antes de começar a abrir.")]
    [SerializeField] private float holdPreto = 0.15f;

    [Tooltip("Duração (s) do fade-out até a tela ficar totalmente visível.")]
    [SerializeField] private float duracaoFade = 0.5f;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        // Garante preto opaco já no primeiro frame, independentemente do que
        // estiver salvo na cena.
        var c = image.color;
        image.color = new Color(c.r, c.g, c.b, 1f);
        image.raycastTarget = false;
    }

    private IEnumerator Start()
    {
        if (holdPreto > 0f)
        {
            yield return new WaitForSeconds(holdPreto);
        }

        float t = 0f;
        Color c = image.color;
        while (t < duracaoFade)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - t / duracaoFade);
            image.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        image.color = new Color(c.r, c.g, c.b, 0f);
        // Não precisa mais renderizar/atualizar; desativa o objeto.
        gameObject.SetActive(false);
    }
}
