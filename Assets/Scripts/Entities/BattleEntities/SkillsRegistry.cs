using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsRegistry
{

    public static List<Skills> registry = new List<Skills>();
    public static List<Skills> registry_E = new List<Skills>();

    public static void firstRun() {
        // Player Skills
        registry.Add(new Skills("Attack", "Deals 100% of the user's attack on a single target. Recovers 1 SP.", 0, -1, -1, "Enemy", null));
        registry.Add(new Skills("Block", "Increases DEF % by 200% and reduces SPD by their base SPD for 1 turn.", 0, -1, -1, "Self", null));
        registry.Add(new Skills("Heavy Slice", "Deals 150% (+8% per stack) of the user's attack on a single target.", 2, 0, -1, "Enemy", "Walter"));
        registry.Add(new Skills("Multi Slice", "Deals 80% of the user's attack on a single target. There is a base 150% (+15% per stack) chance of attacking again.", 3, 0, -1, "Enemy", "Walter"));
        registry.Add(new Skills("Axe Swing", "Increases the user's CRIT RATE by 50% and CRIT DMG by 20% (+2.5% per stack) for 3 turns but be unable to move for 1 turn.", 2, 0, -1, "Self", "Walter"));
        registry.Add(new Skills("Fist to the Face", "Deals 125% (+7% per stack) of the user's attack on a single target.", 2, 0, -1, "Enemy", "Benedict"));
        registry.Add(new Skills("Taunt", "Decreases a single target's DEF by 8% (+2% per stack) of the user's DEF for 2 turns.", 2, 0, -1, "Enemy", "Benedict"));
        registry.Add(new Skills("Gym Bro", "Adds 0.5% (+0.2% per stack) of each base stat of a single ally for 2 turns.", 4, 0, -1, "Ally", "Benedict"));
        registry.Add(new Skills("High Five", "Heals an ally for 10 HP (+2 HP per stack).", 1, 0, -1, "Ally", "Benedict"));
        registry.Add(new Skills("Motivating Touch", "Increases an ally's ATK by 15% (+2.5% per stack) for 2 turns.", 2, 0, -1, "Ally", "Sherri"));
        registry.Add(new Skills("Cheer", "Increases all allies ATK by 5% (+0.75% per stack) for 2 turns.", 4, 0, -1, "Team", "Sherri"));
        registry.Add(new Skills("Protein Shake", "Increases one ally's CRIT DMG by 8% (+2% per stack) for 2 turns.", 2, 0, -1, "Ally", "Sherri"));
        registry.Add(new Skills("Loving Meal", "Heals an ally by 15% (+1.75% per stack) of the user's Max HP.", 2, 0, -1, "Ally", "Jade"));
        registry.Add(new Skills("Dedicated Meal", "Heals an ally by 12% (+2% per stack) of the ally's Max HP.", 2, 0, -1, "Ally", "Jade"));
        registry.Add(new Skills("Energy Bar", "Heals an ally by 6% (+1.5% per stack) of the user's Max HP and increases SPD by 1 (+1 per stack) for 2 turns.", 3, 0, -1, "Ally", "Jade"));
        
        // Enemy Skills
        registry_E.Add(new Skills("Goo Shot", "Deals 120% of the user's attack, decreases SPD by 2 (+1 per stack), and decreases DEF by 20% (+2% per stack) on a single target for 2 turns.", 2, 0, -1, "Enemy", "Slime"));
        registry_E.Add(new Skills("Drunken Charge", "Deals 35% (+3% per stack) of the user's attack to all enemies.", 3, 0, -1, "Enemies", "Cave Bull"));
        registry_E.Add(new Skills("Sand Spray", "Deals 130% of the user's attack and decreases ATK by 2% (+1% per stack) to all enemies for 1 turn.", 2, 0, -1, "Enemies", "Sandbat"));
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

        foreach (Skills s in registry_E) {
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
