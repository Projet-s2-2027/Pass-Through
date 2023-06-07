using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : NetworkBehaviour
{
    public KeyCode interactKey = KeyCode.E;

    [SyncVar]
    public GameObject button;

    public Light targetLight1;
    public Light targetLight2;
    public Light targetLight3;
    public Light targetLight4;

    private bool isLookingAtButton = false;
    [SyncVar]
    private bool isLightEnabled = true;
    public GameObject state1;
    public GameObject state2;
    public Player infoPlayer;

    void Update()
    {
        if (isLookingAtButton && infoPlayer != null && infoPlayer.interacte)
        {
            ToggleLight();
        }
        LightUpdate();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            infoPlayer = other.gameObject.GetComponent<Player>();
            isLookingAtButton = true;
            
        } 
        else{
            isLookingAtButton = false;
        }
    }


    void OnTriggerExit()
    {
        isLookingAtButton = false;
        infoPlayer = null;
    }

    void LightUpdate(){
        if (targetLight1 != null)
        {
            targetLight1.enabled = isLightEnabled;
        }
        if (targetLight2 != null)
        {
            targetLight2.enabled = isLightEnabled;
        }
        if (targetLight3 != null)
        {
            targetLight3.enabled = isLightEnabled;
        }
        if (targetLight4 != null)
        {
            targetLight4.enabled = isLightEnabled;
        }
        state1.SetActive(isLightEnabled);
        state2.SetActive(!isLightEnabled);
    }

    void ToggleLight()
    {
        isLightEnabled = !isLightEnabled;
    }
}
