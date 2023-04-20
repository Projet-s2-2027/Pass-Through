
using System;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform thrusterFuellFill;

    [SerializeField]
    private RectTransform healthBarFill;
    
    private PlayerController controller;
    private Player player;
    [SerializeField] 
    private GameObject pauseMenu;
    [SerializeField] 
    private GameObject scoreBoard;

    public void SetPlayer(Player _player)
    {
        player = _player;
        controller = player.GetComponent<PlayerController>();
    }

    private void Start()
    {
        PauseMenu.isOn = false;
    }

    private void Update()
    {
        SetFuelAmount(controller.GetThrusterFuelAmount());
        SetHealthAmount(player.GetHealthPct());
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            scoreBoard.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            scoreBoard.SetActive(false);
        }
        
        
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.isOn = pauseMenu.activeSelf;
    }


    void SetFuelAmount(float _amount)
    {
        thrusterFuellFill.localScale = new Vector3(1f, _amount, 1f);
    }
    void SetHealthAmount(float _amount)
    {
        healthBarFill.localScale = new Vector3(1f, _amount, 1f);
    }
}
