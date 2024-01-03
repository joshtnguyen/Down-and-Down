using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    public static bool gameMovementFreeze = false;

    public static string gameEvent = "Roaming";

    public GameObject originalEnemy;

    public static int floorNumber = 100;

    // Start is called before the first frame update
    void Start()
    {
        gameEvent = "Roaming";
        EnemyAI.CreateEnemy(originalEnemy, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
