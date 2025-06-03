using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebriefUI : MonoBehaviour
{

    public Text Results;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        List<string> results = new List<string>();
        results.Add("Steps Taken: " + Game.steps);
        results.Add("Enemies Defeated: " + Game.enemiesKilled);
        results.Add("Times Allies Downed: " + Game.timesDowned);

        string finalResults = "";
        foreach (string s in results) {
            finalResults += s + "\n";
        }
        Results.text = finalResults;

        if (getConfirmation() != 0) {
            Game.createFloor();
            Game.gameMovementFreeze = false;
            Game.floorNumber++;
            SceneManager.LoadScene("Overworld Scene");
        }
    }

    private int getConfirmation() {
        if (Input.GetKeyDown(KeyCode.C)) {
            return 1;
        } else if (Input.GetKeyDown(KeyCode.X)) {
            return -1;
        } else {
            return 0;
        }
    }

}
