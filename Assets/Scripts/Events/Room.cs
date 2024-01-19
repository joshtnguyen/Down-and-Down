using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

    public string roomType;
    public bool hasEntered;
    public int enemiesLeft;
    public int wishes = 0;
    public bool north = false;
    public bool east = false;
    public bool south = false;
    public bool west = false;
    public Skills[] skillList = new Skills[4];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetSkills() {
        List<int> temp = new List<int>();
        for (int i = 2; i < SkillsRegistry.registry.Count; i++) {
            if (SkillsRegistry.registry[i].maxStacks >= 0) {
                Skills testSkill = SkillsRegistry.getSkill(Character.GetCharacter(SkillsRegistry.registry[i].skillUser).skills, SkillsRegistry.registry[i].skillName);
                if (testSkill.stacks < SkillsRegistry.registry[i].maxStacks) {
                    temp.Add(i);
                }
            } else {
                temp.Add(i);
            }
        }

        for (int i = 0; i < 4; i++) {
            int chosenSkill = Random.Range(0, temp.Count);
            skillList[i] = SkillsRegistry.registry[temp[chosenSkill]];
            temp.RemoveAt(chosenSkill);
        }
    }

    public void Reset() {
        hasEntered = false;
        north = false;
        east = false;
        south = false;
        west = false;
        roomType = null;
        enemiesLeft = 0;
        wishes = 0;
        for (int i = 0; i < 4; i++) {
            skillList[i] = null;
        }
    }

    public override string ToString() {
        if (!north && !east && !south && !west) {
            return null;
        }
        return "{ TYPE: " + roomType + ", N: " + north + ", E: " + east + ", W: " + west + ", S: " + south + " }";
    }

}
