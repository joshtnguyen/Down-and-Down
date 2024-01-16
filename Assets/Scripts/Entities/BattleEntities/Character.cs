using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using System.Linq;
using Unity.VisualScripting;
using System;

public class Mod {
    public string modName;
    public double value;
    public bool isDebuff;

    public Mod(string modName, double value, bool isDebuff) {
        this.modName = modName;
        this.value = value;
        this.isDebuff = isDebuff;
    }

    public override string ToString() {
        return "{ name: " + modName + ", value: " + value + ", isDebuff: " + isDebuff + " }";
    }

}

public class Character
{

    private static int L_UP_REQ = 5;

    private static int L_UP_HP = 3;
    private static int L_UP_ATK = 2;
    private static int L_UP_DEF = 4;

    public string character;

    public int level;
    public int xp;
    public int sp;

    public int health;
    public int maxhealth;
    public int baseHP;
    public List<List<Mod>> hp_mod = new List<List<Mod>>();
    public List<List<Mod>> hp_p_mod = new List<List<Mod>>();

    public double currentATK;
    public double baseATK;
    public List<List<Mod>> atk_mod = new List<List<Mod>>();
    public List<List<Mod>> atk_p_mod = new List<List<Mod>>();

    public double currentDEF;
    public double baseDEF;
    public List<List<Mod>> def_mod = new List<List<Mod>>();
    public List<List<Mod>> def_p_mod = new List<List<Mod>>();

    public double currentSPD;
    public int baseSPD;
    public List<List<Mod>> spd_mod = new List<List<Mod>>();

    public double currentCR;
    public double baseCR;
    public List<List<Mod>> cr_mod = new List<List<Mod>>();

    public double currentCD;
    public double baseCD;
    public List<List<Mod>> cd_mod = new List<List<Mod>>();

    public List<Skills> skills = new List<Skills>();
    public Skills skill1;
    public Skills skill2;

    public Character(string name, int le, int hp, double atk, double def, int spd, double cr, double cd) {
        character = name;
        level = le;
        baseHP = hp;
        maxhealth = hp;
        health = hp;
        baseATK = atk;
        baseDEF = def;
        baseSPD = spd;
        baseCR = cr;
        baseCD = cd;
        skill1 = null;
        skill2 = null;
    }

    public void resetMod() {
        hp_mod.Clear();
        hp_p_mod.Clear();
        atk_mod.Clear();
        atk_p_mod.Clear();
        def_mod.Clear();
        def_p_mod.Clear();
        spd_mod.Clear();
        cr_mod.Clear();
        cd_mod.Clear();
    }

    public void endTurn() {
        
        if (hp_mod.Any()) {
            hp_mod.RemoveAt(0);
        }
        if (hp_p_mod.Any()) {
            hp_p_mod.RemoveAt(0);
        }
        if (atk_mod.Any()) {
            atk_mod.RemoveAt(0);
        }
        if (atk_p_mod.Any()) {
            atk_p_mod.RemoveAt(0);
        }
        if (def_mod.Any()) {
            hp_mod.RemoveAt(0);
        }
        if (def_p_mod.Any()) {
            def_p_mod.RemoveAt(0);
        }
        if (spd_mod.Any()) {
            spd_mod.RemoveAt(0);
        }
        if (cr_mod.Any()) {
            cr_mod.RemoveAt(0);
        }
        if (cd_mod.Any()) {
            cd_mod.RemoveAt(0);
        }
        
    }

