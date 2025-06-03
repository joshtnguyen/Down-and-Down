using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;

public class Battle : MonoBehaviour
{

    private List<GameObject> trashCan = new List<GameObject>();

    public List<GameObject> enemyTrashCan = new List<GameObject>();

    public GameObject enemyVisualParent;
    public GameObject enemyVisual;

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

    public GameObject CharacterStatsBox;
    public Text CharacterTitle;
    public Text CharacterStats;

    public GameObject StatusesBox;
    public Text StatusesTitle;
    public Text Statuses;

    public GameObject EnemySkillBox;
    public Text EnemySkillDescription;

    public GameObject DescriptionBox;
    public GameObject DamageBox;

    public GameObject GameDescriptionBox;
    public Text GameDescription;

    public GameObject SkillUsageBox;

    public static double avNumerator = 10000; // Numerator for Action Values
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
    public static float slowerMoveSpeed = 1000;
    public static List<Character> cycle = new List<Character>();
    public static string lastTurn;
    public static double multiplier = 0;
    public static int statusPage = 0;

    public static List<Enemy> enemies = new List<Enemy>();

    public static void StartBattle(int numEnemies, int difficulty,  double rewardMultiplier) {
        Room r = Game.map[Game.row, Game.col];
        Game.gameEvent = "Battle";
        Game.gameMovementFreeze = true;
        Game.showMap = false;
        lastTurn = "NOBODY";
        multiplier = rewardMultiplier;
        sel_phase = 1;
        sel_action_last = -1;
        sel_action = 0;
        sel_target = 0;
        battleSPMAX = 10;
        battleSP = battleSPMAX / 2;

        cycle.Clear();
        enemies.Clear();

        int level = Game.floorNumber + difficulty;

        double hp_p = Game.getDisruption("Heartiness").stacks * 0.5;
        double atk_p = Game.getDisruption("Strengthening").stacks * 0.5;
        double def_p = Game.getDisruption("Polishing").stacks * 0.3;

        for (int i = 0; i < numEnemies; i++)
        {
            string enemyType = Game.enemies[Random.Range(0, Game.enemies.Count)];
            Enemy e;
            Skills s;
            string skillName;
            switch (enemyType)
            {
                // NAME / ID / LEVEL / HP / ATK / DEF / SPD / CR / CD / SP
                case "Slime":
                    e = new Enemy("Slime", i + 1, level, 45, 12, 8, 11, 5, 8, 1);
                    skillName = "Goo Shot";
                    break;
                case "Cave Bull":
                    e = new Enemy("Cave Bull", i + 1, level, 52, 14, 2, 9, 7, 20, 1);
                    skillName = "Drunken Charge";
                    break;
                case "Sandbat":
                    e = new Enemy("Sandbat", i + 1, level, 60, 7, 22, 8, 5, 7, 2);
                    skillName = "Sand Spray";
                    break;
                case "Koy Vamp":
                    e = new Enemy("Koy Vamp", i + 1, level, 40, 12, 9, 11, 3, 3, 2);
                    skillName = "Vampiric Gnaw";
                    break;
                case "Sledger":
                    e = new Enemy("Sledger", i + 1, level, 42, 13, 5, 10, 12, 6, 0);
                    skillName = "Hammer Down";
                    break;
                case "Big Tox":
                    e = new Enemy("Big Tox", i + 1, level, 55, 8, 16, 13, 16, 3, 2);
                    skillName = "Mysterious Dew-hickey";
                    break;
                case "Unskilled":
                    e = new Enemy("Unskilled", i + 1, level, 53, 10, 15, 11, 5, 4, 2);
                    skillName = "Skilln't";
                    break;
                default:
                    e = new Enemy("Slime", i + 1, level, 45, 12, 8, 11, 5, 8, 1);
                    skillName = "Goo Shot";
                    break;

            }
            s = SkillsRegistry.getSkill(skillName);
            s.stacks = (int)(Game.floorNumber / 3);
            e.hp_ex += hp_p;
            e.atk_ex += atk_p;
            e.def_ex += def_p;
            e.skills.Add(s);
            e.skill1 = s;
            e.verifyMod();
            e.health = e.maxhealth;
            enemies.Add(e);
        }
        
        
        foreach (Character c in Game.characters) {
            c.resetMod();
            c.verifyMod();
            c.av = avNumerator / c.currentSPD;
        }

        foreach (Character c in enemies) {
            c.resetMod();
            c.verifyMod();
            c.av = avNumerator / c.currentSPD;
        }

        SceneManager.LoadScene("Battle Scene");
    }

