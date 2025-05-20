using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour
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
        results.Add("Floors Climbed: " + Game.floorNumber);
        results.Add(" ");
        foreach (Character c in Game.characters) {
            results.Add(c.character + ": Level " + c.level);
        }

        string finalResults = "";
        foreach (string s in results) {
            finalResults += s + "\n";
        }
        Results.text = finalResults;
    }

}
