using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using System.Linq;

public class Mod {

    public string modName;
    public string user;
    public double value;
    public double duration;
    public bool isDebuff;

    public Mod(string modName, string user, double value, int duration, bool isDebuff) {
        this.modName = modName;
        this.user = user;
        this.value = value;
        this.duration = duration;
        this.isDebuff = isDebuff;
    }

    public override string ToString() {
        return "{ name: " + modName + ", user: " + user + ", value: " + value + ", duration: " + duration + ", isDebuff: " + isDebuff + " }";
    }

}

public class ModHandler {
    public static void UpdateOrAddMod(List<Mod> mods, Mod m) {
        foreach (var mod in mods) {
            if (mod.modName == m.modName && mod.user == m.user) {
                mod.duration = m.duration;
                return;
            }
        }
        mods.Add(m);
    }
}

public class Character
{

    public static int L_UP_REQ = 5;

    public static int L_UP_HP = 3;
    public static int L_UP_ATK = 2;
    public static int L_UP_DEF = 4;

    public string character;

    public int level;
    public int xp;
    public int sp;
    public double av = 0;

    public int health;
    public int maxhealth;
    public int baseHP;
    public double hp_ex;
    public double hp_p_ex;
    public List<Mod> hp_mod = new List<Mod>();
    public List<Mod> hp_p_mod = new List<Mod>();

    public double currentATK;
    public double baseATK;
    public double atk_ex;
    public double atk_p_ex;
    public List<Mod> atk_mod = new List<Mod>();
    public List<Mod> atk_p_mod = new List<Mod>();

    public double currentDEF;
    public double baseDEF;
    public double def_ex;
    public double def_p_ex;
    public List<Mod> def_mod = new List<Mod>();
    public List<Mod> def_p_mod = new List<Mod>();

    public double currentSPD;
    public int baseSPD;
    public double spd_ex;
    public List<Mod> spd_mod = new List<Mod>();

    public double currentCR;
    public double baseCR;
    public double cr_ex;
    public List<Mod> cr_mod = new List<Mod>();

    public double currentCD;
    public double baseCD;
    public double cd_ex;
    public List<Mod> cd_mod = new List<Mod>();

    public Gear gear = new Gear();

    public double effect_bleed = 0;
    public double effect_freeze = 0;
    public List<Mod> bleed = new List<Mod>();
    public List<Mod> freeze = new List<Mod>();

    public List<Skills> skills = new List<Skills>();
    public Skills skill1;
    public Skills skill2;

