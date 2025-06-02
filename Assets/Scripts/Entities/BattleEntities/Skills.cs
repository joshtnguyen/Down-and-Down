using System.Collections;
using System.Collections.Generic;
using System.Numerics; 
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

        var rand = new System.Random();
        
        c.verifyMod();
        if (t != null) 
            t.verifyMod();

        int dmg = 0;
        Mod m1;
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
                m1 = new Mod(s.skillName, c.ToString(), 200, 1, false);
                m2 = new Mod(s.skillName, c.ToString(), -5, 1, false);
                ModHandler.UpdateOrAddMod(c.def_p_mod, m1);
                ModHandler.UpdateOrAddMod(c.spd_mod, m2);
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
                m1 = new Mod(s.skillName, c.ToString(), 50, 3, false);
                m2 = new Mod(s.skillName, c.ToString(), 20 + (2.5 * s.stacks), 3, false);
                ModHandler.UpdateOrAddMod(c.cr_mod, m1);
                ModHandler.UpdateOrAddMod(c.cd_mod, m2);
                c.skipTurn = true;
                break;
            case "Fist to the Face":
                dmg = c.damage(t, 125 + (7 * s.stacks));
                break;
            case "Taunt":
                m1 = new Mod(s.skillName, c.ToString(), c.currentDEF * (0.08 + 0.02 * s.stacks) * -1, 1, true);
                ModHandler.UpdateOrAddMod(t.def_mod, m1);
                break;
            case "Gym Bro":
                m1 = new Mod(s.skillName, c.ToString(), t.baseHP * (0.005 + 0.002 * s.stacks), 2, false);
                m2 = new Mod(s.skillName, c.ToString(), t.baseATK * (0.005 + 0.002 * s.stacks), 2, false);
                m3 = new Mod(s.skillName, c.ToString(), t.baseDEF * (0.005 + 0.002 * s.stacks), 2, false);
                m4 = new Mod(s.skillName, c.ToString(), t.baseSPD * (0.005 + 0.002 * s.stacks), 2, false);
                m5 = new Mod(s.skillName, c.ToString(), t.baseCR * (0.005 + 0.002 * s.stacks), 2, false);
                m6 = new Mod(s.skillName, c.ToString(), t.baseCD * (0.005 + 0.002 * s.stacks), 2, false);
                ModHandler.UpdateOrAddMod(c.hp_mod, m1);
                ModHandler.UpdateOrAddMod(c.atk_mod, m2);
                ModHandler.UpdateOrAddMod(c.def_mod, m3);
                ModHandler.UpdateOrAddMod(c.spd_mod, m4);
                ModHandler.UpdateOrAddMod(c.cr_mod, m5);
                ModHandler.UpdateOrAddMod(c.cd_mod, m6);
                break;
            case "High Five":
                t.health += 10 + (2 * s.stacks);
                break;
            case "Motivating Touch":
                m1 = new Mod(s.skillName, c.ToString(), 15 + (2.5 * s.stacks), 1, false);
                ModHandler.UpdateOrAddMod(t.atk_p_mod, m1);
                break;
            case "Cheer":
                m1 = new Mod(s.skillName, c.ToString(), 5 + (0.75 * s.stacks), 1, false);
                ModHandler.UpdateOrAddMod(t.atk_p_mod, m1);
                break;
            case "Protein Shake":
                m1 = new Mod(s.skillName, c.ToString(), 8 + (2 * s.stacks), 1, false);
                ModHandler.UpdateOrAddMod(t.cd_mod, m1);
                break;
            case "Loving Meal":
                t.health += (int)(c.maxhealth * (0.15 + 0.0175 * s.stacks));
                break;
            case "Dedicated Meal":
                t.health += (int)(t.maxhealth * (0.12 + 0.02 * s.stacks));
                break;
            case "Energy Bar":
                m1 = new Mod(s.skillName, c.ToString(), 1 + s.stacks, 1, false);
                ModHandler.UpdateOrAddMod(t.spd_mod, m1);
                t.health += (int)(c.maxhealth * (0.06 + 0.015 * s.stacks));
                break;
            case "Life Saving Measures":
                if ((rand.NextDouble() * 100.0) < (5 + s.stacks)) {
                    t.health = (int)(t.maxhealth * 0.2);
                }
                break;

            case "Goo Shot":
                dmg = c.damage(t, 120);
                m1 = new Mod(s.skillName, c.ToString(), -2 - s.stacks, 1, true);
                m2 = new Mod(s.skillName, c.ToString(), -20 - (2 * s.stacks), 1, true);
                ModHandler.UpdateOrAddMod(t.spd_mod, m1);
                ModHandler.UpdateOrAddMod(t.def_p_mod, m2);
                break;

            case "Drunken Charge":
                dmg = c.damage(t, 35 + (3 * s.stacks));
                break;

            case "Sand Spray":
                dmg = c.damage(t, 90 + (3 * s.stacks));
                m1 = new Mod(s.skillName, c.ToString(), -2 - (1 * s.stacks), 0, true);
                ModHandler.UpdateOrAddMod(t.atk_p_mod, m1);
                break;
            case "Vampiric Gnaw":
                dmg = c.damage(t, 100);
                c.health += (int) (dmg * (0.5 + (0.05 * s.stacks)));
                break;
            case "Hammer Down":
                m1 = new Mod(s.skillName, c.ToString(), -10 - (2 * s.stacks), 0, true);
                ModHandler.UpdateOrAddMod(t.def_p_mod, m1);
                dmg = c.damage(t, 150 + (8 * s.stacks));
                break;
            case "Mysterious Dew-hickey":
                m1 = new Mod(s.skillName, c.ToString(), -8 - (2 * s.stacks), 1, true);
                m2 = new Mod(s.skillName, c.ToString(), -12 - (1.5 * s.stacks), 1, true);
                m3 = new Mod(s.skillName, c.ToString(), -1 - (0.5 * s.stacks), 1, true);
                ModHandler.UpdateOrAddMod(t.atk_p_mod, m1);
                ModHandler.UpdateOrAddMod(t.def_p_mod, m2);
                ModHandler.UpdateOrAddMod(t.spd_mod, m3);
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
        if (t != null) {
            t.verifyMod();
            if (dmg > 0) {
                if ((rand.NextDouble() * 100.0) < c.effect_bleed) {
                    m1 = new Mod("Bleed", c.ToString(), Game.getDisruption("Bleed").stacks * 0.05 * c.currentATK, 1, true);
                    ModHandler.UpdateOrAddMod(t.bleed, m1);
                    
                }
                if ((rand.NextDouble() * 100.0) < c.effect_freeze) {
                    m1 = new Mod("Freeze", c.ToString(), 100, 0, true);
                    ModHandler.UpdateOrAddMod(t.freeze, m1);
                }
            }
            t.verifyMod();
        }
        
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
