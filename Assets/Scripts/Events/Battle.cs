using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.AI;

public class Battle : MonoBehaviour
{

    private List<GameObject> trashCan = new List<GameObject>();
    public GameObject SP_Bar;
    public Slider obj_SPSLIDER;
    public Text obj_SP;

    public GameObject ActionBox;
    public Text action_OP1;
    public Text action_OP2;
    public Text action_OP3;
    public Text action_OP4;

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

    public GameObject TargetBox;
    public bool targetTypeIsEnemy = true;

    public GameObject tar_BOX_1;
    public GameObject tar_BOX_2;
    public GameObject tar_BOX_3;
    public GameObject tar_BOX_4;

    public Text tar_NAME_1;
    public Text tar_NAME_2;
    public Text tar_NAME_3;
    public Text tar_NAME_4;

    public Slider tar_HPSLIDER_1;
    public Slider tar_HPSLIDER_2;
    public Slider tar_HPSLIDER_3;
    public Slider tar_HPSLIDER_4;

    public Text tar_HPVALUE_1;
    public Text tar_HPVALUE_2;
    public Text tar_HPVALUE_3;
    public Text tar_HPVALUE_4;

    public GameObject TurnBox;
    public Text turnOrderDisplay;

    public GameObject DescriptionBox;
    public GameObject DamageBox;

    public GameObject GameDescriptionBox;
    public Text GameDescription;

    public static bool firstRun = false;
    public static int battleSP;
    public static int battleSPMAX;
    public static int sel_phase;
    public static int sel_action_last = -1;
    public static int sel_action = 0;
    public static int sel_target = 0;
    public static int levelCheck = 0;
    public static bool buttonCooldown = false;
    public static bool animationCooldown = false;
    public static float moveSpeed = 4000;
    public static List<Character> cycle = new List<Character>();
    public static string lastTurn;

    public static List<Enemy> enemies = new List<Enemy>();


    public static void StartBattle(GameObject enemy, int numEnemies) {
        Game.gameEvent = "Battle";
        Game.gameMovementFreeze = true;
        Game.showMap = false;
        lastTurn = "NOBODY";
        sel_phase = 1;
        sel_action_last = -1;
        sel_action = 0;
        sel_target = 0;
        battleSPMAX = 10;
        battleSP = battleSPMAX / 2;

        cycle.Clear();
        enemies.Clear();

        for (int i = 0; i < numEnemies; i++) {
            enemies.Add(new Enemy("Slime", i + 1, 1, 45, 12, 8, 11, 5, 8));
            enemies[i].verifyMod();
        }
        
        foreach (Character c in Game.characters) {
            c.resetMod();
        }

        SceneManager.LoadScene("Battle Scene");
    }

    private void NewCycle() {
        cycle.Clear();

        foreach (Character c in Game.characters) {
            c.verifyMod();
        }

        foreach (Character c in enemies) {
            c.verifyMod();
        }

        for (int i = 0; i < Game.characters.Count; i++) {
            if (Game.characters[i].health > 0) {
                bool flag = true;
                for (int j = 0; j < cycle.Count; j++) {
                    if (Game.characters[i].currentSPD > cycle[j].currentSPD) {
                        cycle.Insert(j, Game.characters[i]);
                        flag = false;
                        j = cycle.Count;
                    }
                }
                if (flag) {
                    cycle.Add(Game.characters[i]);
                }
            }
        }
        
        if (enemies.Any()) {
            for (int i = 0; i < enemies.Count; i++) {
                if (enemies[i].health > 0) {
                    bool flag = true;
                    for (int j = 0; j < cycle.Count; j++) {
                        if (enemies[i].currentSPD > cycle[j].currentSPD) {
                            cycle.Insert(j, enemies[i]);
                            flag = false;
                            j = cycle.Count;
                        }
                    }
                    if (flag) {
                        cycle.Add(enemies[i]);
                    }
                }
            }
        }

        StartCoroutine(Sleep(2));
        
    }

