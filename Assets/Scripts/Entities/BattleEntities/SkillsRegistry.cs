using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsRegistry
{

    public static List<Skills> registry = new List<Skills>();

    public static void firstRun() {
        registry.Add(new Skills("Attack", "Deals 100% of the player's attack on a single target.", 0, 0, "Enemy"));
        registry.Add(new Skills("Block", "Increases DEF % by 200% and reduces SPD by their base SPD for 1 turn.", 0, 0, "Self"));
        registry.Add(new Skills("Test", "Decreases an enemy's ATK % by 50% for 2 turns.", 2, 3, "Enemy"));
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
