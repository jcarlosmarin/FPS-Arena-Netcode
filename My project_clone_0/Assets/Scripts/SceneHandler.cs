using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SceneHandler : NetworkBehaviour
{
    [SerializeField] Transform playerPrefab;
    private bool initialSpawnDone = false;

    public void LoadGame()
    {
        if (!IsServer)
            return;

        //Carregar cena pelo NetworkManager;
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (!initialSpawnDone && sceneName == "GameScene")
        {
            initialSpawnDone = true;
            foreach (ulong id in NetworkManager.Singleton.ConnectedClientsIds)
            {
                //Instanciar o objeto player;
                Transform playerTransform = Instantiate(playerPrefab);

                //Usar a funcao de "SpawnAsPlayerObject" dentro do NetworkObject para conectar o player;
                NetworkObject networkObject = playerTransform.GetComponent<NetworkObject>();

                networkObject.SpawnAsPlayerObject(id, true);
            }
        }
    }
}
