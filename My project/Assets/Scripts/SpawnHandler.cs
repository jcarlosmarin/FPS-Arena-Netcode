using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SpawnHandler : NetworkBehaviour
{
    [SerializeField] Transform playerPrefab;

    public override void OnNetworkSpawn()
    {
        //Ao terminar de carregar a cena, executar um evento;
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong>clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (IsServer && sceneName == "GameScene")
        {
            foreach (ulong id in clientsCompleted)
            {
                Debug.Log(clientsCompleted.Count);

                //Instanciar o objeto player;
                Transform playerTransform = Instantiate(playerPrefab);

                //Usar a funcao de "SpawnAsPlayerObject" dentro do NetworkObject para conectar o player;
                NetworkObject networkObject = playerTransform.GetComponent<NetworkObject>();

                networkObject.SpawnAsPlayerObject(id, true);

                //Mudar a posicao e rotacao do player aleatoriamente;
                playerTransform.position = new Vector3(Random.Range(-8, 8), 3f, Random.Range(-8, 8));
                playerTransform.Rotate(new Vector3(0f, Random.Range(-180, 180), 0f), Space.Self);
            }
        }
    }
}