    public bool skipTurn;

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
        skipTurn = false;
    }

    public void printMod() {
        foreach (Mod m in hp_mod) {
            Debug.Log(m);
        }

        foreach (Mod m in hp_p_mod) {
            Debug.Log(m);
        }

        foreach (Mod m in atk_mod) {
            Debug.Log(m);
        }
        
        foreach (Mod m in atk_p_mod) {
            Debug.Log(m);
        }
        
        foreach (Mod m in def_mod) {
            Debug.Log(m);
        }

        foreach (Mod m in def_p_mod) {
            Debug.Log(m);
        }

        foreach (Mod m in spd_mod) {
            Debug.Log(m);
        }

        foreach (Mod m in cr_mod) {
            Debug.Log(m);
        }

        foreach (Mod m in cd_mod) {
            Debug.Log(m);
        }

        foreach (Mod m in bleed) {
            Debug.Log(m);
        }

        foreach (Mod m in freeze) {
            Debug.Log(m);
        }
    }

    public int startTurn() {

        //printMod();

        double dmg = 0;
        var rand = new System.Random();

        if (bleed.Any()) {
            foreach(Mod m in bleed) {
                dmg += m.value;
            }
        }
        if (freeze.Any()) {
            double freezeChance = 0;
            foreach(Mod m in freeze) {
                freezeChance += m.value;
            }
            if ((rand.NextDouble() * 100.0) < freezeChance) {
                Battle.sel_phase = -2;
            }
            if (skipTurn) {
                Battle.sel_phase = -2;
            }
        }
        health -= (int) dmg;
        return (int) dmg;
    }
    
    public void endTurn() {
        
        for (int i = hp_mod.Count() - 1; i >= 0; i--) {
            Mod m = hp_mod[i];
            if (m.duration <= 0) {
                hp_mod.Remove(m);
            }
            m.duration -= 1;
        }

        for (int i = hp_p_mod.Count() - 1; i >= 0; i--) {
            Mod m = hp_p_mod[i];
            if (m.duration <= 0) {
                hp_p_mod.Remove(m);
            }
            m.duration -= 1;
        }

        for (int i = atk_mod.Count() - 1; i >= 0; i--) {
            Mod m = atk_mod[i];
            if (m.duration <= 0) {
                atk_mod.Remove(m);
            }
            m.duration -= 1;
        }
        
        for (int i = atk_p_mod.Count() - 1; i >= 0; i--) {
            Mod m = atk_p_mod[i];
            if (m.duration <= 0) {
                atk_p_mod.Remove(m);
            }
            m.duration -= 1;
        }
        
        for (int i = def_mod.Count() - 1; i >= 0; i--) {
            Mod m = def_mod[i];
            if (m.duration <= 0) {
                def_mod.Remove(m);
            }
            m.duration -= 1;
        }

        for (int i = def_p_mod.Count() - 1; i >= 0; i--) {
            Mod m = def_p_mod[i];
            if (m.duration <= 0) {
                def_p_mod.Remove(m);
            }
            m.duration -= 1;
        }

        for (int i = spd_mod.Count() - 1; i >= 0; i--) {
            Mod m = spd_mod[i];
            if (m.duration <= 0) {
                spd_mod.Remove(m);
            }
            m.duration -= 1;
        }

        for (int i = cr_mod.Count() - 1; i >= 0; i--) {
            Mod m = cr_mod[i];
            if (m.duration <= 0) {
                cr_mod.Remove(m);
            }
            m.duration -= 1;
        }

        for (int i = cd_mod.Count() - 1; i >= 0; i--) {
            Mod m = cd_mod[i];
            if (m.duration <= 0) {
                cd_mod.Remove(m);
            }
            m.duration -= 1;
        }

        for (int i = bleed.Count() - 1; i >= 0; i--) {
            Mod m = bleed[i];
            if (m.duration <= 0) {
                bleed.Remove(m);
            }
            m.duration -= 1;
        }

        for (int i = freeze.Count() - 1; i >= 0; i--) {
            Mod m = freeze[i];
            if (m.duration <= 0) {
                freeze.Remove(m);
            }
            m.duration -= 1;
        }
    }

    public int damage(Character ch, double multiplier) {

        double dmg = 0;

        if (ch != null) {
            verifyMod();
            ch.verifyMod();

            var rand = new System.Random();

            // DMG CALCULATION
            dmg = (double)(currentATK * (multiplier / 100.0));

            // CRIT MULTIPLIER
            double crit = 1;
            if ((rand.NextDouble() * 100.0) < currentCR) {
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
        effect_bleed = gear.effect_bleed;
        effect_freeze = gear.effect_freeze;
        if (isEnemy()) {
            effect_bleed += Game.getDisruption("Bleed").stacks * 0.5;
            effect_freeze += Game.getDisruption("Freeze").stacks * 0.375;
        }
        
        double val = 0;
        double per = 100;

        val = baseHP + hp_ex + gear.hp + (L_UP_HP * (level-1));
        per = 100 + hp_p_ex + gear.hp_p;
        if (hp_mod.Any()) {
            foreach (Mod m in hp_mod) {
                if (m != null) {
                    val += m.value;
                }
            }

        }
        if (hp_p_mod.Any()) {
            foreach (Mod m in hp_p_mod) {
                if (m != null) {
                    per += m.value;
                }
            }
        }
        maxhealth = (int)(val * (per / 100));

        val = baseATK + atk_ex + gear.atk + (L_UP_ATK * (level-1));
        per = 100 + atk_p_ex + gear.atk_p;
        if (atk_mod.Any()) {
            foreach (Mod m in atk_mod) {
                if (m != null) {
                    val += m.value;
                }
            }
        }
        if (atk_p_mod.Any()) {
            foreach (Mod m in atk_p_mod) {
                if (m != null) {
                    per += m.value;
                }
            }
        }
        currentATK = val * (per / 100);

        val = baseDEF + def_ex + gear.def + (L_UP_DEF * (level-1));
        per = 100 + def_p_ex + gear.def_p;
        if (def_mod.Any()) {
            foreach (Mod m in def_mod) {
                if (m != null) {
                    val += m.value;
                }
            }
        }
        if (def_p_mod.Any()) {
            foreach (Mod m in def_p_mod) {
                if (m != null) {
                    per += m.value;
                }
            }
        }
        currentDEF = val * (per / 100);

        val = baseSPD + spd_ex + gear.spd;
        if (spd_mod.Any()) {
            foreach (Mod m in spd_mod) {
                if (m != null) {
                    val += m.value;
                }
            }
        }
        currentSPD = val;
        if (currentSPD <= 1) {
            currentSPD = 1;
        } 

        val = baseCR + cr_ex + gear.cr;
        if (cr_mod.Any()) {
            foreach (Mod m in cr_mod) {
                if (m != null) {
                    val += m.value;
                }
            }
        }
        currentCR = val;

        val = baseCD + cd_ex + gear.cd;
        if (cd_mod.Any()) {
            foreach (Mod m in cd_mod) {
                if (m != null) {
                    val += m.value;
                }
            }
        }
        currentCD = val;

        if (health > maxhealth) {
            health = maxhealth;
        }

    }

    public int checkLevelChange() {
        int up = 0;
        while (xp >= L_UP_REQ * level)
        {
            xp -= L_UP_REQ * level;
            level++;
            up++;
            health += L_UP_HP;
            verifyMod();
        }
        return up;
    }

    public string getStats()
    {
        verifyMod();
        string characterStatText = "Level " + level + " (Progress: " + xp + "/" + (L_UP_REQ * level) + ")";
        characterStatText += "\nHP: " + (health) + " / " + (maxhealth);
        characterStatText += "\nATK: " + (baseATK + atk_ex + gear.atk + (Character.L_UP_ATK * (level - 1)));
        characterStatText += "\nDEF: " + (baseDEF + def_ex + gear.def + (Character.L_UP_DEF * (level - 1)));
        characterStatText += "\nSPD: " + (baseSPD + spd_ex + gear.spd);
        characterStatText += "\nCRIT RATE: " + (baseCR + cr_ex + gear.cr);
        characterStatText += "\nCRIT DMG: " + (baseCD + cd_ex + gear.cd);
        return characterStatText;
    }

    public static void addSkill(Skills s)
    {
        Character c = GetCharacter(s.skillUser);
        bool hasAdded = false;
        foreach (Skills skill in c.skills)
        {
            if (s.skillName == skill.skillName)
            {
                skill.stacks += s.stacks + 1;
                hasAdded = true;
            }
        }

        if (!hasAdded)
        {
            c.skills.Add(s);
        }
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
