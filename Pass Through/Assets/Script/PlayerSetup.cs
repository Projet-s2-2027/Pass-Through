using UnityEngine;
using Mirror;
using Unity.VisualScripting;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    
    [SerializeField]
    private string remoteLayerName = "RemotePlayer";
    
    Camera sceneCamera;
    
    private void Start()
    {
        if (!isLocalPlayer)
        {
            for (int i = 0; i < componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false;
            }

            
            // On desactive les composants renseigner si ce n'est pas notre joueur 
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            
            sceneCamera = Camera.main;
            if (sceneCamera!=null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }
    
    }
    
    public override void OnStartClient()
    {
        base.OnStartClient();
        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();
        GameManager.RegisterPlayer(netId,player);
    }
    
    private void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }
    
    private void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }
    
    private void OnDisable()
    {
        if (sceneCamera!=null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    
        GameManager.UnregisterPlayer(transform.name);
    }
}
