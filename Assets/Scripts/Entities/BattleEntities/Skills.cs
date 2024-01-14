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

    public Skills(string n, string d, int c, int s, string t) {
        skillName = n;
        description = d;
        spConsumption = c;
        stacks = s;
        maxStacks = 1;
        targetType = t;
    }

    public Skills(string n, string d, int c, int s, int m, string t) {
        skillName = n;
        description = d;
        spConsumption = c;
        stacks = s;
        maxStacks = m;
        targetType = t;
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
                dmg = c.damage(t, 100, false);
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
            case "Test":
                m = new Mod(s.skillName, -50, true);
                t.atk_p_mod[0].Add(m);
                t.atk_p_mod[1].Add(m);
                break;
            case "Ally Thing":
                dmg = c.damage(t, 300, false);
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
