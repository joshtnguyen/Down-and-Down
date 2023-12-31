using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Battle : MonoBehaviour
{

    public GameObject SP_Bar;
    public Slider obj_SPSLIDER;
    public Text obj_SP;

    public GameObject ActionBox;
    public Text action_ATK;
    public Text action_SK1;
    public Text action_SK2;
    public Text action_SKIP;

    public GameObject CharacterBox;

    public Text obj_NAME_1;
    public Text obj_NAME_2;
    public Text obj_NAME_3;
    public Text obj_NAME_4;

    public Slider obj_HPSLIDER_1;
    public Slider obj_HPSLIDER_2;
    public Slider obj_HPSLIDER_3;
    public Slider obj_HPSLIDER_4;

    public Text obj_HPVALUE_1;
    public Text obj_HPVALUE_2;
    public Text obj_HPVALUE_3;
    public Text obj_HPVALUE_4;


    public static bool firstRun = false;
    public static int battleSP;
    public static int battleSPMAX;
    public static List<Character> characters = new List<Character>();
    public static Character walter = new Character("Walter", 100, 15, 10, 10, 5, 12);
    public static Character benedict = new Character("Benedict", 95, 10, 12, 11, 8, 8);
    public static Character sherri = new Character("Sherri", 120, 7, 12, 12, 8, 3);
    public static Character jade = new Character("Jade", 150, 5, 18, 9, 2, 3);


    public static void StartBattle(GameObject enemy) {
        Destroy(enemy);
        Game.gameEvent = "Battle";
        Game.gameMovementFreeze = true;
        SceneManager.LoadScene("Battle Scene");
    }

    // Start is called before the first frame update
    void Start()
    {

        if (!firstRun) {
            firstRun = true;
            characters.Add(walter);
            characters.Add(benedict);
            characters.Add(sherri);
            characters.Add(jade);
            battleSP = 10;
            battleSPMAX = battleSP;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        obj_HPSLIDER_1.maxValue = walter.baseHP;
        obj_HPSLIDER_1.value = walter.health;
        obj_HPVALUE_1.text = walter.health + " / " + walter.baseHP;
        obj_HPSLIDER_2.maxValue = benedict.baseHP;
        obj_HPSLIDER_2.value = benedict.health;
        obj_HPVALUE_2.text = benedict.health + " / " + benedict.baseHP;
        obj_HPSLIDER_3.maxValue = sherri.baseHP;
        obj_HPSLIDER_3.value = sherri.health;
        obj_HPVALUE_3.text = sherri.health + " / " + sherri.baseHP;
        obj_HPSLIDER_4.maxValue = jade.baseHP;
        obj_HPSLIDER_4.value = jade.health;
        obj_HPVALUE_4.text = jade.health + " / " + jade.baseHP;

        obj_SPSLIDER.maxValue = battleSPMAX;
        obj_SPSLIDER.value = battleSP;
        if (battleSP == battleSPMAX) {
            obj_SP.text = "MAX";
        } else {
            obj_SP.text = "" + battleSP;
        }

    }
}
