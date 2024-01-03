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
                if (s == skill.skillName) {
                    desc = skill.description;
                    if (skill.stacks > 0)
                        s += " (x" + skill.stacks + ")";
                    if (skill.spConsumption > 0)
                        s += " - " + skill.spConsumption + " SP"; 
                }
            }
        }

        string r = s + "\n" + desc + "\n";
        return r;
    }

    
}