    // Start is called before the first frame update
    void Start()
    {

        if (!firstRun) {
            firstRun = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        obj_HPSLIDER_1.maxValue = Game.characters[0].maxhealth;
        obj_HPSLIDER_1.value = Game.characters[0].health;
        obj_HPVALUE_1.text = Game.characters[0].health + " / " + Game.characters[0].maxhealth;
        obj_HPSLIDER_2.maxValue = Game.characters[1].maxhealth;
        obj_HPSLIDER_2.value = Game.characters[1].health;
        obj_HPVALUE_2.text = Game.characters[1].health + " / " + Game.characters[1].maxhealth;
        obj_HPSLIDER_3.maxValue = Game.characters[2].maxhealth;
        obj_HPSLIDER_3.value = Game.characters[2].health;
        obj_HPVALUE_3.text = Game.characters[2].health + " / " + Game.characters[2].maxhealth;
        obj_HPSLIDER_4.maxValue = Game.characters[3].maxhealth;
        obj_HPSLIDER_4.value = Game.characters[3].health;
        obj_HPVALUE_4.text = Game.characters[3].health + " / " + Game.characters[3].maxhealth;

        if (battleSP > battleSPMAX) {
            battleSP = battleSPMAX;
        }
        
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
        }

        string turn = cycle[0].character;
        switch(cycle[0].character) {
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

        action_OP1.fontStyle = FontStyle.Normal;
        action_OP2.fontStyle = FontStyle.Normal;
        action_OP3.fontStyle = FontStyle.Normal;
        action_OP4.fontStyle = FontStyle.Normal;

        switch(sel_action) {
            case 0:
                action_OP1.fontStyle = FontStyle.Bold;
                break;
            case 1:
                action_OP2.fontStyle = FontStyle.Bold;
                break;
            case 2:
                action_OP3.fontStyle = FontStyle.Bold;
                break;
            case 3:
                action_OP4.fontStyle = FontStyle.Bold;
                break;
        }

        tar_BOX_1.SetActive(false);
        tar_BOX_2.SetActive(false);
        tar_BOX_3.SetActive(false);
        tar_BOX_4.SetActive(false);

        int target = 0;

        if (targetTypeIsEnemy) {
            if (sel_target < 4) {

                target = 0;
                if (enemies.Count >= target + 1) {
                    tar_NAME_1.text = ">>  " + enemies[target].getName();
                    tar_HPSLIDER_1.maxValue = enemies[target].maxhealth;
                    tar_HPSLIDER_1.value = enemies[target].health;
                    tar_HPVALUE_1.text = enemies[target].health + " / " + enemies[target].maxhealth;
                    tar_BOX_1.SetActive(true);
                }
                target = 1;
                if (enemies.Count >= target + 1) {
                    tar_NAME_2.text = ">>  " + enemies[target].getName();
                    tar_HPSLIDER_2.maxValue = enemies[target].maxhealth;
                    tar_HPSLIDER_2.value = enemies[target].health;
                    tar_HPVALUE_2.text = enemies[target].health + " / " + enemies[target].maxhealth;
                    tar_BOX_2.SetActive(true);
                }
                target = 2;
                if (enemies.Count >= target + 1) {
                    tar_NAME_3.text = ">>  " + enemies[target].getName();
                    tar_HPSLIDER_3.maxValue = enemies[target].maxhealth;
                    tar_HPSLIDER_3.value = enemies[target].health;
                    tar_HPVALUE_3.text = enemies[target].health + " / " + enemies[target].maxhealth;
                    tar_BOX_3.SetActive(true);
                }
                target = 3;
                if (enemies.Count >= target + 1) {
                    tar_NAME_4.text = ">>  " + enemies[target].getName();
                    tar_HPSLIDER_4.maxValue = enemies[target].maxhealth;
                    tar_HPSLIDER_4.value = enemies[target].health;
                    tar_HPVALUE_4.text = enemies[target].health + " / " + enemies[target].maxhealth;
                    tar_BOX_4.SetActive(true);
                }

            } else {

                target = 4;
                if (enemies.Count >= target + 1) {
                    tar_NAME_1.text = ">>  " + enemies[target].getName();
                    tar_HPSLIDER_1.maxValue = enemies[target].maxhealth;
                    tar_HPSLIDER_1.value = enemies[target].health;
                    tar_HPVALUE_1.text = enemies[target].health + " / " + enemies[target].maxhealth;
                    tar_BOX_1.SetActive(true);
                }
                target = 5;
                if (enemies.Count >= target + 1) {
                    tar_NAME_2.text = ">>  " + enemies[target].getName();
                    tar_HPSLIDER_2.maxValue = enemies[target].maxhealth;
                    tar_HPSLIDER_2.value = enemies[target].health;
                    tar_HPVALUE_2.text = enemies[target].health + " / " + enemies[target].maxhealth;
                    tar_BOX_2.SetActive(true);
                }
                target = 6;
                if (enemies.Count >= target + 1) {
                    tar_NAME_3.text = ">>  " + enemies[target].getName();
                    tar_HPSLIDER_3.maxValue = enemies[target].maxhealth;
                    tar_HPSLIDER_3.value = enemies[target].health;
                    tar_HPVALUE_3.text = enemies[target].health + " / " + enemies[target].maxhealth;
                    tar_BOX_3.SetActive(true);
                }
                target = 7;
                if (enemies.Count >= target + 1) {
                    tar_NAME_4.text = ">>  " + enemies[target].getName();
                    tar_HPSLIDER_4.maxValue = enemies[target].maxhealth;
                    tar_HPSLIDER_4.value = enemies[target].health;
                    tar_HPVALUE_4.text = enemies[target].health + " / " + enemies[target].maxhealth;
                    tar_BOX_4.SetActive(true);
                }
            }

            tar_NAME_1.fontStyle = FontStyle.Normal;
            tar_NAME_2.fontStyle = FontStyle.Normal;
            tar_NAME_3.fontStyle = FontStyle.Normal;
            tar_NAME_4.fontStyle = FontStyle.Normal;
            
            switch(sel_target) {
                case 0 or 4:
                    tar_NAME_1.fontStyle = FontStyle.Bold;
                    break;
                case 1 or 5:
                    tar_NAME_2.fontStyle = FontStyle.Bold;
                    break;
                case 2 or 6:
                    tar_NAME_3.fontStyle = FontStyle.Bold;
                    break;
                case 3 or 7:
                    tar_NAME_4.fontStyle = FontStyle.Bold;
                    break;
            }

        } else {

            target = 0;
            tar_NAME_1.text = ">>  " + Game.characters[target].getName();
            tar_HPSLIDER_1.maxValue = Game.characters[target].maxhealth;
            tar_HPSLIDER_1.value = Game.characters[target].health;
            tar_HPVALUE_1.text = Game.characters[target].health + " / " + Game.characters[target].maxhealth;
            tar_BOX_1.SetActive(true);

            target = 1;
            tar_NAME_2.text = ">>  " + Game.characters[target].getName();
            tar_HPSLIDER_2.maxValue = Game.characters[target].maxhealth;
            tar_HPSLIDER_2.value = Game.characters[target].health;
            tar_HPVALUE_2.text = Game.characters[target].health + " / " + Game.characters[target].maxhealth;
                tar_BOX_2.SetActive(true);

            target = 2;
            tar_NAME_3.text = ">>  " + Game.characters[target].getName();
            tar_HPSLIDER_3.maxValue = Game.characters[target].maxhealth;
            tar_HPSLIDER_3.value = Game.characters[target].health;
            tar_HPVALUE_3.text = Game.characters[target].health + " / " + Game.characters[target].maxhealth;
            tar_BOX_3.SetActive(true);
            
            target = 3;
            tar_NAME_4.text = ">>  " + Game.characters[target].getName();
            tar_HPSLIDER_4.maxValue = Game.characters[target].maxhealth;
            tar_HPSLIDER_4.value = Game.characters[target].health;
            tar_HPVALUE_4.text = Game.characters[target].health + " / " + Game.characters[target].maxhealth;
            tar_BOX_4.SetActive(true);

            tar_NAME_1.fontStyle = FontStyle.Normal;
            tar_NAME_2.fontStyle = FontStyle.Normal;
            tar_NAME_3.fontStyle = FontStyle.Normal;
            tar_NAME_4.fontStyle = FontStyle.Normal;
            
            switch(sel_target) {
                case 0 or 4:
                    tar_NAME_1.fontStyle = FontStyle.Bold;
                    break;
                case 1 or 5:
                    tar_NAME_2.fontStyle = FontStyle.Bold;
                    break;
                case 2 or 6:
                    tar_NAME_3.fontStyle = FontStyle.Bold;
                    break;
                case 3 or 7:
                    tar_NAME_4.fontStyle = FontStyle.Bold;
                    break;
            }

        }
        
        int turnNumber = 0;
        string displayCycle = "";
        foreach(Character c in cycle) {
            turnNumber++;
            displayCycle += turnNumber + " - " + c.getName() + "\n";
        }

        turnOrderDisplay.text = displayCycle;

        // CYCLE UPDATE
        if (!buttonCooldown && !animationCooldown && sel_phase >= -5) {
            if (isPlayerTurn()) {
                if (lastTurn != turn) {
                    sel_phase = 2;
                    lastTurn = turn;
                    var targetPos = CharacterBox.transform.position;
                    ActionBox.transform.position = CharacterBox.transform.position;
                    targetPos.x += 517;
                    StartCoroutine(Move(ActionBox, targetPos));

                } else if (sel_phase == 2) {
                    updateSelection(ref sel_action, 3);
                    string selectedAction = null;
                    switch(sel_action) {
                        case 0:
                            selectedAction = action_OP1.text;
                            break;
                        case 1:
                            selectedAction = action_OP2.text;
                            break;
                        case 2:
                            selectedAction = action_OP3.text;
                            break;
                        case 3:
                            selectedAction = action_OP4.text;
                            break;
                    }
                    
                    if (selectedAction != null) {
                        if (sel_action_last != sel_action) {
                            sel_action_last = sel_action;
                            selectedAction = selectedAction.Substring(4);
                            while (trashCan.Any()) {
                                GameObject t = trashCan[0];
                                trashCan.RemoveAt(0);
                                Destroy(t);
                            }
                            GameObject DescriptionBoxClone = Instantiate(DescriptionBox, new Vector3(ActionBox.transform.position.x, ActionBox.transform.position.y), ActionBox.transform.rotation);
                            DescriptionBoxClone.transform.SetParent(DescriptionBox.transform);
                            DescriptionBoxClone.name = "TEMP DescBox" + sel_action;
                            DescriptionBoxClone.GetComponent<TextManager>().text = TextManager.convertSkillDescription(cycle[0], selectedAction);
                            trashCan.Add(DescriptionBoxClone);
                            var targetPos = ActionBox.transform.position;
                            targetPos.x += 517;
                            StartCoroutine(Move(DescriptionBoxClone, targetPos));
                        }

                        int confirm = getConfirmation();
                        if (confirm == 1) {

                            selectedAction = selectedAction.Substring(4);
                            Skills skill = SkillsRegistry.getSkill(selectedAction);
                            if (skill == null) {
                                if (selectedAction.IndexOf("Skill: ")  >= 0) {
                                    selectedAction = selectedAction.Substring(7);
                                    skill = SkillsRegistry.getSkill(selectedAction);
                                }
                            }

                            if (skill != null) {
                                if (battleSP >= skill.spConsumption) {
                                    while (trashCan.Any()) {
                                        GameObject t = trashCan[0];
                                        trashCan.RemoveAt(0);
                                        Destroy(t);
                                    }
                                    if (skill.targetType == "Self") {
                                        Skills.useSkill(cycle[0], skill);
                                        sel_phase = -2;
                                    } else if (skill.targetType == "Enemy" || skill.targetType == "Ally" || skill.targetType == "Non-Self Ally" || skill.targetType == "Dead") {
                                        if (skill.targetType == "Enemy") {
                                            targetTypeIsEnemy = true;
                                            if (sel_target >= enemies.Count) {
                                                sel_target = enemies.Count - 1;
                                            }
                                        }
                                        if (skill.targetType == "Ally" || skill.targetType == "Non-Self Ally" || skill.targetType == "Dead") {
                                            targetTypeIsEnemy = false;
                                            if (sel_target >= Game.characters.Count) {
                                                sel_target = Game.characters.Count - 1;
                                            }
                                        }
                                            
                                        sel_phase = 3;
                                        var targetPos = ActionBox.transform.position;
                                        TargetBox.transform.position = ActionBox.transform.position;
                                        targetPos.x += 517;
                                        StartCoroutine(Move(TargetBox, targetPos));
                                    }
                                }
                            }
                        }

                    }
                } else if (sel_phase == 3) {

                    int dmg = 0;

                    string selectedAction = null;
                    switch(sel_action) {
                        case 0:
                            selectedAction = action_OP1.text;
                            break;
                        case 1:
                            selectedAction = action_OP2.text;
                            break;
                        case 2:
                            selectedAction = action_OP3.text;
                            break;
                        case 3:
                            selectedAction = action_OP4.text;
                            break;
                    }

                    selectedAction = selectedAction.Substring(4);
                    Skills skill = SkillsRegistry.getSkill(selectedAction);
                    if (skill == null) {
                        if (selectedAction.IndexOf("Skill: ")  >= 0) {
                            selectedAction = selectedAction.Substring(7);
                            skill = SkillsRegistry.getSkill(selectedAction);
                        }
                    }

                    if (targetTypeIsEnemy) {
                        updateSelection(ref sel_target, enemies.Count - 1);
                    } else {
                        updateSelection(ref sel_target, Game.characters.Count - 1);
                    }
                    int confirm = getConfirmation();

                    if (confirm == -1) {
                        while (trashCan.Any()) {
                            GameObject t = trashCan[0];
                            trashCan.RemoveAt(0);
                            Destroy(t);
                        }
                        TargetBox.transform.position = CharacterBox.transform.position;
                        sel_phase = 2;
                        sel_action_last = -1;
                    } else if (confirm == 1) {
                        if (skill != null) {
                            if (skill.targetType == "Enemy") {
                                if (enemies[sel_target].health > 0) {
                                    dmg = Skills.useSkill(cycle[0], skill, enemies[sel_target]);
                                    sel_phase = -2;
                                }
                            } else if (skill.targetType == "Ally") {
                                if (Game.characters[sel_target].health > 0) {
                                    dmg = Skills.useSkill(cycle[0], skill, Game.characters[sel_target]);
                                    sel_phase = -2;
                                }
                            } else if (skill.targetType == "Non-Self Ally") {
                                if (Game.characters[sel_target].health > 0 && Game.characters[sel_target] != cycle[0]) {
                                    dmg = Skills.useSkill(cycle[0], skill, Game.characters[sel_target]);
                                    sel_phase = -2;
                                }
                            } else if (skill.targetType == "Non-Self Ally") {
                                if (Game.characters[sel_target].health > 0 && Game.characters[sel_target] != cycle[0]) {
                                    dmg = Skills.useSkill(cycle[0], skill, Game.characters[sel_target]);
                                    sel_phase = -2;
                                }
                            }
                        }
                    }

                    if (dmg > 0) {
                        StartCoroutine(Damage(dmg));
                    }

                } else if (sel_phase == -2) {
                    while (trashCan.Any()) {
                        GameObject t = trashCan[0];
                        trashCan.RemoveAt(0);
                        Destroy(t);
                    }
                    ActionBox.transform.position = CharacterBox.transform.position;
                    TargetBox.transform.position = CharacterBox.transform.position;
                    cycle[0].endTurn();
                    sel_action_last = -1;
                    lastTurn = null;
                    StartCoroutine(SuspendCycleChange(0));
                }
            } else {
                int dmg = Skills.useSkill(cycle[0], SkillsRegistry.getSkill("Attack"), Enemy.selectTarget(true));
                if (dmg > 0) {
                    StartCoroutine(Damage(dmg));
                }
                cycle[0].endTurn();
                StartCoroutine(SuspendCycleChange(0));
            }
        } else if (sel_phase == -10) {
            levelCheck++;
            if (levelCheck >= Game.characters.Count) {
                sel_phase = -12;
                GameDescriptionBox.SetActive(false);
                StartCoroutine(SuspendSceneChange(2, "Overworld Scene"));
                Game.updateEnemies = true;
            } else if (levelCheck == -1) {
                int gold = 2 * enemies.Count;
                Game.gold += gold;
                Game.enemiesKilled += enemies.Count;
                GameDescription.text = "You earned " + gold + "G!";
                GameDescriptionBox.SetActive(true);
                sel_phase = -11;
            } else {
                if (Game.characters[levelCheck].health > 0) {
                    Game.characters[levelCheck].xp += enemies.Count();
                    int levelChange = Game.characters[levelCheck].checkLevelChange();
                    if (levelChange > 0) {
                        GameDescription.text = Game.characters[levelCheck].character + " leveled up!\nLevel " + (Game.characters[levelCheck].level - levelChange) + " >> " + Game.characters[levelCheck].level;
                        GameDescriptionBox.SetActive(true);
                        sel_phase = -11;
                    }
                } else {
                    Game.timesDowned++;
                    GameDescription.text = Game.characters[levelCheck].character + " was downed and did not get any XP.";
                    GameDescriptionBox.SetActive(true);
                    sel_phase = -11;
                }
            }
        } else if (sel_phase == -11) {
            int confirm = getConfirmation();
            if (confirm != 0) {
                sel_phase = -10;
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

    private int getConfirmation() {
        if (Input.GetKeyDown(KeyCode.C)) {
            return 1;
        } else if (Input.GetKeyDown(KeyCode.X)) {
            return -1;
        } else {
            return 0;
        }
    }

    private bool isPlayerTurn() {
        string p = cycle[0].character;
        foreach (Character c in Game.characters) {
            if (p == c.character) {
                return true;
            }
        }
         return false;
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

    public static IEnumerator Move(GameObject item, Vector3 targetPos)
    {
        buttonCooldown = true;
        while (item != null && (targetPos - item.transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        if (item != null) {
            item.transform.position = targetPos;
        }
        buttonCooldown = false;
    }

    IEnumerator Sleep(int seconds) {
        animationCooldown = true;
        yield return new WaitForSecondsRealtime(seconds);
        animationCooldown = false;
    }

    IEnumerator SuspendCycleChange(int seconds)
    {

        buttonCooldown = true;
        yield return new WaitForSecondsRealtime(seconds);
        cycle.RemoveAt(0);
        checkGameStatus();
        buttonCooldown = false;
        
    }

    public IEnumerator Damage(int dmg)
    {
        animationCooldown = true;
        DamageBox.SetActive(true);
        DamageBox.GetComponent<TextManager>().text = "Total Damage\n" + dmg;
        yield return new WaitForSecondsRealtime(1);
        DamageBox.SetActive(false);
        yield return new WaitForSecondsRealtime(1);
        animationCooldown = false;
    }

    public void checkGameStatus() {
        if (sel_phase > -5) {
            int alive = 0;
            foreach (Character c in Game.characters) {
                if (c.health > 0) {
                    alive++;
                }
            }

            if (alive <= 0) {
                StartCoroutine(SuspendSceneChange(2, "End Scene"));
            }

            alive = 0;
            foreach (Character c in enemies) {
                if (c.health > 0) {
                    alive++;
                }
            }

            if (alive <= 0) {
                buttonCooldown = true;
                animationCooldown = true;
                levelCheck = -2;
                sel_phase = -10;
                //StartCoroutine(SuspendSceneChange(2, "Overworld Scene"));
            }
        }
    }

    IEnumerator SuspendSceneChange(int seconds, string scene)
    {

        animationCooldown = true;
        yield return new WaitForSecondsRealtime(seconds);
        SceneManager.LoadScene(scene);
        Game.gameMovementFreeze = false;
        animationCooldown = false;
        
    }
    
}
