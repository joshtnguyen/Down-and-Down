using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Character
{

    public int id;
    public int sp;

    public Enemy(string name, int identifier, int le, int hp, double atk, double def, double spd, double cr, double cd) : base(name,le, hp, atk, def, spd, cr, cd) {
        character = name;
        level = le;
        baseHP = hp;
        baseATK = atk;
        baseDEF = def;
        baseSPD = spd;
        baseCR = cr;
        baseCD = cd;
        health = hp;
        id = identifier;
    }

    public static Character selectTarget(bool isCharacter) {
        if (isCharacter) {
            List<Character> selection = new List<Character>();
            foreach (Character c in Battle.characters) {
                if (c.health > 0) {
                    selection.Add(c);
                }
            }

            if (!selection.Any()) {
                return null;
            }

            var rand = new System.Random();
            return selection[rand.Next(selection.Count)];
        }

        return Battle.characters[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string getName() {
        return character + " (" + id + ")"; 
    }

    public override string ToString() {
        return "{ name: " + character + ", id: " + id + " }";
    }

}
