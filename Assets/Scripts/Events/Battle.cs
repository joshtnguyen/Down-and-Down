using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{

    public static void StartBattle() {
        Game.gameEvent = "Battle";
        Game.gameMovementFreeze = true;
        SceneManager.LoadScene("Battle Scene");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
