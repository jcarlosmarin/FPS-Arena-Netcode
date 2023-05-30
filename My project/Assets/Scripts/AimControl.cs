using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AimControl : NetworkBehaviour
{
    [SerializeField] GameObject myCameraObject;

    public GameObject objectHit;
    AudioListener myAudio;
    Camera myCamera;
    
    EndGame gameManager;
    
    float reloading;
    
    public override void OnNetworkSpawn()
    {
        //Find no objeto que finaliza o jogo
        gameManager = FindObjectOfType<EndGame>();

        if (!IsOwner)
            return;

        //Habilita a camera e o audio localmente para o dono do personagem;
        myCamera = myCameraObject.GetComponent<Camera>();
        myCamera.enabled = true;

        myAudio = myCameraObject.GetComponent<AudioListener>();
        myAudio.enabled = true;
    }


    void Update()
    {
        if (!IsOwner)
            return;

        //Atirar;
        if (Input.GetButtonDown("Fire1"))
        {
            if (reloading <= 0)
            {
                //Dispara
                Fire();
            }
            else
            {
                Debug.Log("Reloading");
            }
        }

        //Recarregamento;
        if (reloading > 0)
        {
            reloading -= 1 * Time.deltaTime;
        }
    }

    void Fire()
    {
        //Esvazia a arma
        reloading = 1;

        //Disparo via Raycast;
        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                EndGameServerRpc();
                hit.collider.gameObject.SetActive(false);
            }
        }
    }

    [ServerRpc]
    public void EndGameServerRpc()
    {
        EndGameClientRpc();
    }

    [ClientRpc]
    public void EndGameClientRpc()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameManager.restartButton.gameObject.SetActive(true);
        gameManager.quitButton.gameObject.SetActive(true);

        if (IsOwner)
        {
            gameManager.victoryText.SetActive(true);
        }
        else
        {
            gameManager.defeatText.SetActive(true);
            gameManager.spectatorCamera.enabled = true;
        }
    }
}