    public int damage(Character ch, double multiplier, bool isMultiple) {

        double dmg = 0;

        if (ch != null) {
            verifyMod();
            ch.verifyMod();

            var rand = new System.Random();

            // DMG CALCULATION
            dmg = (double)(currentATK * (multiplier / 100.0));

            // CRIT MULTIPLIER
            double crit = 1;
            if ((rand.NextDouble() * 100.0) <= currentCR) {
                crit = currentCD;
            }

            // DEF MULTIPLIER
            double def = (double)(1 - (ch.currentDEF / (ch.currentDEF + 100 + (5 * ((double) ch.level)))));

            // INVOKE DMG
            dmg = dmg * crit * def;
            ch.health -= (int) dmg;
            
            if (ch.health <= 0) {
                ch.health = 0;
                for (int i = 0; i < Battle.cycle.Count; i++) {
                    if (Battle.cycle[i] == ch) {
                        Battle.cycle.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
        
        return (int) dmg;

    }

    public void verifyMod() {
        for (int i = 0; i < 6; i++) {
            if (hp_mod.Count < 5) {
                hp_mod.Add(new List<Mod>());
            }
            if (hp_p_mod.Count < 5) {
                hp_p_mod.Add(new List<Mod>());
            }
            if (atk_mod.Count < 5) {
                atk_mod.Add(new List<Mod>());
            }
            if (atk_p_mod.Count < 5) {
                atk_p_mod.Add(new List<Mod>());
            }
            if (def_mod.Count < 5) {
                def_mod.Add(new List<Mod>());
            }
            if (def_p_mod.Count < 5) {
                def_p_mod.Add(new List<Mod>());
            }
            if (spd_mod.Count < 5) {
                spd_mod.Add(new List<Mod>());
            }
            if (cr_mod.Count < 5) {
                cr_mod.Add(new List<Mod>());
            }
            if (cd_mod.Count < 5) {
                cd_mod.Add(new List<Mod>());
            }
        }
        
        double val = 0;
        double per = 100;

        val = baseHP + (L_UP_HP * (level-1));
        per = 100;
        if (hp_mod[0].Any()) {
            foreach (Mod m in hp_mod[0]) {
                if (m != null) {
                    val += m.value;
                }
            }
        }
        if (hp_p_mod[0].Any()) {
            foreach (Mod m in hp_p_mod[0]) {
                if (m != null) {
                    per += m.value;
                }
            }
        }
        maxhealth = (int)(val * (per / 100));

        val = baseATK + (L_UP_ATK * (level-1));
        per = 100;
        if (atk_mod[0].Any()) {
            foreach (Mod m in atk_mod[0]) {
                if (m != null) {
                    val += m.value;
                }
            }
        }
        if (atk_p_mod[0].Any()) {
            foreach (Mod m in atk_p_mod[0]) {
                if (m != null) {
                    per += m.value;
                }
            }
        }
        currentATK = val * (per / 100);

        val = baseDEF + (L_UP_DEF * (level-1));
        per = 100;
        if (def_mod[0].Any()) {
            foreach (Mod m in def_mod[0]) {
                if (m != null) {
                    val += m.value;
                }
            }
        }
        if (def_p_mod[0].Any()) {
            foreach (Mod m in def_p_mod[0]) {
                if (m != null) {
                    per += m.value;
                }
            }
        }
        currentDEF = val * (per / 100);

        val = baseSPD;
        per = 100;
        if (spd_mod[0].Any()) {
            foreach (Mod m in spd_mod[0]) {
                if (m != null) {
                    val += m.value;
                }
            }
        }
        currentSPD = val * (per / 100);

        val = baseCR;
        if (cr_mod[0].Any()) {
            foreach (Mod m in cr_mod[0]) {
                if (m != null) {
                    val += m.value;
                }
            }
        }
        currentCR = val * (per / 100);

        val = baseCD + 100;
        if (cd_mod[0].Any()) {
            foreach (Mod m in cd_mod[0]) {
                if (m != null) {
                    val += m.value;
                }
            }
        }
        currentCD = val / 100;

        if (health > maxhealth) {
            health = maxhealth;
        }

    }

    public int checkLevelChange() {
        int up = 0;
        while (xp >= L_UP_REQ * level) {
            xp -= L_UP_REQ * level;
            level++;
            up++;
        }
        return up;
    }

    public static void addSkill(Skills s) {
        Character c = GetCharacter(s.skillUser);
        bool hasAdded = false;
        foreach (Skills skill in c.skills) {
            if (s.skillName == skill.skillName) {
                skill.stacks += s.stacks + 1;
                hasAdded = true;
            }
        }

        if (!hasAdded) {
            c.skills.Add(s);
        }

        Debug.Log("==");
        foreach (Skills skill in c.skills) {
            Debug.Log(skill.skillName);
        }
        Debug.Log("==");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Character GetCharacter(string name) {
        foreach (Character c in Game.characters) {
            if (c.character == name) {
                return c;
            }
        }
        return null;
    }

    public virtual bool isEnemy() {
        return false; 
    }
    
    public virtual string getName() {
        return character; 
    }

    public override string ToString() {
        return "{ name: " + character + " }";
    }

}
