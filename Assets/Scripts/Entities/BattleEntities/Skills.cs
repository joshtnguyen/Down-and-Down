using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{

    public string skillName;
    public string description;
    public int spConsumption;
    public int stacks;

    public Skills(string n, string d, int c, int s) {
        skillName = n;
        description = d;
        spConsumption = c;
        stacks = s;
    }

    public void use(Character c, EnemyAI e) {
        switch(skillName) {
            case "Attack":
                break;
        }
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