    public static void StartBattle(int numEnemies) {
        StartBattle(numEnemies, 0, 1);
    }

    private void UpdateAVCycle() {
        // Rebuilding the Cycle using Action Values
        cycle.Clear();

        List<Character> aliveList = new List<Character>();
        foreach (Character c in Game.characters) {
            if (c.health > 0) {
                aliveList.Add(c);
            }
        }
        foreach (Character c in enemies) {
            if (c.health > 0) {
                aliveList.Add(c);
            }
        }

        while (aliveList.Any()) {
            Character fastest = aliveList[0];
            foreach (Character test in aliveList){
                test.verifyMod();
                if (fastest.av > test.av) {
                    fastest = test;
                }
            }
            cycle.Add(fastest);
            aliveList.Remove(fastest);
        }

        if (cycle.Any()) {
            double avUsed = cycle[0].av;
            foreach (Character c in Game.characters) {
                if (c.health > 0) {
                    c.av -= avUsed;
                }
            }
            foreach (Character c in enemies) {
                if (c.health > 0) {
                    c.av -= avUsed;
                }
            }
            cycle[0].verifyMod();
            cycle[0].av = avNumerator / cycle[0].currentSPD;
        }

        StartCoroutine(Sleep(2));
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
            if (Game.characters[i].health > 0 && !Game.characters[i].skipTurn) {
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
            Game.characters[i].skipTurn = false;
        }
        
        if (enemies.Any()) {
            for (int i = 0; i < enemies.Count; i++) {
                if (enemies[i].health > 0 && !enemies[i].skipTurn) {
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
                enemies[i].skipTurn = false;
            }
        }

        StartCoroutine(Sleep(2));
        
    }

    // Start is called before the first frame update
    void Start()
    {

        if (!firstRun)
        {
            firstRun = true;
        }

        foreach (Enemy e in enemies)
        {
            GameObject VisualEnemyClone = Instantiate(enemyVisual, enemyVisualParent.transform.position, enemyVisualParent.transform.rotation);
            VisualEnemyClone.transform.SetParent(enemyVisualParent.transform);
            VisualEnemyClone.name = "TEMP VisualEnemy";

            EnemyVisual visualScript = VisualEnemyClone.GetComponent<EnemyVisual>();
            visualScript.enemy = e;

            VisualEnemyClone.SetActive(true);
            enemyTrashCan.Add(VisualEnemyClone);
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
            UpdateAVCycle();
            StatusesBox.SetActive(false);
            CharacterStatsBox.SetActive(false);
            EnemySkillBox.SetActive(false);
        }

        string turn = cycle[0].getName();
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

        if (cycle.Any()) {
            if (cycle[0] != null) {
                if (cycle[0].skill1 != null) {
                    action_OP2.text = ">>  Skill: " + cycle[0].skill1.skillName;
                } else {
                    action_OP2.text = ">>  Skill Not Set";
                }

                if (cycle[0].skill2 != null) {
                    action_OP3.text = ">>  Skill: " + cycle[0].skill2.skillName;
                } else {
                    action_OP3.text = ">>  Skill Not Set";
                }
            } 
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
            if (turnNumber == 1 || c.av <= 0) {
                displayCycle += "0 | " + c.getName() + "\n";
            } else {
                displayCycle += (int)(c.av / 15) + " | " + c.getName() + "\n";
            }
        }

        turnOrderDisplay.text = displayCycle;

        // CYCLE UPDATE
        if (!buttonCooldown && !animationCooldown && sel_phase >= -5) {
            if (isPlayerTurn()) {
                if (lastTurn != turn) {
                    sel_phase = 0;
                    lastTurn = turn;
                    int dmg = cycle[0].startTurn();
                    cycle[0].verifyMod();
                    if (dmg > 0) {
                        StartCoroutine(Damage(dmg));
                    }
                    if (cycle[0].health > 0) {
                        if (sel_phase != -2) {
                            sel_phase = 2;
                            var targetPos = CharacterBox.transform.position;
                            ActionBox.transform.position = CharacterBox.transform.position;
                            targetPos.x += 517;
                            StartCoroutine(Move(ActionBox, targetPos));
                        } else {
                            if (cycle[0].skipTurn) {
                                StartCoroutine(ShowStatus(1, cycle[0].getName() + " can't move!"));
                            } else {
                                StartCoroutine(ShowStatus(1, cycle[0].getName() + " is frozen and can't move!"));
                            }
                            cycle[0].skipTurn = false;
                        }
                    } else {
                        sel_phase = -2;
                    }

                } else if (sel_phase == 2) {

                    if (Input.GetKeyDown(KeyCode.K)) {
                        int dmg = 0;
                        for (int i = 0; i < enemies.Count(); i++) {
                            dmg += enemies[i].damage(enemies[i], 1000000);
                        }
                        StartCoroutine(Damage(dmg));
                        sel_phase = -2;
                        return;
                    }

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
                            Skills skill = SkillsRegistry.getSkill(cycle[0].skills, selectedAction);
                            if (skill == null) {
                                if (selectedAction.IndexOf("Skill: ") >= 0) {
                                    selectedAction = selectedAction.Substring(7);
                                    skill = SkillsRegistry.getSkill(cycle[0].skills, selectedAction);
                                }
                            }

                            if (skill != null) {
                                int dmg = 0;
                                bool usedSkill = false;
                                if (battleSP >= skill.spConsumption) {
                                    while (trashCan.Any()) {
                                        GameObject t = trashCan[0];
                                        trashCan.RemoveAt(0);
                                        Destroy(t);
                                    }
                                    if (skill.targetType == "Self") {
                                        battleSP -= skill.spConsumption;
                                        dmg = Skills.useSkill(cycle[0], skill);
                                        StartCoroutine(ShowSkillUse(1, cycle[0], skill));
                                        usedSkill = true;
                                    } else if (skill.targetType == "Team") {
                                        battleSP -= skill.spConsumption;
                                        foreach (Character c in Game.characters) {
                                            if (c.health > 0) {
                                                dmg += Skills.useSkill(cycle[0], skill, c);
                                            }
                                        }
                                        StartCoroutine(ShowSkillUse(1, cycle[0], skill));
                                        usedSkill = true;
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

                                if (usedSkill) {
                                    if (dmg > 0) {
                                        StartCoroutine(Damage(dmg));
                                        sel_phase = -2;
                                    } else {
                                        StartCoroutine(Sleep(2));
                                        sel_phase = -2;
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
                    Skills skill = SkillsRegistry.getSkill(cycle[0].skills, selectedAction);
                    if (skill == null) {
                        if (selectedAction.IndexOf("Skill: ")  >= 0) {
                            selectedAction = selectedAction.Substring(7);
                            skill = SkillsRegistry.getSkill(cycle[0].skills, selectedAction);
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
                        bool usedSkill = false;
                        if (skill != null) {
                            if (skill.targetType == "Enemy") {
                                if (enemies[sel_target].health > 0) {
                                    battleSP -= skill.spConsumption;
                                    dmg = Skills.useSkill(cycle[0], skill, enemies[sel_target]);
                                    StartCoroutine(ShowSkillUse(1, cycle[0], skill));
                                    usedSkill = true;
                                }
                            } else if (skill.targetType == "Ally") {
                                if (Game.characters[sel_target].health > 0) {
                                    battleSP -= skill.spConsumption;
                                    dmg = Skills.useSkill(cycle[0], skill, Game.characters[sel_target]);
                                    StartCoroutine(ShowSkillUse(1, cycle[0], skill));
                                    usedSkill = true;
                                }
                            } else if (skill.targetType == "Non-Self Ally") {
                                if (Game.characters[sel_target].health > 0 && Game.characters[sel_target] != cycle[0]) {
                                    battleSP -= skill.spConsumption;
                                    dmg = Skills.useSkill(cycle[0], skill, Game.characters[sel_target]);
                                    StartCoroutine(ShowSkillUse(1, cycle[0], skill));
                                    usedSkill = true;
                                }
                            } else if (skill.targetType == "Dead") {
                                if (Game.characters[sel_target].health <= 0 && Game.characters[sel_target] != cycle[0]) {
                                    battleSP -= skill.spConsumption;
                                    dmg = Skills.useSkill(cycle[0], skill, Game.characters[sel_target]);
                                    StartCoroutine(ShowSkillUse(1, cycle[0], skill));
                                    usedSkill = true;
                                }
                            }

                            if (usedSkill) {
                                if (dmg > 0) {
                                    StartCoroutine(Damage(dmg));
                                    sel_phase = -2;
                                } else {
                                    StartCoroutine(Sleep(2));
                                    sel_phase = -2;
                                }
                            }

                        }
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
                int dmg = 0;
                bool usedSkill = false;
                if (lastTurn != turn) {
                    lastTurn = turn;
                    sel_phase = 0;
                }

                if (sel_phase == 0) {
                    dmg = cycle[0].startTurn();
                    cycle[0].verifyMod();
                    if (dmg > 0) {
                        StartCoroutine(Damage(dmg));
                    }

                    if (cycle[0].health > 0) {
                        if (sel_phase != -2) {
                            sel_phase = 2;
                        } else {
                            if (cycle[0].skipTurn) {
                                StartCoroutine(ShowStatus(1, cycle[0].getName() + " can't move!"));
                            } else {
                                StartCoroutine(ShowStatus(1, cycle[0].getName() + " is frozen and can't move!"));
                            }
                            cycle[0].skipTurn = false;
                        }
                    } else {
                        sel_phase = -2;
                    }
                } else if (sel_phase == 2) {
                    Skills skillUsed = null;
                    if (cycle[0].skill1 != null) {
                        skillUsed = cycle[0].skill1;
                        if (cycle[0].sp >= skillUsed.spConsumption) {
                            cycle[0].sp -= skillUsed.spConsumption;
                            if (skillUsed.targetType == "Enemy") {
                                dmg = Skills.useSkill(cycle[0], skillUsed, Enemy.selectTarget(true));
                            } else if (skillUsed.targetType == "Self") {
                                dmg = Skills.useSkill(cycle[0], skillUsed);
                            } else if (skillUsed.targetType == "Enemies") {
                                foreach (Character t in Game.characters) {
                                    if (t.health > 0) {
                                        dmg += Skills.useSkill(cycle[0], skillUsed, t);
                                    }
                                }
                            }
                            usedSkill = true;
                        }
                    }

                    if (!usedSkill) {
                        skillUsed = SkillsRegistry.getSkill("Attack");
                        cycle[0].sp++;
                        dmg = Skills.useSkill(cycle[0], skillUsed, Enemy.selectTarget(true));
                    }
                    if (skillUsed != null) {
                        StartCoroutine(ShowSkillUse(1, cycle[0], skillUsed));
                    }
                    if (dmg > 0) {
                        StartCoroutine(Damage(dmg));
                    } else {
                        StartCoroutine(Sleep(2));
                    }
                    sel_phase = -2;
                } else if (sel_phase == -2) {
                    cycle[0].endTurn();
                    StartCoroutine(SuspendCycleChange(0));
                }
            }
        } else if (sel_phase == -10) {
            levelCheck++;
            if (Game.map[Game.row, Game.col].roomType == "Demon Room") {
                levelCheck = Game.characters.Count;
            }
            if (levelCheck >= Game.characters.Count) {
                sel_phase = -12;
                GameDescriptionBox.SetActive(false);
                StartCoroutine(SuspendSceneChange(2, "Overworld Scene"));
                Game.updateEnemies = true;
            } else if (levelCheck == -1) {
                int gold = 0;
                foreach (Enemy e in enemies) {
                    gold += e.level;
                }
                gold = (int) ((gold / 2 + 2) * multiplier);
                Game.gold += gold;
                Game.enemiesKilled += enemies.Count;
                GameDescription.text = "You earned " + gold + "G!";
                GameDescriptionBox.SetActive(true);
                sel_phase = -11;
            } else {
                if (Game.characters[levelCheck].health > 0) {
                    int xpGained = 0;
                    foreach (Enemy e in enemies) {
                        xpGained += e.level;
                    }
                    xpGained = (int) ((xpGained / 4 + 1) * multiplier);
                    Game.characters[levelCheck].xp += xpGained;
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
            if (Input.GetKeyDown(KeyCode.Z)) {
                if (CharacterStatsBox.activeSelf) {
                    statusPage++;
                    if (statusPage > cycle.Count() - 1) {
                        statusPage = 0;
                    }
                } else {
                    CharacterStatsBox.SetActive(true);
                    statusPage = 0;
                }
            }
            if (CharacterStatsBox.activeSelf) {
                if (statusPage <= cycle.Count() - 1) {
                    string currentStatuses = "";
                    int modCount = 0;
                    
                    foreach (Mod m in cycle[statusPage].hp_mod) {
                        currentStatuses += m.modName + ": " + m.value + " HP (" + m.duration + ") \n";
                        modCount++;
                    }

                    foreach (Mod m in cycle[statusPage].hp_p_mod) {
                        currentStatuses += m.modName + ": " + m.value + " HP% (" + m.duration + ") \n";
                        modCount++;
                    }

                    foreach (Mod m in cycle[statusPage].atk_mod) {
                        currentStatuses += m.modName + ": " + m.value + " ATK (" + m.duration + ") \n";
                        modCount++;
                    }

                    foreach (Mod m in cycle[statusPage].atk_p_mod) {
                        currentStatuses += m.modName + ": " + m.value + " ATK% (" + m.duration + ") \n";
                        modCount++;
                    }

                    foreach (Mod m in cycle[statusPage].def_mod) {
                        currentStatuses += m.modName + ": " + m.value + " DEF (" + m.duration + ") \n";
                        modCount++;
                    }

                    foreach (Mod m in cycle[statusPage].def_p_mod) {
                        currentStatuses += m.modName + ": " + m.value + " DEF% (" + m.duration + ") \n";
                        modCount++;
                    }

                    foreach (Mod m in cycle[statusPage].spd_mod) {
                        currentStatuses += m.modName + ": " + m.value + " SPD (" + m.duration + ") \n";
                        modCount++;
                    }

                    foreach (Mod m in cycle[statusPage].cr_mod) {
                        currentStatuses += m.modName + ": " + m.value + " CR% (" + m.duration + ") \n";
                        modCount++;
                    }

                    foreach (Mod m in cycle[statusPage].cd_mod) {
                        currentStatuses += m.modName + ": " + m.value + " CD% (" + m.duration + ") \n";
                        modCount++;
                    }

                    foreach (Mod m in cycle[statusPage].bleed) {
                        currentStatuses += m.modName + ": " + m.value + " BLEED (" + m.duration + ") \n";
                        modCount++;
                    }

                    foreach (Mod m in cycle[statusPage].freeze) {
                        currentStatuses += m.modName + ": " + m.value + " FREEZE (" + m.duration + ") \n";
                        modCount++;
                    }

                    if (modCount == 0) {
                        currentStatuses = "There are no statuses on them!";
                        StatusesBox.SetActive(false);
                    } else {
                        StatusesBox.SetActive(true);
                    }

                    Character statusCharacter = cycle[statusPage];

                    StatusesTitle.text = modCount + " Statuses";
                    Statuses.text = currentStatuses;

                    CharacterTitle.text = statusCharacter.getName();
                    string characterStatText = "Level: " + statusCharacter.level;
                    characterStatText += "\nHP: " + (statusCharacter.health) + " / " + (statusCharacter.maxhealth);
                    characterStatText += "\nATK: " + (statusCharacter.baseATK + statusCharacter.atk_ex + statusCharacter.gear.atk + (Character.L_UP_ATK * (statusCharacter.level-1))) + " > " + (statusCharacter.currentATK);
                    characterStatText += "\nDEF: " + (statusCharacter.baseDEF + statusCharacter.def_ex + statusCharacter.gear.def + (Character.L_UP_DEF * (statusCharacter.level-1))) + " > " + (statusCharacter.currentDEF);
                    characterStatText += "\nSPD: " + (statusCharacter.baseSPD + statusCharacter.spd_ex + statusCharacter.gear.spd) + " > " + (statusCharacter.currentSPD);
                    characterStatText += "\nCRIT RATE: " + (statusCharacter.baseCR + statusCharacter.cr_ex + statusCharacter.gear.cr) + " > " + (statusCharacter.currentCR);
                    characterStatText += "\nCRIT DMG: " + (statusCharacter.baseCD + statusCharacter.cd_ex + statusCharacter.gear.cd) + " > " + (statusCharacter.currentCD);
                    CharacterStats.text = characterStatText;

                    if (statusCharacter.isEnemy()) {
                        EnemySkillBox.SetActive(true);
                        EnemySkillDescription.text = statusCharacter.skill1.skillName + " (" + statusCharacter.skill1.stacks + " Stacks)" + ": " + statusCharacter.skill1.description;
                    } else {
                        EnemySkillBox.SetActive(false);
                    }
                }
            }
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
        cycle.Clear();
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

    IEnumerator ShowSkillUse(int seconds, Character character, Skills skill)
    {
        GameObject InfoClone = Instantiate(SkillUsageBox, new Vector3(CharacterBox.transform.position.x, CharacterBox.transform.position.y), CharacterBox.transform.rotation);
        InfoClone.transform.SetParent(SkillUsageBox.transform);
        InfoClone.name = "TEMP InfoBox";
        InfoClone.GetComponent<TextManager>().text = character.character + " uses " + skill.skillName + "!";
        InfoClone.SetActive(true);
        InfoClone.transform.position = CharacterBox.transform.position;
        var targetPos = CharacterBox.transform.position;
        targetPos.y = CharacterBox.transform.position.y + 200;
        while (InfoClone != null && (targetPos - InfoClone.transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            InfoClone.transform.position = Vector3.MoveTowards(InfoClone.transform.position, targetPos, slowerMoveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(seconds);

        if (InfoClone != null) {
            InfoClone.transform.position = targetPos;
        }
        Destroy(InfoClone);
    }

    IEnumerator ShowStatus(int seconds, string status)
    {
        GameObject InfoClone = Instantiate(SkillUsageBox, new Vector3(CharacterBox.transform.position.x, CharacterBox.transform.position.y), CharacterBox.transform.rotation);
        InfoClone.transform.SetParent(SkillUsageBox.transform);
        InfoClone.name = "TEMP InfoBox";
        InfoClone.GetComponent<TextManager>().text = status;
        InfoClone.SetActive(true);
        InfoClone.transform.position = CharacterBox.transform.position;
        var targetPos = CharacterBox.transform.position;
        targetPos.y = CharacterBox.transform.position.y + 200;
        while (InfoClone != null && (targetPos - InfoClone.transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            InfoClone.transform.position = Vector3.MoveTowards(InfoClone.transform.position, targetPos, slowerMoveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(seconds);

        if (InfoClone != null) {
            InfoClone.transform.position = targetPos;
        }
        Destroy(InfoClone);
    }

    
    
}
