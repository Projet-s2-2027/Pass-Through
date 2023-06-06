using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PauseMenu : NetworkBehaviour
{
    public static bool isOn = false;

    private NetworkManager networkManager;
    private void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    public void LeaveRoomButton()
    {
        Debug.Log("click");
        if (isClientOnly)
        {
            networkManager.StopClient();
        }
        else
        {
            networkManager.StopHost();
        }
    }
}
