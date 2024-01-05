using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills
{

    public string skillName;
    public string description;
    public int spConsumption;
    public int stacks;
    public string targetType;

    public Skills(string n, string d, int c, int s, string t) {
        skillName = n;
        description = d;
        spConsumption = c;
        stacks = s;
        targetType = t;
    }

    public static int useSkill(Character c, Skills s) {
        int dmg = 0;
        switch (s.skillName) {
            case "Block":
                Mod m = new Mod(s.skillName, 200, false);
                Mod m2 = new Mod(s.skillName, c.baseSPD * -1, false);
                c.def_p_mod[0].Add(m);
                c.spd_mod[0].Add(m2);
                break;

        }
        c.verifyMod();
        return dmg;
    }

    public static int useSkill(Character c, Skills s, Character t) {
        int dmg = 0;
        switch (s.skillName) {
            case "Attack":
                dmg = c.damage(t, 100, false);
                break;
        }
        c.verifyMod();
        t.verifyMod();
        return dmg;
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
