using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    public int id;

    public Enemy(string name, int identifier, int hp, double atk, double def, double spd, double cr, double cd) : base(name, hp, atk, def, spd, cr, cd) {
        character = name;
        baseHP = hp;
        baseATK = atk;
        baseDEF = def;
        baseSPD = spd;
        baseCR = cr;
        baseCD = cd;
        health = hp;
        id = identifier;
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
        return "{ name: " + character + ", id: " + id + ", baseHP: " + baseHP + ", hp: " + health + " }";
    }

}
