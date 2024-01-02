using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Linq;
using Unity.VisualScripting;

public class Character
{

    public string character;

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

    public double baseSPD;
    public List<double> spd_mod = new List<double>();

    public double baseCR;
    public List<double> cr_mod = new List<double>();

    public double baseCD;
    public List<double> cd_mod = new List<double>();

    public List<Skills> skills = new List<Skills>();

    public Character(string name, int hp, double atk, double def, double spd, double cr, double cd) {
        character = name;
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

    public void damage(Character ch, double multiplier) {
        verifyMod();
        ch.verifyMod();

        var rand = new System.Random();

        // DMG CALCULATION
        double dmg = (double)((baseATK + atk_mod[0]) * ((100 + atk_p_mod[0]) / 100) * (multiplier / 100));

        // CRIT MULTIPLIER
        if ((rand.NextDouble() * 100) > (baseCR + cr_mod[0])) {
            dmg *= (double)((100 + baseCD + cd_mod[0]) / 100);
        }

        // DEF MULTIPLIER
        dmg *= (double)(100 - ((baseDEF + def_mod[0]) * (100 * def_p_mod[0])) / (((baseDEF + def_mod[0]) * (100 * def_p_mod[0])) + 200 + (10 * Game.floorNumber))) / 100;

        // INVOKE DMG
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string ToString() {
        return "{ name: " + character + " }";
    }

}
