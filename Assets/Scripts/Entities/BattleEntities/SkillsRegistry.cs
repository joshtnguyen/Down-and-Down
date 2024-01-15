using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsRegistry
{

    public static List<Skills> registry = new List<Skills>();

    public static void firstRun() {
        registry.Add(new Skills("Attack", "Deals 100% of the player's attack on a single target. Recovers 1 SP.", 0, 0, -1, "Enemy", null));
        registry.Add(new Skills("Block", "Increases DEF % by 200% and reduces SPD by their base SPD for 1 turn.", 0, 0, -1, "Self", null));
        registry.Add(new Skills("Heavy Slice", "Deals 150% (+8% per stack) of the player's attack on a single target.", 2, 1, -1, "Enemy", "Walter"));
        registry.Add(new Skills("Multi Slice", "Deals 80% of the player's attack on a single target. There is a base 150% (+15% per stack) chance of attacking again.", 2, 1, -1, "Enemy", "Walter"));
        registry.Add(new Skills("Axe Swing", "Increases the player's CRIT DMG by 20% (+2.5% per stack) for 2 turns but be unable to move for 1 turn.", 2, 1, -1, "Self", "Walter"));
        registry.Add(new Skills("Fist to the Face", "Deals 125% (+7% per stack) of the player's attack on a single target.", 2, 1, -1, "Enemy", "Benedict"));
        registry.Add(new Skills("Taunt", "Deals 125% (+7% per stack) of the player's attack on a single target.", 2, 1, -1, "Enemy", "Benedict"));
        registry.Add(new Skills("Gym Bro", "Adds 0.5% (+0.2% per stack) of each base stat of a single ally for 2 turns.", 4, 1, -1, "Ally", "Benedict"));
        registry.Add(new Skills("High Five", "Heals an ally for 10 HP (+2 HP per stack).", 1, 1, -1, "Ally", "Benedict"));
        registry.Add(new Skills("Motivating Touch", "Increases an ally's ATK by 15% (+2.5% per stack).", 2, 1, -1, "Ally", "Sherri"));
        registry.Add(new Skills("Cheer", "Increases all allies ATK by 8% (+2% per stack).", 2, 1, -1, "Team", "Sherri"));
        registry.Add(new Skills("Protein Shake", "Increases one ally's CRIT DMG by 8% (+2% per stack).", 2, 1, -1, "Ally", "Sherri"));
        registry.Add(new Skills("Loving Meal", "Heals an ally by 15% (+1.75% per stack) of the user's Max HP.", 2, 1, -1, "Ally", "Jade"));
        registry.Add(new Skills("Dedicated Meal", "Heals an ally by 12% (+2% per stack) of the ally's Max HP.", 2, 1, -1, "Ally", "Jade"));
        registry.Add(new Skills("Energy Bar", "Heals an ally by 6% (+1.5% per stack) of the user's Max HP and increases SPD by 1 (+1 per stack).", 3, 1, -1, "Ally", "Jade"));

    }

    public static Skills getSkill(List<Skills> skills, string skill) {
        foreach (Skills s in skills) {
            if (s.skillName == skill) {
                return s;
            }
        }
        return null;
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
