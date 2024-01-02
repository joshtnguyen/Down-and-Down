using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsRegistry
{

    public static List<Skills> registry = new List<Skills>();

    public static void firstRun() {
        registry.Add(new Skills("Attack", "A basic attack that deals 100% of the player's attack on a single target.", 0, 0));
        registry.Add(new Skills("Items", "Use an item to help you and your allies with battle!", 0, 0));
        registry.Add(new Skills("Test", "Test skill. JTN is a pro gamer tho.", 2, 3));
    }

    public static Skills getSkill(string skill) {
        foreach (Skills s in registry) {
            if (s.skillName == skill) {
                return s;
            }
        }
        return null;
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
