using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using System.Linq;
using Unity.VisualScripting;

public class Character
{

    public string character;

    public int level;

    public int health;
    public int maxhealth;
    public int baseHP;
    public List<int> hp_mod = new List<int>();
    public List<double> hp_p_mod = new List<double>();

    public double baseATK;
    public List<double> atk_mod = new List<double>();
    public List<double> atk_p_mod = new List<double>();

    public double baseDEF;
    public List<double> def_mod = new List<double>();
    public List<double> def_p_mod = new List<double>();

    public int baseSPD;
    public List<int> spd_mod = new List<int>();

    public double baseCR;
    public List<double> cr_mod = new List<double>();

    public double baseCD;
    public List<double> cd_mod = new List<double>();

    public List<Skills> skills = new List<Skills>();

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

        skills.Add(new Skills("Attack", "A basic attack that deals 100% of the player's attack on a single target.", 0, 0));

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

    public void startTurn() {
        
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
        verifyMod();
        ch.verifyMod();

        var rand = new System.Random();

        // DMG CALCULATION
        double dmg = (double)((baseATK + atk_mod[0]) * ((100.0 + atk_p_mod[0]) / 100.0) * (multiplier / 100.0));

        // CRIT MULTIPLIER
        double crit = 1;
        if ((rand.NextDouble() * 100.0) <= (baseCR + cr_mod[0])) {
            crit = (100.0 + baseCD + cd_mod[0]) / 100.0;
        }

        // DEF MULTIPLIER
        double def = (double)(1 - ((ch.baseDEF + ch.def_mod[0]) * ((100.0 + ch.def_p_mod[0])/100.0)) / (((ch.baseDEF + ch.def_mod[0]) * ((100.0 + ch.def_p_mod[0])/100.0)) + 200 + (10 * ((double) ch.level))));

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
        
        return (int) dmg;

    }

    public void verifyMod() {
        for (int i = 0; i < 11; i++) {
            if (hp_mod.Count < 10) {
                hp_mod.Add(0);
            }
            if (hp_p_mod.Count < 10) {
                hp_p_mod.Add(0);
            }
            if (atk_mod.Count < 10) {
                atk_mod.Add(0);
            }
            if (atk_p_mod.Count < 10) {
                atk_p_mod.Add(0);
            }
            if (def_mod.Count < 10) {
                def_mod.Add(0);
            }
            if (def_p_mod.Count < 10) {
                def_p_mod.Add(0);
            }
            if (spd_mod.Count < 10) {
                spd_mod.Add(0);
            }
            if (cr_mod.Count < 10) {
                cr_mod.Add(0);
            }
            if (cd_mod.Count < 10) {
                cd_mod.Add(0);
            }
        }

        maxhealth = (int)((baseHP + hp_mod[0]) * (100 + hp_p_mod[0]));

    }

    public int getSpeed() {
        verifyMod();
        return baseSPD + spd_mod[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual string getName() {
        return character; 
    }

    public override string ToString() {
        return "{ name: " + character + " }";
    }

}
