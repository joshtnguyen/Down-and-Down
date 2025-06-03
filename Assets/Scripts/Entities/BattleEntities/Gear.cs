using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear
{

    public static int ids;
    public static int wallet;

    public Character user = null;
    public string name;
    public int id;
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
    public double effect_bleed = 0;
    public double effect_freeze = 0;

    public Gear() {
        name = "Default Gear";
        id = 0;
    }

    public Gear(bool isReal) {
        if (isReal) {
            ids++;
            id = ids;
            List<string> names = new List<string>();
            names.Add("Bloodshedder");
            names.Add("Frostbyte");
            name = names[UnityEngine.Random.Range(0, names.Count)];
        } else {
            id = 0;
        }
        for (int i = 0; i < Game.floorNumber; i++) {
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
        int rand = UnityEngine.Random.Range(0, (int) ((upper - lower) * 100));
        return (double) (rand / 100.0) + lower;

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

    public string GetDisplayName() {
        return name + "/" + id;
    }

    public string GetValues() {

        string userName = "";
        if (user != null) {
            userName = " - " + user.character;
        }

        string hp_ex = "";
        if (hp_up > 0) {
            hp_ex = " (+" + hp_up + ")";
        }
        string hp_p_ex = "";
        if (hp_p_up > 0) {
            hp_p_ex = " (+" + hp_p_up + ")";
        }
        string atk_ex = "";
        if (atk_up > 0) {
            atk_ex = " (+" + atk_up + ")";
        }
        string atk_p_ex = "";
        if (atk_p_up > 0) {
            atk_p_ex = " (+" + atk_p_up + ")";
        }
        string def_ex = "";
        if (def_up > 0) {
            def_ex = " (+" + def_up + ")";
        }
        string def_p_ex = "";
        if (def_p_up > 0) {
            def_p_ex = " (+" + def_p_up + ")";
        }
        string cr_ex = "";
        if (cr_up > 0) {
            cr_ex = " (+" + cr_up + ")";
        }
        string cd_ex = "";
        if (cd_up > 0) {
            cd_ex = " (+" + cd_up + ")";
        }
        string spd_ex = "";
        if (spd_up > 0) {
            spd_ex = " (+" + spd_up + ")";
        }
         

        return name + "/" + id + userName + " (" + points + " POINTS)" + "\n" + 
        "HP: " + hp + hp_ex + " / HP %: " + hp_p + hp_p_ex + "\n" +
        "ATK: " + atk + atk_ex + " / ATK %: " + atk_p + atk_p_ex + "\n" +
        "DEF: " + def + def_ex + " / DEF %: " + def_p + def_p_ex + "\n" +
        "CRIT.R %: " + cr + cr_ex + " / CRIT.D %: " + cd + cd_ex + "\n" +
        "SPD: " + spd + spd_ex;
    }
}
