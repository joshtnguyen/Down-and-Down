using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using System.Linq;
using Unity.VisualScripting;

public class Battle : MonoBehaviour
{

    private Vector2 input;
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

    public GameObject TurnBox;
    public Text turnOrderDisplay;

    public static bool firstRun = false;
    public static int battleSP;
    public static int battleSPMAX;
    public static int sel_phase;
    public static int sel_action = 0;
    public static int sel_target = 0;
    public static bool buttonCooldown = false;
    public static float moveSpeed = 2500;
    public static List<string> cycle = new List<string>();
    public static string lastTurn;

    public static List<Character> characters = new List<Character>();
    public static Character walter = new Character("Walter", 100, 15, 10, 10, 5, 12);
    public static Character benedict = new Character("Benedict", 95, 10, 12, 11, 8, 8);
    public static Character sherri = new Character("Sherri", 120, 7, 12, 12, 8, 3);
    public static Character jade = new Character("Jade", 150, 5, 18, 9, 2, 3);


    public static void StartBattle(GameObject enemy) {
        Destroy(enemy);
        Game.gameEvent = "Battle";
        Game.gameMovementFreeze = true;
        lastTurn = "NOBODY";
        SceneManager.LoadScene("Battle Scene");
    }

    private static void NewCycle() {
        cycle.Clear();
        List<Character> charCycle = new List<Character>();
        charCycle.Add(characters[characters.Count - 1]);
        for (int i = characters.Count - 2; i >= 0; i--) {
            for (int j = charCycle.Count - 1; j >= 0; j--) {
                if (characters[i].baseSPD > charCycle[j].baseSPD) {
                    charCycle.Insert(j, characters[i]);
                }
                j = -1;
            }
        }

        foreach(Character c in charCycle) {
            cycle.Add(c.character);
        }
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

        obj_NAME_1.fontStyle = FontStyle.Normal;
        obj_NAME_2.fontStyle = FontStyle.Normal;
        obj_NAME_3.fontStyle = FontStyle.Normal;
        obj_NAME_4.fontStyle = FontStyle.Normal;

        if (!cycle.Any()) {
            NewCycle();
            //yield return new WaitForSeconds(2);
        }

        string turn = cycle[0];
        switch(cycle[0]) {
            case "Walter":
                obj_NAME_1.fontStyle = FontStyle.Bold;
                break;
            case "Benedict":
                obj_NAME_2.fontStyle = FontStyle.Bold;
                break;
            case "Sherri":
                obj_NAME_3.fontStyle = FontStyle.Bold;
                break;
            case "Jade":
                obj_NAME_4.fontStyle = FontStyle.Bold;
                break;
        }

        action_ATK.fontStyle = FontStyle.Normal;
        action_SK1.fontStyle = FontStyle.Normal;
        action_SK2.fontStyle = FontStyle.Normal;
        action_SKIP.fontStyle = FontStyle.Normal;

        switch(sel_action) {
            case 0:
                action_ATK.fontStyle = FontStyle.Bold;
                break;
            case 1:
                action_SK1.fontStyle = FontStyle.Bold;
                break;
            case 2:
                action_SK2.fontStyle = FontStyle.Bold;
                break;
            case 3:
                action_SKIP.fontStyle = FontStyle.Bold;
                break;
        }
        
        int turnNumber = 0;
        string displayCycle = "";
        foreach(string c in cycle) {
            turnNumber++;
            displayCycle += turnNumber + " - " + c + "\n";
        }

        turnOrderDisplay.text = displayCycle;

        if (!buttonCooldown) {
            if (lastTurn != turn) {
                buttonCooldown = true;
                sel_phase = 0;
                lastTurn = turn;
                var targetPos = CharacterBox.transform.position;
                ActionBox.transform.position = CharacterBox.transform.position;
                targetPos.x += 517;
                StartCoroutine(Move(ActionBox, targetPos));
                buttonCooldown = false;

            } else if (sel_phase == 0) {
                updateSelection(ref sel_action, 3);
            }
        }

    }

    private void updateSelection(ref int option, int optionLimit) {
        int hx = GetHorizontal();
        int hy = GetVertical();

        if (hx > 0 || hy > 0) {
            option++;
            if (option > optionLimit) {
                option = 0;
            }
        } else if (hx < 0 || hy < 0) {
            option--;
            if (option < 0) {
                option = optionLimit;
            }
        }
    }

    private int GetVertical() {
        int val = 0;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            return -1;
        } else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            return 1;
        }
        return val;
    }

    private int GetHorizontal() {
        int val = 0;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            return 1;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            return -1;
        }
        return val;
    }

    IEnumerator Move(GameObject item, Vector3 targetPos)
    {
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        item.transform.position = targetPos;
    }
    
}
