using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

    public string roomType;
    public bool hasEntered;
    public int enemiesLeft;
    public bool north = false;
    public bool east = false;
    public bool south = false;
    public bool west = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset() {
        hasEntered = false;
        north = false;
        east = false;
        south = false;
        west = false;
        roomType = null;
        enemiesLeft = 0;
    }

    public override string ToString() {
        if (!north && !east && !south && !west) {
            return null;
        }
        return "{ TYPE: " + roomType + ", N: " + north + ", E: " + east + ", W: " + west + ", S: " + south + " }";
    }

}
