using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills
{

    public string skillName;
    public string description;
    public int spConsumption;
    public int stacks;
    public int maxStacks;
    public string targetType;
    public string skillUser;

    public Skills(string n, string d, int c, int s, string t) {
        skillName = n;
        description = d;
        spConsumption = c;
        stacks = s;
        maxStacks = 1;
        targetType = t;
        skillUser = null;
    }

    public Skills(string n, string d, int c, int s, int m, string t, string u) {
        skillName = n;
        description = d;
        spConsumption = c;
        stacks = s;
        maxStacks = m;
        targetType = t;
        skillUser = u;
    }

    public static int useSkill(Character c, Skills s, Character t) {

        c.verifyMod();
        if (t != null) 
            t.verifyMod();

        int dmg = 0;
        Mod m;
        Mod m2;
        Mod m3;
        Mod m4;

        if (c.isEnemy()) {
            c.sp -= s.spConsumption;
        } else {
            Battle.battleSP -= s.spConsumption;
        }

        switch (s.skillName) {
            case "Attack":
                dmg = c.damage(t, 100);
                if (c.isEnemy()) {
                    c.sp += s.spConsumption;
                } else {
                    Battle.battleSP += 1;
                }
                break;
            case "Block":
                m = new Mod(s.skillName, 200, false);
                m2 = new Mod(s.skillName, c.baseSPD * -1, false);
                c.def_p_mod[1].Add(m);
                c.spd_mod[1].Add(m2);
                break;
            case "Heavy Slice":
                dmg = c.damage(t, 150 + (8 * s.stacks));
                break;
            case "Multi Slice":
                for (int i = 0; i < convertToLoops(150 + (15 * s.stacks)); i++) {
                    dmg += c.damage(t, 80);
                }
                break;
            case "Axe Swing":
                m = new Mod(s.skillName, 20 + (2.5 * s.stacks), false);
                c.cd_mod[1].Add(m);
                c.cd_mod[2].Add(m);
                break;
            case "Fist to the Face":
                dmg = c.damage(t, 125 + (7 * s.stacks));
                break;
            case "Taunt":
                m = new Mod(s.skillName, c.currentDEF * (0.08 + 0.02 * s.stacks) * -1, true);
                t.def_mod[1].Add(m);
                t.def_mod[2].Add(m);
                break;
        }

        c.verifyMod();
        if (t != null) 
            t.verifyMod();
        
        return dmg;
    }

    public static int useSkill(Character c, Skills s) {
        return useSkill(c, s, null);
    }

    public static int convertToLoops(double chance) {
        var rand = new System.Random();
        int times = 0;
        while (chance >= 100) {
            chance -= 100;
            times++;
        }
        if ((rand.NextDouble() * 100.0) <= chance) {
            times++;
        }
        return times;
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
