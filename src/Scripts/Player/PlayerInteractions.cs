using System;
using Assets.Scripts.GameManager;
using Assets.Scripts.Items;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private Camera mainCamera;

    // Inicializa componentes e configurações iniciais
    private void Start()
    {
        InitializeComponents();
        // LockCursor();
    }

    // Inicializa os componentes necessários
    private void InitializeComponents()
    {
        mainCamera = Camera.main;
    }

    // Trava o cursor no centro da tela
    // private void LockCursor()
    // {
    //     Cursor.lockState = CursorLockMode.Locked;
    //     Cursor.visible = false;
    // }

    // Atualiza o estado do cursor quando o foco da aplicação muda
    // private void OnApplicationFocus(bool hasFocus)
    // {
    //     if (hasFocus)
    //     {
    //         LockCursor();
    //     }
    // }
}