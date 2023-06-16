using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AimControl : NetworkBehaviour
{
    [SerializeField] GameObject myCameraObject;
    AudioListener myAudio;
    Camera myCamera;
    
    EndGame gameManager;
    
    float reloading;
    
    public override void OnNetworkSpawn()
    {
        //Find no objeto que finaliza o jogo para usar seus objetos
        gameManager = FindObjectOfType<EndGame>();

        //Habilita a camera e o audio localmente para o dono do personagem;
        if (!IsOwner)
            return;

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
                //Disparar
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
        //se o jogo ja tiver sido finalizado, o tiro deixa de funcionar
        if (!gameManager.gameOn.Value)
            return;

        //Esvazia a arma
        reloading = 1;

        //Disparo via Raycast;
        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.gameObject.GetComponentInParent<PlayerMovement>().Die();
                EndGameServerRpc();
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

        //Botoes de reiniciar e sair para o host
        if (IsServer)
        {
            gameManager.restartButton.gameObject.SetActive(true);
            gameManager.quitButton.gameObject.SetActive(true);

            gameManager.gameOn.Value = false;
        }

        //Telas de vitoria e derrota
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
