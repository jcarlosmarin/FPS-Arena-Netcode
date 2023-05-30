using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SelectableHandler : MonoBehaviour
{
    [SerializeField] Button hostButton;
    [SerializeField] Button clientButton;
    [SerializeField] Button startButton;

    private void Awake()
    {
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();

            //Desabilitar botoes de Join, habilitar botao de Start;
            hostButton.interactable = false;
            clientButton.interactable = false;
            startButton.interactable = true;
        });
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();

            //Desabilitar botoes de Join;
            hostButton.interactable = false;
            clientButton.interactable = false;
        });
    }
}
