using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class EndGame : NetworkBehaviour
{
    public GameObject victoryText;
    public GameObject defeatText;

    public Button restartButton;
    public Button quitButton;

    public Camera spectatorCamera;
}
