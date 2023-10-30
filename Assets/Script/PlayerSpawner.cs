using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform[] spawnPoints;
    [Inject] private DiContainer container;
    
    public void SpawnPlayer() 
    {
        int playerId = -1;

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("PlayerId", out object playerIdObj))
        {
            playerId = (int)playerIdObj;
        }

        string prefabName = "PlayerKnight" + playerId;
        Vector3 spawnPosition = spawnPoints[playerId].position;
        Quaternion spawnRotation = Quaternion.identity;
        GameObject player = PhotonNetwork.Instantiate(prefabName, spawnPosition, spawnRotation);
        
        if (player.GetPhotonView().IsMine)
        {
            container.InjectGameObject(player);
        }
    }
}
