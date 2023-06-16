using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;


public class TestRelay : MonoBehaviour
{
    [SerializeField] TMP_InputField joinCodeField;

    async void Start()
    {
        //Inicializar a API e conectar o usuario;
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Conectado em " + AuthenticationService.Instance.PlayerId);
        };

        //Conectar de forma "anonima" (nao relacionada a outro login);
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay()
    {
        //Criar uma alocacao na rede do relay e gerar o joinCode para usuarios se conectarem;
        try
        {           
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
                );

            joinCodeField.text = joinCode;
            joinCodeField.interactable = false;

            NetworkManager.Singleton.StartHost();
        }

        //Caso falhe, mostrar uma mensagem de erro;
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    public async void JoinRelay(string joinCode)
    {

        joinCode = joinCodeField.text;

        try
        {
            Debug.Log("Conectando ao Relay com Id: " + joinCode);

            //Atribuir a joinAllocation
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort)joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData
                );

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
