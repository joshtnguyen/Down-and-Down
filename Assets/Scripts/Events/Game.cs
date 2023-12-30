using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public static bool gameMovementFreeze = false;

    public static string gameEvent = "Roaming";

    // Start is called before the first frame update
    void Start()
    {
        gameEvent = "Roaming";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
