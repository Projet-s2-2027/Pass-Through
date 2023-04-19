
using UnityEngine;
using UnityEngine.UI;

public class KillfeedItem : MonoBehaviour
{
    [SerializeField] 
    private Text text;

    
    public void Setup(string player, string source)
    {
        text.text = source + " killed " + player;
    }
}
