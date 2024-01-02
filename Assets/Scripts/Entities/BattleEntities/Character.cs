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
    public List<int> atk_mod = new List<int>();
    public List<double> atk_p_mod = new List<double>();

    public double baseDEF;
    public List<int> def_mod = new List<int>();
    public List<double> def_p_mod = new List<double>();

    public double baseSPD;
    public List<int> spd_mod = new List<int>();

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
    }

    public void startTurn() {
        hp_mod.RemoveAt(0);
        
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
        return "{ name: " + character + ", baseHP: " + baseHP + ", hp: " + health + " }";
    }

}
