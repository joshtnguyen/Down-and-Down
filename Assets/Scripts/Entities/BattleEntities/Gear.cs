using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear
{
    public string name;
    public string gearAbility;
    public List<string> upgradeList = new List<string>();
    public int points = 0;
    public double hp_up = 0;
    public double hp_p_up = 0;
    public double atk_up = 0;
    public double atk_p_up = 0;
    public double def_up = 0;
    public double def_p_up = 0;
    public double spd_up = 0;
    public double cr_up = 0;
    public double cd_up = 0;
    public double hp = 0;
    public double hp_p = 0;
    public double atk = 0;
    public double atk_p = 0;
    public double def = 0;
    public double def_p = 0;
    public double spd = 0;
    public double cr = 0;
    public double cd = 0;
    public double effect_bleed;
    public double effect_freeze;

    public Gear(bool def) {
        name = "Default Gear";
        gearAbility = "Nothing";
    }

    public Gear() {
        for (int i = 0; i < Game.floorNumber * -1; i++) {
            switch (UnityEngine.Random.Range(0, 9)) {
                case 0:
                    hp += Random(1.5, 4.0);
                    break;
                case 1:
                    hp_p += Random(0.2, 0.5);
                    break;
                case 2:
                    atk += Random(2.0, 3.5);
                    break;
                case 3:
                    atk_p += Random(0.2, 0.5);
                    break;
                case 4:
                    def += Random(2.0, 3.5);
                    break;
                case 5:
                    def_p += Random(0.2, 0.5);
                    break;
                case 6:
                    spd += Random(0.1, 0.3);
                    break;
                case 7:
                    cr += Random(0.5, 2.5);
                    break;
                case 8:
                    cd += Random(0.5, 2.5);
                    break;
            }
        }
        for (int i = 0; i < 3; i++) {
            switch (UnityEngine.Random.Range(0, 9)) {
                case 0:
                    hp_up += Random(1.5, 4.0);
                    upgradeList.Add("hp_up");
                    break;
                case 1:
                    hp_p_up += Random(0.2, 0.5);
                    upgradeList.Add("hp_p_up");
                    break;
                case 2:
                    atk_up += Random(2.0, 3.5);
                    upgradeList.Add("atk_up");
                    break;
                case 3:
                    atk_p_up += Random(0.2, 0.5);
                    upgradeList.Add("atk_p_up");
                    break;
                case 4:
                    def_up += Random(2.0, 3.5);
                    upgradeList.Add("def_up");
                    break;
                case 5:
                    def_p_up += Random(0.2, 0.5);
                    upgradeList.Add("def_p_up");
                    break;
                case 6:
                    spd_up += Random(0.1, 0.3);
                    upgradeList.Add("spd_up");
                    break;
                case 7:
                    cr_up += Random(0.5, 2.5);
                    upgradeList.Add("cr_up");
                    break;
                case 8:
                    cd_up += Random(0.5, 2.5);
                    upgradeList.Add("cd_up");
                    break;
            }
        }
    }

    public double Random(double lower, double upper) {
        var rand = new System.Random();
        return rand.NextDouble() * (upper - lower) + lower;
    }

    public void levelUp(int times) {
        for (int i = 0; i < times; i++) {
            switch (upgradeList[UnityEngine.Random.Range(0, upgradeList.Count)]) {
                case "hp_up":
                    hp += hp_up;
                    break;
                case "hp_p_up":
                    hp_p += hp_p_up;
                    break;
                case "atk_up":
                    atk += atk_up;
                    break;
                case "atk_p_up":
                    atk_p += atk_p_up;
                    break;
                case "def_up":
                    def += def_up;
                    break;
                case "def_p_up":
                    def_p += def_p_up;
                    break;
                case "spd_up":
                    spd += spd_up;
                    break;
                case "cr_up":
                    cr += cr_up;
                    break;
                case "cd_up":
                    cd += cd_up;
                    break;
            }
        }
    }

    public void levelUp() {
        levelUp(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetValues() {
        Debug.Log(hp_up + " / " + hp_p_up + " / " + atk_up + " / " + atk_p_up + " / " + def_up + " / " + def_p_up + " / " + spd_up + " / " + cr_up + " / " + cd_up);
        Debug.Log(hp + " / " + hp_p + " / " + atk + " / " + atk_p + " / " + def + " / " + def_p + " / " + spd + " / " + cr + " / " + cd);
    }
}
