using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager instance;
    public string lobbySceneName = "Lobby";

    public static string LoggedIdUsername;
    private void Awake(){
        
        if (instance != null ){
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(this);
    }

    public void LogIn(Text username){
        LoggedIdUsername = username.text;
        SceneManager.LoadScene(lobbySceneName);

    }
 
}
