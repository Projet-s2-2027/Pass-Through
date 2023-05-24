using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public GameObject button;
    public Light targetLight;

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
        if (other.gameObject == button)
        {
            isLookingAtButton = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == button)
        {
            isLookingAtButton = false;
        }
    }

    void ToggleLight()
    {
        isLightEnabled = !isLightEnabled;
        targetLight.enabled = isLightEnabled;
        isState1Active = !isState1Active;
        state1.SetActive(isState1Active);
        state2.SetActive(!isState1Active);
    }
}
