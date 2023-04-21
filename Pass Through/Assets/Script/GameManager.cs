 
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private const string playerIdPrefix = "Player";
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public MatchSettings matchSettings;

    public static GameManager instance;
    
    
    [SerializeField]
    private GameObject sceneCamera;

    public delegate void OnPlayerKilledCallback(string player, string source);

    public OnPlayerKilledCallback onPlayerKilledCallback;

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
            return;
        }
        Debug.LogError("Plus d'une instance de GameManager dans la scene");
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if (sceneCamera==null)
        {
            return;
        }
        
        sceneCamera.SetActive(isActive);
        
    }

    public static void RegisterPlayer(string netId, Player player)
    {
        string playerId = playerIdPrefix + netId;
        players.Add(playerId, player);
        player.transform.name = playerId;
    }

    public static void UnregisterPlayer(string playerId)
    {
        players.Remove(playerId);
    }

    public static Player GetPlayer(string playerId)
    {
        if (players.ContainsKey(playerId)){
            return players[playerId];
        }
        return null;
        
    }

    public static Player[] GetAllPlayers()
    {
        return players.Values.ToArray();
    }

}
