using System.Collections;
using System.Collections.Generic;
using System.Numerics;

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
        Mod m5;
        Mod m6;

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
                m = new Mod(s.skillName, 50, false);
                m2 = new Mod(s.skillName, 20 + (2.5 * s.stacks), false);
                c.cr_mod[1].Add(m);
                c.cr_mod[2].Add(m);
                c.cr_mod[3].Add(m);
                c.cd_mod[1].Add(m2);
                c.cd_mod[2].Add(m2);
                c.cd_mod[3].Add(m2);
                c.skipTurn = true;
                break;
            case "Fist to the Face":
                dmg = c.damage(t, 125 + (7 * s.stacks));
                break;
            case "Taunt":
                m = new Mod(s.skillName, c.currentDEF * (0.08 + 0.02 * s.stacks) * -1, true);
                t.def_mod[0].Add(m);
                t.def_mod[1].Add(m);
                break;
            case "Gym Bro":
                m = new Mod(s.skillName, t.baseHP * (0.005 + 0.002 * s.stacks), false);
                m2 = new Mod(s.skillName, t.baseATK * (0.005 + 0.002 * s.stacks), false);
                m3 = new Mod(s.skillName, t.baseDEF * (0.005 + 0.002 * s.stacks), false);
                m4 = new Mod(s.skillName, t.baseSPD * (0.005 + 0.002 * s.stacks), false);
                m5 = new Mod(s.skillName, t.baseCR * (0.005 + 0.002 * s.stacks), false);
                m6 = new Mod(s.skillName, t.baseCD * (0.005 + 0.002 * s.stacks), false);
                for (int i = 1; i <= 2; i++) {
                    c.hp_mod[i].Add(m);
                    c.atk_mod[i].Add(m2);
                    c.def_mod[i].Add(m3);
                    c.spd_mod[i].Add(m4);
                    c.cr_mod[i].Add(m5);
                    c.cd_mod[i].Add(m6);
                }
                break;
            case "High Five":
                t.health += 10 + (2 * s.stacks);
                break;
            case "Motivating Touch":
                m = new Mod(s.skillName, 15 + (2.5 * s.stacks), false);
                t.atk_p_mod[0].Add(m);
                t.atk_p_mod[1].Add(m);
                break;
            case "Cheer":
                m = new Mod(s.skillName, 5 + (0.75 * s.stacks), false);
                c.atk_p_mod[0].Add(m);
                c.atk_p_mod[1].Add(m);
                break;
            case "Protein Shake":
                m = new Mod(s.skillName, 8 + (2 * s.stacks), false);
                t.cd_mod[0].Add(m);
                t.cd_mod[1].Add(m);
                break;
            case "Loving Meal":
                t.health += (int)(c.maxhealth * (0.15 + 0.0175 * s.stacks));
                break;
            case "Dedicated Meal":
                t.health += (int)(t.maxhealth * (0.12 + 0.02 * s.stacks));
                break;
            case "Energy Bar":
                m = new Mod(s.skillName, 1 + s.stacks, false);
                t.spd_mod[0].Add(m);
                t.spd_mod[1].Add(m);
                t.health += (int)(c.maxhealth * (0.06 + 0.015 * s.stacks));
                break;

            case "Goo Shot":
                dmg = c.damage(t, 120);
                m = new Mod(s.skillName, -2 - s.stacks, true);
                m2 = new Mod(s.skillName, -20 - (2 * s.stacks), true);
                t.spd_mod[0].Add(m);
                t.spd_mod[1].Add(m);
                t.def_p_mod[0].Add(m2);
                t.def_p_mod[1].Add(m2);
                break;

            case "Drunken Charge":
                dmg = c.damage(t, 35 + (3 * s.stacks));
                break;

            case "Sand Spray":
                dmg = c.damage(t, 90 + (3 * s.stacks));
                m = new Mod(s.skillName, -2 - (1 * s.stacks), true);
                t.atk_p_mod[1].Add(m);
                break;
            case "Vampiric Gnaw":
                dmg = c.damage(t, 100);
                c.health += (int) (dmg * (0.5 + (0.05 * s.stacks)));
                break;
            case "Hammer Down":
                m = new Mod(s.skillName, -10 - (2 * s.stacks), true);
                t.def_p_mod[0].Add(m);
                dmg = c.damage(t, 150 + (8 * s.stacks));
                break;
            case "Mysterious Dew-hickey":
                m = new Mod(s.skillName, -8 - (2 * s.stacks), true);
                m2 = new Mod(s.skillName, -12 - (1.5 * s.stacks), true);
                m3 = new Mod(s.skillName, -1 - (0.5 * s.stacks), true);
                t.atk_p_mod[0].Add(m);
                t.def_p_mod[0].Add(m2);
                t.spd_mod[0].Add(m3);
                t.atk_p_mod[1].Add(m);
                t.def_p_mod[1].Add(m2);
                t.spd_mod[1].Add(m3);
                break;
            case "Skilln't":
                dmg = c.damage(t, 65 + (8 * s.stacks));
                int skillLost = (int) (0.25 * s.stacks);
                skillLost += 1;
                Battle.battleSP -= skillLost;
                if (Battle.battleSP < 0) {
                    Battle.battleSP = 0;
                }
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
