using Assets.Scripts.Player;
using PixelCrushers;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LuaFunctionsToDialogue : MonoBehaviour
{
    public void EntregarMacas(double qtdMacas)
    {
        PontuacaoController.Instance.AdicionarPontos((int)qtdMacas);
    }

    public void RemoverMacas(double qtdMacas)
    {
        PontuacaoController.Instance.RemoverPontos((int)qtdMacas);
    }

    /// <summary>
    /// Carrega uma cena mantendo o player ativo (comportamento padrão).
    /// </summary>
    public void CarregarCena(string nomeCena)
    {
        CarregarCena(nomeCena, true);
    }

    /// <summary>
    /// Carrega uma cena com controle sobre o estado do player persistente.
    /// </summary>
    /// <param name="nomeCena">Nome da cena a carregar</param>
    /// <param name="playerAtivo">Se true, o player fica ativo na cena destino. 
    /// Se false, o player (e sua câmera) são desativados.</param>
    public void CarregarCena(string nomeCena, bool playerAtivo)
    {
        if (!playerAtivo)
        {
            // Desativa o jogador persistente (DontDestroyOnLoad)
            var playerStatus = PlayerStatus.Instance;
            if (playerStatus != null)
            {
                playerStatus.gameObject.SetActive(false);
            }

            // Desativa a câmera do jogador (VCAM_PLAYER)
            var vcam = GameObject.FindGameObjectWithTag("VCAM_PLAYER");
            if (vcam != null)
            {
                vcam.SetActive(false);
            }
        }

        SaveSystem.LoadScene(nomeCena);
    }

    void OnEnable()
    {
        Lua.RegisterFunction("EntregarMacas", this, SymbolExtensions.GetMethodInfo(() => EntregarMacas(0)));
        Lua.RegisterFunction("RemoverMacas", this, SymbolExtensions.GetMethodInfo(() => RemoverMacas(0)));
        Lua.RegisterFunction("CarregarCena", this, SymbolExtensions.GetMethodInfo(() => CarregarCena("")));
        Lua.RegisterFunction("CarregarCenaComPlayer", this,
            SymbolExtensions.GetMethodInfo(() => CarregarCena("", true)));
        Lua.RegisterFunction("AlterarPersonagem", this, SymbolExtensions.GetMethodInfo(() => AlterarPersonagem("")));
    }

    public void AlterarPersonagem(string personagem)
    {
        PlayerStatus.Instance.TrocarPersonagem(personagem);
    }


    void OnDisable()
    {
        Lua.UnregisterFunction("EntregarMacas");
        Lua.UnregisterFunction("RemoverMacas");
        Lua.UnregisterFunction("CarregarCena");
        Lua.UnregisterFunction("CarregarCenaComPlayer");
        Lua.UnregisterFunction("AlterarPersonagem");
    }
}
