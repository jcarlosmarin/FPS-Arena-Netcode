using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class EndGame : NetworkBehaviour
{
    public NetworkVariable<bool> gameOn = new NetworkVariable<bool>();

    public GameObject victoryText;
    public GameObject defeatText;

    public Button restartButton;
    public Button quitButton;

    public Camera spectatorCamera;

    public override void OnNetworkSpawn()
    {
        //Servidor reseta o estado do jogo
        if (IsServer)
        {
            gameOn.Value = true;
        }
    }
}
