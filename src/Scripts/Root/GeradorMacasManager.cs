using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Player;
using UnityEngine.Playables;

public class GeradorMacasManager : MonoBehaviour
{
    [Header("Configurações")] [SerializeField]
    private List<Trilha> trilhas; // Lista de trilhas

    [SerializeField] private float distanciaParaGerar = 1.5f; // Distância para gerar uma maçã
    [SerializeField] private GameObject prefabAgente; // Prefab do agente gerador de maçãs

    private float ultimaVerificacao = 0f;
    private float intervaloVerificacao = 0.1f; // Verificar a cada 100ms ao invés de todo frame

    private void Update()
    {
        if (Time.time - ultimaVerificacao >= intervaloVerificacao)
        {
            ProcessarTrilhasDisponiveis();
            ultimaVerificacao = Time.time;
        }
    }

    private void ProcessarTrilhasDisponiveis()
    {
        // Se o PlayerStatus ainda não existe ou foi destruído, não processa
        if (PlayerStatus.Instance == null)
            return;

        // Se alguma Timeline estiver tocando, não gera maçãs
        if (FindObjectsOfType<PlayableDirector>().Any(d => d.state == PlayState.Playing))
            return;

        var trilhasDisponiveis = trilhas
            .Where(t => !t.Percorrida && t.PodeGerar(PlayerStatus.Instance.gameObject.transform))
            .ToList();

        if (!trilhasDisponiveis.Any()) return;

        foreach (var trilha in trilhasDisponiveis)
        {
            CriarAgenteParaTrilha(trilha);
        }
    }

    private void CriarAgenteParaTrilha(Trilha trilha)
    {
        if (trilha.Percorrida) return;

        GameObject agente = Instantiate(prefabAgente, trilha.origem.position, Quaternion.identity);

        if (!agente.TryGetComponent<GeradorMacasAgent>(out var geradorMacasAgent))
        {
            Debug.LogError("GeradorMacasAgent não encontrado no prefab.");
            Destroy(agente);
            return;
        }

        geradorMacasAgent.ConfigurarTrilhaEIniciar(trilha.origem.position, trilha.destino.position, distanciaParaGerar);
        trilha.MarcarComoPercorrida();
    }
}