using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{

    public string character;

    public int baseHP;

    public double baseATK;

    public double baseDEF;

    public double baseSPD;

    public double baseCR;

    public double baseCD;

    public int health;

    public Character(string name, int hp, double atk, double def, double spd, double cr, double cd) {
        character = name;
        baseHP = hp;
        baseATK = atk;
        baseDEF = def;
        baseSPD = spd;
        baseCR = cr;
        baseCD = cd;
        health = hp;
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
