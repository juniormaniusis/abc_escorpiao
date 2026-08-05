using System;
using UnityEngine;
using UnityEngine.AI;
using PixelCrushers.DialogueSystem;
using Assets.Scripts.Enums;

public class GeradorMacasAgent : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public Vector3 origem;
    public Vector3 destino;
    public NavMeshAgent agent;

    [Header("Configurações de Geração")]
    [SerializeField] private GameObject macaPrefab;
    [SerializeField] private GameObject macaVerdePrefab;
    [SerializeField] private float distanciaParaGerar = float.MaxValue;

    [Header("Chance de Maçã Verde (%)")]
    [Range(0, 100)] // Mudei para 0-100 para ser mais intuitivo
    [SerializeField] private float chanceMacaVerde = 11f; // Agora representa porcentagem real

    private float distanciaPercorrida = 0f;
    private Vector3 ultimaPosicao;
    [SerializeField]
    private float distanciaMinima = 0.5f; // Valor de tolerância para considerar que chegou ao destino

    private bool caminhoPronto = false;
    private float tempoEsperaMaximo = 5f; // Tempo máximo para aguardar o caminho
    private float tempoEsperaAtual = 0f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent não encontrado. Certifique-se de que o componente esteja anexado ao GameObject.");
            enabled = false;
        }
    }

    public void ConfigurarTrilhaEIniciar(Vector3 origem, Vector3 destino, float distanciaParaGerar)
    {
        if (!ValidarConfiguracoes(origem, destino, macaPrefab, macaVerdePrefab)) return;

        this.origem = origem;
        this.destino = destino;
        this.distanciaParaGerar = distanciaParaGerar;

        InicializarGerador();
    }

    private bool ValidarConfiguracoes(Vector3 origem, Vector3 destino, GameObject macaPrefab, GameObject macaVerdePrefab)
    {
        if (origem == null || destino == null)
        {
            Debug.LogError("As referências 'origem' ou 'destino' não foram atribuídas.");
            enabled = false;
            return false;
        }

        if (macaPrefab == null || macaVerdePrefab == null)
        {
            Debug.LogError("Prefab da maçã não foi definido.");
            enabled = false;
            return false;
        }

        return true;
    }

    private void Update()
    {
        if (!caminhoPronto)
        {
            tempoEsperaAtual += Time.deltaTime;

            if (agent.hasPath && agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                caminhoPronto = true;
                Debug.Log("Caminho preparado com sucesso.");
            }
            else if (tempoEsperaAtual >= tempoEsperaMaximo)
            {
                Debug.LogWarning("Tempo limite para preparar o caminho excedido. Forçando início.");
                caminhoPronto = true;
            }
            else if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                Debug.LogError("Caminho inválido detectado. Tentando reconfigurar...");
                TentarReconfigurarCaminho();
            }

            return;
        }

        // Verificar se o agente ainda está ativo e se movendo
        if (!agent.enabled || agent.isStopped)
        {
            return;
        }

        AtualizarDistanciaPercorrida();
        VerificarGeracaoDeMaca();
        VerificarChegadaAoDestino();
    }

    private void TentarReconfigurarCaminho()
    {
        agent.ResetPath();
        agent.Warp(origem);

        // Aguarda um frame antes de reconfigurar
        StartCoroutine(ReconfigurarCaminhoComDelay());
    }

    private System.Collections.IEnumerator ReconfigurarCaminhoComDelay()
    {
        yield return null; // Aguarda um frame
        agent.SetDestination(destino);
        tempoEsperaAtual = 0f;
    }

    private void AtualizarDistanciaPercorrida()
    {
        // Verificar se o agente realmente se moveu
        float distancia = Vector3.Distance(agent.transform.position, ultimaPosicao);

        // Só conta a distância se for significativa (evita micro-movimentos)
        if (distancia > 0.01f)
        {
            distanciaPercorrida += distancia;
            ultimaPosicao = agent.transform.position;
        }
    }

    private void InicializarGerador()
    {
        // Garantir que o agente esteja habilitado
        agent.enabled = true;
        agent.isStopped = false;

        agent.Warp(origem);
        ultimaPosicao = origem; // Inicializa a última posição

        // Gera uma maçã inicial no ponto de origem
        GerarMaca(DeterminarTipoMaca());

        // Aguardar um frame antes de definir o destino
        StartCoroutine(DefinirDestinoComDelay());
    }

    private System.Collections.IEnumerator DefinirDestinoComDelay()
    {
        yield return null; // Aguarda um frame
        agent.SetDestination(destino);
        caminhoPronto = false;
        tempoEsperaAtual = 0f;

        // Aguardar múltiplos frames para dar tempo do NavMesh calcular o caminho
        for (int i = 0; i < 3; i++)
        {
            yield return null;
        }

        // Força o início do caminho mesmo se não estiver totalmente calculado
        caminhoPronto = true;
    }

    private TipoMaca DeterminarTipoMaca()
    {
        float numeroAleatorio = UnityEngine.Random.Range(0f, 100f);

        if (numeroAleatorio <= chanceMacaVerde)
        {
            // Debug.Log($"Gerando maçã VERDE (número: {numeroAleatorio:F1}, chance: {chanceMacaVerde}%)");
            return TipoMaca.Verde;
        }
        else
        {
            // Debug.Log($"Gerando maçã VERMELHA (número: {numeroAleatorio:F1}, chance: {100f - chanceMacaVerde}%)");
            return TipoMaca.Vermelha;
        }
    }

    private void VerificarGeracaoDeMaca()
    {
        if (distanciaPercorrida >= distanciaParaGerar)
        {
            distanciaPercorrida = 0f;
            GerarMaca(DeterminarTipoMaca());
        }
    }

    private void VerificarChegadaAoDestino()
    {
        var distancia = Vector3.Distance(agent.transform.position, destino);
        if (distancia < distanciaMinima)
        {
            agent.isStopped = true; // Para o movimento do agente
            gameObject.SetActive(false); // Desativa o GameObject
            return;
        }
    }

    private void GerarMaca(TipoMaca tipoMaca = TipoMaca.Vermelha)
    {
        if (tipoMaca == TipoMaca.Verde)
        {
            GerarMacaVerde();
            return;
        }

        GerarMacaVermelha();
    }
    private void GerarMacaVerde()
    {
        Instantiate(macaVerdePrefab, agent.transform.position, Quaternion.identity);
    }
    private void GerarMacaVermelha()
    {
        Instantiate(macaPrefab, agent.transform.position, Quaternion.identity);
    }

    private enum TipoMaca
    {
        Verde,
        Vermelha
    }
}