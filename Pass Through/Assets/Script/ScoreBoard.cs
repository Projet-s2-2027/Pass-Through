using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{

    [SerializeField] 
    private GameObject playerScoreboardItem;
    
    [SerializeField]
    private Transform playerScoreboardList;
    
    private void OnEnable()
    {
        
        Player[] players = GameManager.GetAllPlayers();
        foreach (Player player in players)
        {
            GameObject itemGO = Instantiate(playerScoreboardItem, playerScoreboardList);
            PlayerScoreBoardItem item = itemGO.GetComponent<PlayerScoreBoardItem>();
            if (item != null)
            {
                item.Setup(player);
            }
        }

        
    }

    private void OnDisable()
    {
        foreach (Transform child in playerScoreboardList)
        {
            Destroy(child.gameObject);
        }
    }
}
