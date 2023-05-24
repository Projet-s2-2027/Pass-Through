using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public GameObject button;
    public Light targetLight1;
    public Light targetLight2;
    public Light targetLight3;
    public Light targetLight4;

    private bool isLookingAtButton = false;
    private bool isLightEnabled = true;
    private bool isState1Active = true;
    public GameObject state1;
    public GameObject state2;


    void Update()
    {
        if (isLookingAtButton && Input.GetKeyDown(interactKey))
        {
            ToggleLight();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        isLookingAtButton = true;  
    }

    void OnTriggerExit()
    {
        isLookingAtButton = false;
    }

    void ToggleLight()
    {
        isLightEnabled = !isLightEnabled;
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
        isState1Active = !isState1Active;
        state1.SetActive(isState1Active);
        state2.SetActive(!isState1Active);
    }
}
