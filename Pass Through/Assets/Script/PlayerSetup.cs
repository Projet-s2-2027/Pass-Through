using UnityEngine;
using Mirror;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    
    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    [SerializeField] 
    private string dontDrawLayerName = "DontDraw";

    [SerializeField] private GameObject playerGraphics;
    
    [SerializeField]
    private GameObject playerUIPrefab;
    [HideInInspector]
    public GameObject playerUIInstance;
    
    private void Start()
    {
        if (!isLocalPlayer)
        {
            // On desactive les composants renseigner si ce n'est pas notre joueur 
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            
            //desactiver la partie graphique du joueur local
            Util.SetLayerRecursively(playerGraphics,LayerMask.NameToLayer(dontDrawLayerName));
            
            //Creation du UI du joueur local
            playerUIInstance=Instantiate(playerUIPrefab);

            //Configuration du UI
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui==null)
            {
                Debug.LogError("Pas de component PlayerUI sur playerUIInstance");
            }
            else
            {
                ui.SetPlayer(GetComponent<Player>());
            }
        }
        GetComponent<Player>().Setup();
    }
    
    [Command]
    void CmdSetUsername(string playerId, string username){
        Player player = GameManager.GetPlayer(playerId);
        if (player != null){
            player.username = username;

        }
    }


    public override void OnStartServer()
    {
        base.OnStartServer();
        RegisterPlayerAndUsername();

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        RegisterPlayerAndUsername();
    }

    void RegisterPlayerAndUsername(){
        NetworkIdentity infoIdentity = GetComponent<NetworkIdentity>();
        string netId =infoIdentity.netId.ToString();
        Player player = GetComponent<Player>();
        GameManager.RegisterPlayer(netId,player);

        CmdSetUsername(transform.name, UserAccountManager.LoggedIdUsername);
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
        Destroy(playerUIInstance);


        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
        }
        
        GameManager.UnregisterPlayer(transform.name);
    }
}
