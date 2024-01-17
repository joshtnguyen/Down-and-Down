using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{

    public Text textBox;
    public string text;
    public static int moveSpeed = 500;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textBox.text = text;
    }

    public static string convertSkillDescription(Character c, string s) {
        if (s.IndexOf("Skill: ")  >= 0) {
            s = s.Substring(7);
        }

        string desc = "[NO DESCRIPTION FOUND]";
        if (c.skills.Any()) {
            foreach (Skills skill in c.skills) {
                if (skill != null) {
                    if (s == skill.skillName) {
                        desc = skill.description;
                        if (skill.stacks > -1)
                            s += " (x" + skill.stacks + ")";
                        if (skill.spConsumption > 0)
                            s += " - " + skill.spConsumption + " SP"; 
                    }
                }
            }
        }

        string r = s + "\n" + desc + "\n";
        return r;
    }

    public static string convertSkillShop(Skills s) {

        Character ch = Character.GetCharacter(s.skillUser);

        Skills skill = null;

        if (ch.skills.Any()) {
            foreach (Skills sk in ch.skills) {
                if (sk.skillName == s.skillName) {
                    skill = sk;
                }
            }
        }

        string st = s.skillUser + " Skill";
        string desc = s.description;
        int cost = getSkillCost(s);
        if (skill != null) {
            st += " (x" + (skill.stacks + 1) + ")";
        } else {
            st += " (x" + s.stacks + ")";
        }
        if (s.spConsumption > 0) {
            st += " - " + s.spConsumption + " SP"; 
        }
        st += " - " + cost + "G";

        string r = st + "\n" + desc + "\n";
        return r;
    }

    public static int getSkillCost(Skills s) {
        Character ch = Character.GetCharacter(s.skillUser);

        Skills skill = null;

        foreach (Skills sk in ch.skills) {
            if (sk.skillName == s.skillName) {
                skill = sk;
            }
        }
        int cost = 1;
        if (skill != null) {
            cost = skill.stacks + 2;
        }

        return cost * 3;
    }

    
}
