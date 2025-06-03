using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using Unity.VisualScripting;

public class GameUIManager : MonoBehaviour
{

    public static float moveSpeed = 2000;
    public static float fastMoveSpeed = 3000;

    public GameObject MoneyBox;
    public Text MoneyIndicator;
    public GameObject OptionBox;
    public GameObject DescriptionBox;
    public Text D_1;
    public Text D_2;
    public Text D_3;
    public Text D_4;
    public GameObject ActionBox;
    public Text A_1;
    public Text A_2;
    public Text A_3;
    public Text A_4;
    public GameObject TargetBox;
    public Text T_1;
    public Text T_2;
    public Text T_3;
    public Text T_4;
    public GameObject InfoBox;
    public Text InfoIndicator;

    public static string menu_name;
    public static string menu_type;
    public static int menu_phase;
    public static int menu_selection;
    public static int menu_selection_limit;
    public static int menu_page;

    public static int action_selection;
    public static int action_selection_limit;

    public static int target_selection;
    public static int target_selection_limit;
    public static int target_page;

    public static string shrine;

    public bool updateOptions;
    public bool slideCooldown;
    public bool animationCooldown;

    List<GameObject> trashCan = new List<GameObject>();

    public void openMenu(string menuType) {
        menu_name = menuType;
        Room r = Game.map[Game.row, Game.col];

        ActionBox.transform.position = DescriptionBox.transform.position;
        TargetBox.transform.position = DescriptionBox.transform.position;
        emptyTrash();

        switch (menuType)
        {
            case "Teleporter":
                OptionBox.SetActive(true);
                D_1.text = "Would you like to go further down? You will not be able to come back here later...";
                D_2.text = "";
                D_3.text = "";
                D_4.text = "";
                A_1.text = ">> Yes";
                A_2.text = ">> No";
                A_3.text = "";
                A_4.text = "";
                menu_type = "Highlight";
                menu_phase = 1;
                menu_selection = -1;
                menu_selection_limit = -1;
                action_selection = 1;
                action_selection_limit = 1;
                target_selection = -1;
                target_selection_limit = -1;
                Game.gameMovementFreeze = true;
                break;
            case "Wizard":
                OptionBox.SetActive(true);
                D_1.text = "Welcome to my humble shop. I can teach you some skills for your future battles. For a price that is...";
                D_2.text = "";
                D_3.text = "";
                D_4.text = "";
                if (r.skillList[0] != null)
                {
                    A_1.text = ">> Skill: " + r.skillList[0].skillName;
                }
                else
                {
                    A_1.text = ">> [PURCHASED!]";
                }
                if (r.skillList[1] != null)
                {
                    A_2.text = ">> Skill: " + r.skillList[1].skillName;
                }
                else
                {
                    A_2.text = ">> [PURCHASED!]";
                }
                if (r.skillList[2] != null)
                {
                    A_3.text = ">> Skill: " + r.skillList[2].skillName;
                }
                else
                {
                    A_3.text = ">> [PURCHASED!]";
                }
                if (r.skillList[3] != null)
                {
                    A_4.text = ">> Skill: " + r.skillList[3].skillName;
                }
                else
                {
                    A_4.text = ">> [PURCHASED!]";
                }
                T_1.text = ">> Confirm Purchase";
                T_2.text = ">> Cancel Purchase";
                T_3.text = "";
                T_4.text = "";
                menu_type = "Highlight";
                menu_phase = 1;
                menu_selection = -1;
                menu_selection_limit = -1;
                action_selection = 0;
                action_selection_limit = 3;
                target_selection = 0;
                target_selection_limit = 1;
                MoneyBox.SetActive(true);
                Game.gameMovementFreeze = true;
                break;
            case "Break":
                OptionBox.SetActive(true);
                D_1.text = ">> " + Game.characters[0].character + " (HP: " + Game.characters[0].health + "/" + Game.characters[0].maxhealth + ")";
                D_2.text = ">> " + Game.characters[1].character + " (HP: " + Game.characters[1].health + "/" + Game.characters[1].maxhealth + ")";
                D_3.text = ">> " + Game.characters[2].character + " (HP: " + Game.characters[2].health + "/" + Game.characters[2].maxhealth + ")";
                D_4.text = ">> " + Game.characters[3].character + " (HP: " + Game.characters[3].health + "/" + Game.characters[3].maxhealth + ")";
                A_1.text = ">> Change Skill 1";
                A_2.text = ">> Change Skill 2";
                A_3.text = ">> Heal";
                A_4.text = "";
                T_1.text = "";
                T_2.text = "";
                T_3.text = "";
                T_4.text = "";
                menu_type = "Highlight";
                menu_phase = 0;
                menu_selection = 0;
                menu_selection_limit = 3;
                action_selection = 0;
                action_selection_limit = 2;
                target_selection = -1;
                target_selection_limit = -1;
                MoneyBox.SetActive(true);
                Game.gameMovementFreeze = true;
                break;
            case "Shrine":
                OptionBox.SetActive(true);
                if (r.wishes <= 0)
                {
                    D_1.text = "The remanants of a shrine stands before you. It calls for you to wish upon it.";
                    A_1.text = ">> Wish Upon It";
                    T_1.text = ">> Confirm Wish";
                    MoneyBox.SetActive(false);
                }
                else
                {
                    D_1.text = "The remanants of a shrine stands before you once again. It imposes a price this time.";
                    A_1.text = ">> Wish Upon It for " + getShrineCost() + "G";
                    T_1.text = ">> Confirm Wish for " + getShrineCost() + "G";
                    MoneyBox.SetActive(true);
                }
                D_2.text = "";
                D_3.text = "";
                D_4.text = "";
                A_2.text = ">> Leave";
                A_3.text = "";
                A_4.text = "";
                T_2.text = ">> Leave";
                T_3.text = "";
                T_4.text = "";
                menu_type = "Highlight";
                menu_phase = 1;
                menu_selection = -1;
                menu_selection_limit = -1;
                action_selection = 1;
                action_selection_limit = 1;
                target_selection = 1;
                target_selection_limit = 1;
                Game.gameMovementFreeze = true;
                break;
            case "Demon":
                OptionBox.SetActive(true);
                D_1.text = "You were fooled by a trap set up by the cave itself. Brace yourself for battle!";
                D_2.text = "";
                D_3.text = "";
                D_4.text = "";
                A_1.text = ">> No other choice but to fight!";
                A_2.text = ">> No other choice but to fight!";
                A_3.text = ">> No other choice but to fight!";
                A_4.text = ">> No other choice but to fight!";
                T_1.text = "";
                T_2.text = "";
                T_3.text = "";
                T_4.text = "";
                menu_type = "Highlight";
                menu_phase = 1;
                menu_selection = -1;
                menu_selection_limit = -1;
                action_selection = 0;
                action_selection_limit = 3;
                target_selection = -1;
                target_selection_limit = -1;
                Game.gameMovementFreeze = true;
                break;
            case "Gear":
                if (Game.map[Game.row, Game.col].wishes == 0)
                {
                    OptionBox.SetActive(true);
                    D_1.text = "The lava acknowledges your power. For your first time arriving here, it gives you gear and an upgrade point.";
                    D_2.text = "";
                    D_3.text = "";
                    D_4.text = "";
                    A_1.text = "";
                    A_2.text = "";
                    A_3.text = "";
                    A_4.text = "";
                    T_1.text = "";
                    T_2.text = "";
                    T_3.text = "";
                    T_4.text = "";
                    menu_type = "Highlight";
                    menu_phase = 20;
                    menu_selection = -1;
                    menu_selection_limit = -1;
                    action_selection = -1;
                    action_selection_limit = -1;
                    target_selection = -1;
                    target_selection_limit = -1;
                    Game.gameMovementFreeze = true;
                    Gear.wallet += 1;
                    for (int i = 0; i < 1; i++)
                    {
                        Game.gear.Add(new Gear(true));
                    }
                }
                else
                {
                    OptionBox.SetActive(true);
                    D_1.text = "";
                    D_2.text = "";
                    D_3.text = "";
                    D_4.text = "";
                    A_1.text = ">> Equip to Character";
                    A_2.text = ">> Power Up for 1 Point";
                    A_3.text = ">> Discard Gear";
                    A_4.text = "";
                    T_1.text = "";
                    T_2.text = "";
                    T_3.text = "";
                    T_4.text = "";
                    updateOptions = true;
                    menu_type = "Highlight";
                    menu_phase = 0;
                    menu_selection = 0;
                    menu_selection_limit = -1;
                    action_selection = 0;
                    action_selection_limit = 1;
                    target_selection = -1;
                    target_selection_limit = -1;
                    Game.gameMovementFreeze = true;
                }
                break;

            case "Stats":
                OptionBox.SetActive(true);
                D_1.text = ">> " + Game.characters[0].character + " (HP: " + Game.characters[0].health + "/" + Game.characters[0].maxhealth + ")";
                D_2.text = ">> " + Game.characters[1].character + " (HP: " + Game.characters[1].health + "/" + Game.characters[1].maxhealth + ")";
                D_3.text = ">> " + Game.characters[2].character + " (HP: " + Game.characters[2].health + "/" + Game.characters[2].maxhealth + ")";
                D_4.text = ">> " + Game.characters[3].character + " (HP: " + Game.characters[3].health + "/" + Game.characters[3].maxhealth + ")";
                A_1.text = ">> Change Skill 1";
                A_2.text = ">> Change Skill 2";
                A_3.text = ">> Check Gear";
                A_4.text = "";
                T_1.text = "";
                T_2.text = "";
                T_3.text = "";
                T_4.text = "";
                menu_type = "Highlight";
                menu_phase = 0;
                menu_selection = 0;
                menu_selection_limit = 3;
                action_selection = 0;
                action_selection_limit = 2;
                target_selection = -1;
                target_selection_limit = -1;
                MoneyBox.SetActive(true);
                Game.gameMovementFreeze = true;
                updateOptions = true;
                break;
        }

        emptyTrash();

    }

    public void closeMenu() {
        OptionBox.SetActive(false);
        ActionBox.transform.position = DescriptionBox.transform.position;
        D_1.text = "";
        D_2.text = "";
        D_3.text = "";
        D_4.text = "";
        A_1.text = "";
        A_2.text = "";
        A_3.text = "";
        A_4.text = "";
        T_1.text = "";
        T_2.text = "";
        T_3.text = "";
        T_4.text = "";
        menu_type = "";
        menu_phase = -1;
        menu_selection = -1;
        action_selection = -1;
        action_selection_limit = -1;
        target_selection = -1;
        target_selection_limit = -1;
        MoneyBox.SetActive(false);
        emptyTrash();
    }

    private int updateSelection(ref int option, int optionLimit) {
        int hx = GetHorizontal();
        int hy = GetVertical();

        if (hx > 0 || hy > 0) {
            updateOptions = true;
            option++;
            if (option > optionLimit) {
                option = 0;
                if (menu_phase == 4) {
                    target_page++;
                }
                if (menu_phase == 0) {
                    menu_page++;
                }
                return 1;
            }
        } else if (hx < 0 || hy < 0) {
            updateOptions = true;
            option--;
            if (option < 0) {
                option = optionLimit;
                if (menu_phase == 4) {
                    target_page--;
                }
                if (menu_phase == 0) {
                    menu_page--;
                }
                return -1;
            }
        }

        return 0;

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

    private int getConfirmation() {
        if (Input.GetKeyDown(KeyCode.C)) {
            return 1;
        } else if (Input.GetKeyDown(KeyCode.X)) {
            return -1;
        } else {
            return 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        MoneyIndicator.text = Game.gold + " G";
        D_1.fontStyle = FontStyle.Normal;
        D_2.fontStyle = FontStyle.Normal;
        D_3.fontStyle = FontStyle.Normal;
        D_4.fontStyle = FontStyle.Normal;
        A_1.fontStyle = FontStyle.Normal;
        A_2.fontStyle = FontStyle.Normal;
        A_3.fontStyle = FontStyle.Normal;
        A_4.fontStyle = FontStyle.Normal;
        T_1.fontStyle = FontStyle.Normal;
        T_2.fontStyle = FontStyle.Normal;
        T_3.fontStyle = FontStyle.Normal;
        T_4.fontStyle = FontStyle.Normal;

        if (menu_type == "Highlight") {
            switch (menu_selection) {
                case 0:
                    D_1.fontStyle = FontStyle.Bold;
                    break;
                case 1:
                    D_2.fontStyle = FontStyle.Bold;
                    break;
                case 2:
                    D_3.fontStyle = FontStyle.Bold;
                    break;
                case 3:
                    D_4.fontStyle = FontStyle.Bold;
                    break;
            }
            switch (action_selection) {
                case 0:
                    A_1.fontStyle = FontStyle.Bold;
                    break;
                case 1:
                    A_2.fontStyle = FontStyle.Bold;
                    break;
                case 2:
                    A_3.fontStyle = FontStyle.Bold;
                    break;
                case 3:
                    A_4.fontStyle = FontStyle.Bold;
                    break;
            }
            switch (target_selection) {
                case 0:
                    T_1.fontStyle = FontStyle.Bold;
                    break;
                case 1:
                    T_2.fontStyle = FontStyle.Bold;
                    break;
                case 2:
                    T_3.fontStyle = FontStyle.Bold;
                    break;
                case 3:
                    T_4.fontStyle = FontStyle.Bold;
                    break;
            }
        }

        if (!slideCooldown && !animationCooldown) {
            if (menu_phase == 0) {
                ActionBox.transform.position = DescriptionBox.transform.position;
                TargetBox.transform.position = DescriptionBox.transform.position;
                int pageUpdate = updateSelection(ref menu_selection, menu_selection_limit);

                int confirm = getConfirmation();
                switch (menu_name)
                {
                    case "Break":
                        if (confirm == 1) {
                            A_3.text = ">> Heal for " + (5 * Game.characters[menu_selection].level) + "G";
                            menu_phase = 1;
                            action_selection = 0;
                        } else if (confirm == -1) {
                            closeMenu();
                            emptyTrash();
                            Game.gameMovementFreeze = false;
                        }
                        break;
                    case "Gear":
                        int maxPage = 0;
                        if (Game.gear.Count <= 0)
                        {
                            menu_selection = -2;
                            menu_selection_limit = -2;
                            D_1.text = "You have no gear to choose from. Come back when you have some!";
                            D_2.text = "";
                            D_3.text = "";
                            D_4.text = "";
                        }
                        else if (Game.gear.Count > 0)
                        {
                            maxPage = (Game.gear.Count - 1) / 4;
                            if (menu_page < 0)
                            {
                                menu_page = maxPage;
                                menu_selection = (Game.gear.Count) % 4 - 1;
                            }
                            else if (menu_page > maxPage)
                            {
                                menu_page = 0;
                                menu_selection = 0;
                            }

                            if ((Game.gear.Count) >= (menu_page * 4) + 1)
                            {
                                D_1.text = ">> " + Game.gear[(menu_page * 4) + 0].GetDisplayName();
                                menu_selection_limit = 0;
                                if (pageUpdate == -1)
                                {
                                    menu_selection = menu_selection_limit;
                                }
                            }
                            else
                            {
                                D_1.text = "";
                            }

                            if ((Game.gear.Count) >= (menu_page * 4) + 2)
                            {
                                D_2.text = ">> " + Game.gear[(menu_page * 4) + 1].GetDisplayName();
                                menu_selection_limit = 1;
                                if (pageUpdate == -1)
                                {
                                    menu_selection = menu_selection_limit;
                                }
                            }
                            else
                            {
                                D_2.text = "";
                            }

                            if ((Game.gear.Count) >= (menu_page * 4) + 3)
                            {
                                D_3.text = ">> " + Game.gear[(menu_page * 4) + 2].GetDisplayName();
                                menu_selection_limit = 2;
                                if (pageUpdate == -1)
                                {
                                    menu_selection = menu_selection_limit;
                                }
                            }
                            else
                            {
                                D_3.text = "";
                            }

                            if ((Game.gear.Count) >= (menu_page * 4) + 4)
                            {
                                D_4.text = ">> " + Game.gear[(menu_page * 4) + 3].GetDisplayName();
                                menu_selection_limit = 3;
                                if (pageUpdate == -1)
                                {
                                    menu_selection = menu_selection_limit;
                                }
                            }
                            else
                            {
                                D_4.text = "";
                            }

                        }
                        if (confirm == 1 && menu_selection >= 0)
                        {
                            menu_phase = 1;
                            action_selection = 0;
                            break;
                        }
                        if (confirm == -1)
                        {
                            closeMenu();
                            emptyTrash();
                            Game.gameMovementFreeze = false;
                        }

                        if (updateOptions && menu_phase == 0)
                        {
                            emptyTrash();

                            if (Game.gear.Count > (menu_page * 4) + menu_selection && menu_selection >= 0)
                            {
                                if (Game.gear[(menu_page * 4) + menu_selection] != null)
                                {
                                    GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(DescriptionBox.transform.position.x, DescriptionBox.transform.position.y), DescriptionBox.transform.rotation);
                                    InfoBoxClone.transform.SetParent(InfoBox.transform);
                                    InfoBoxClone.name = "TEMP InfoBox" + menu_selection;
                                    InfoBoxClone.GetComponent<TextManager>().text = Game.gear[menu_page * 4 + menu_selection].GetValues();
                                    InfoBoxClone.GetComponent<TextManager>().textBox.lineSpacing = 0.8f;
                                    trashCan.Add(InfoBoxClone);
                                    var targetPos = DescriptionBox.transform.position;
                                    targetPos.x += 517;
                                    StartCoroutine(Slide(InfoBoxClone, targetPos, false));
                                    updateOptions = false;
                                }
                            }
                        }
                        break;

                    case "Stats":
                        if (confirm == 1 && menu_selection >= 0) {
                            menu_phase = 1;
                            action_selection = 0;
                            break;
                        }
                        if (confirm == -1) {
                            closeMenu();
                            emptyTrash();
                            Game.gameMovementFreeze = false;
                        }

                        if (updateOptions && menu_phase == 0) {
                            emptyTrash();
                            GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(DescriptionBox.transform.position.x, DescriptionBox.transform.position.y), DescriptionBox.transform.rotation);
                            InfoBoxClone.transform.SetParent(InfoBox.transform);
                            InfoBoxClone.name = "TEMP InfoBox" + menu_selection;
                            InfoBoxClone.GetComponent<TextManager>().text = Game.characters[menu_selection].getStats();
                            InfoBoxClone.GetComponent<TextManager>().textBox.lineSpacing = 0.8f;
                            trashCan.Add(InfoBoxClone);
                            var targetPos = DescriptionBox.transform.position;
                            targetPos.x += 517;
                            StartCoroutine(Slide(InfoBoxClone, targetPos, false));
                            updateOptions = false;
                        }
                        break;
                }
            } else if (menu_phase == 1) {
                var targetPos = DescriptionBox.transform.position;
                ActionBox.transform.position = DescriptionBox.transform.position;
                targetPos.x += 517;
                StartCoroutine(Slide(ActionBox, targetPos));
                
                updateOptions = true;
                emptyTrash();

            } else if (menu_phase == 2) {

                TargetBox.transform.position = DescriptionBox.transform.position;
                int pageUpdate = updateSelection(ref action_selection, action_selection_limit);

                int confirm = getConfirmation();
                switch (menu_name)
                {
                    case "Teleporter":
                        if (confirm == 1)
                        {
                            if (action_selection == 0)
                            {
                                SceneManager.LoadScene("Debrief Scene");
                            }
                            else if (action_selection == 1)
                            {
                                confirm = -1;
                            }
                        }
                        if (confirm == -1)
                        {
                            closeMenu();
                            emptyTrash();
                            Game.gameMovementFreeze = false;
                        }
                        break;
                    case "Wizard":
                        if (confirm == 1)
                        {
                            if (Game.map[Game.row, Game.col].skillList[action_selection] != null)
                            {
                                int cost = TextManager.getSkillCost(Game.map[Game.row, Game.col].skillList[action_selection]);
                                if (Game.gold >= cost)
                                {
                                    menu_phase = 3;
                                    T_1.text = ">> Confirm Purchase for " + cost + "G";
                                    target_selection = 1;
                                }
                            }

                        }
                        if (confirm == -1)
                        {
                            closeMenu();
                            emptyTrash();
                            Game.gameMovementFreeze = false;
                        }

                        if (updateOptions && menu_phase == 2)
                        {
                            emptyTrash();

                            if (Game.map[Game.row, Game.col].skillList[action_selection] != null)
                            {
                                GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(ActionBox.transform.position.x, ActionBox.transform.position.y), ActionBox.transform.rotation);
                                InfoBoxClone.transform.SetParent(InfoBox.transform);
                                InfoBoxClone.name = "TEMP InfoBox" + action_selection;
                                InfoBoxClone.GetComponent<TextManager>().text = TextManager.convertSkillShop(Game.map[Game.row, Game.col].skillList[action_selection]);
                                trashCan.Add(InfoBoxClone);
                                var targetPos = ActionBox.transform.position;
                                targetPos.x += 517;
                                StartCoroutine(Slide(InfoBoxClone, targetPos, false));
                                updateOptions = false;
                            }
                        }


                        break;
                    case "Break":
                        if (confirm == 1)
                        {
                            switch (action_selection)
                            {
                                case 0 or 1:
                                    target_page = 0;
                                    target_selection = 0;
                                    target_selection_limit = 0;
                                    menu_phase = 3;
                                    break;
                                case 2:
                                    if (Game.gold >= 5 * Game.characters[menu_selection].level)
                                    {
                                        target_selection = 1;
                                        target_selection_limit = 1;
                                        menu_phase = 3;
                                        T_1.text = ">> Confirm Purchase";
                                        T_2.text = ">> Cancel Purchase";
                                        T_3.text = "";
                                        T_4.text = "";
                                    }
                                    break;
                            }
                        }
                        if (confirm == -1)
                        {
                            emptyTrash();
                            menu_phase = 0;
                        }

                        if (updateOptions && menu_phase == 2)
                        {
                            emptyTrash();

                            if (action_selection == 0 || action_selection == 1)
                            {
                                Character c = Game.characters[menu_selection];
                                Skills s = null;

                                if (c.skills.Count <= 2)
                                {
                                    target_selection = -2;
                                    target_selection_limit = -2;
                                    T_1.text = "You have no skills to choose from. Learn a few from the Wizard!";
                                    T_2.text = "";
                                    T_3.text = "";
                                    T_4.text = "";
                                }
                                else if (c.skills.Count > 2)
                                {
                                    target_page = 0;

                                    if ((c.skills.Count - 2) >= (target_page * 4) + 1)
                                    {
                                        T_1.text = ">> " + c.skills[(target_page * 4) + 2].skillName;
                                        target_selection_limit = 0;
                                    }
                                    else
                                    {
                                        T_1.text = "";
                                    }

                                    if ((c.skills.Count - 2) >= (target_page * 4) + 2)
                                    {
                                        T_2.text = ">> " + c.skills[(target_page * 4) + 3].skillName;
                                        target_selection_limit = 1;
                                    }
                                    else
                                    {
                                        T_2.text = "";
                                    }

                                    if ((c.skills.Count - 2) >= (target_page * 4) + 3)
                                    {
                                        T_3.text = ">> " + c.skills[(target_page * 4) + 4].skillName;
                                        target_selection_limit = 2;
                                    }
                                    else
                                    {
                                        T_3.text = "";
                                    }

                                    if ((c.skills.Count - 2) >= (target_page * 4) + 4)
                                    {
                                        T_4.text = ">> " + c.skills[(target_page * 4) + 5].skillName;
                                        target_selection_limit = 3;
                                    }
                                    else
                                    {
                                        T_4.text = "";
                                    }

                                }

                                if (action_selection == 0 && c.skill1 != null)
                                {
                                    s = c.skill1;
                                }
                                if (action_selection == 1 && c.skill2 != null)
                                {
                                    s = c.skill2;
                                }

                                if (s != null)
                                {
                                    GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(ActionBox.transform.position.x, ActionBox.transform.position.y), ActionBox.transform.rotation);
                                    InfoBoxClone.transform.SetParent(InfoBox.transform);
                                    InfoBoxClone.name = "TEMP InfoBox" + action_selection;
                                    InfoBoxClone.GetComponent<TextManager>().text = TextManager.convertSkillDescription(c, s.skillName);
                                    trashCan.Add(InfoBoxClone);
                                    var targetPos = ActionBox.transform.position;
                                    targetPos.x += 517;
                                    StartCoroutine(Slide(InfoBoxClone, targetPos, false));
                                    updateOptions = false;
                                }
                                else
                                {
                                    GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(ActionBox.transform.position.x, ActionBox.transform.position.y), ActionBox.transform.rotation);
                                    InfoBoxClone.transform.SetParent(InfoBox.transform);
                                    InfoBoxClone.name = "TEMP InfoBox" + action_selection;
                                    InfoBoxClone.GetComponent<TextManager>().text = "This skill has not been selected yet. Learn skills from the Wizard and equip them here!";
                                    trashCan.Add(InfoBoxClone);
                                    var targetPos = ActionBox.transform.position;
                                    targetPos.x += 517;
                                    StartCoroutine(Slide(InfoBoxClone, targetPos, false));
                                    updateOptions = false;
                                }
                            }
                        }
                        break;

                    case "Shrine":
                        if (confirm == 1)
                        {
                            switch (action_selection)
                            {
                                case 0:
                                    if (Game.gold >= getShrineCost())
                                    {
                                        menu_phase = 3;
                                    }
                                    break;
                                case 1:
                                    confirm = -1;
                                    break;
                            }
                        }
                        if (confirm == -1)
                        {
                            closeMenu();
                            emptyTrash();
                            Game.gameMovementFreeze = false;
                        }
                        break;

                    case "Demon":
                        if (confirm == 1)
                        {
                            closeMenu();
                            emptyTrash();
                            Game.gameMovementFreeze = true;
                            Battle.StartBattle(Game.getEnemyCount() + 2, 2, 0);
                            Game.map[Game.row, Game.col].roomType = "Empty Room";
                        }
                        break;

                    case "Gear":
                        if (confirm == 1)
                        {
                            switch (action_selection)
                            {
                                case 0:
                                    T_1.text = ">> " + Game.characters[0].character;
                                    T_2.text = ">> " + Game.characters[1].character;
                                    T_3.text = ">> " + Game.characters[2].character;
                                    T_4.text = ">> " + Game.characters[3].character;
                                    target_selection = 0;
                                    target_selection_limit = 3;
                                    menu_phase = 3;
                                    break;
                                case 1:
                                    if (Gear.wallet > 0)
                                    {
                                        T_1.text = ">> Confirm Purchase for 1 Point";
                                        T_2.text = ">> Cancel";
                                        T_3.text = "";
                                        T_4.text = "";
                                        target_selection = 1;
                                        target_selection_limit = 1;
                                        menu_phase = 3;
                                    }
                                    break;
                                case 2:
                                    T_1.text = ">> Discard Item Forever";
                                    T_2.text = ">> Cancel";
                                    T_3.text = "";
                                    T_4.text = "";
                                    target_selection = 1;
                                    target_selection_limit = 1;
                                    menu_phase = 3;
                                    break;
                            }
                        }
                        if (confirm == -1)
                        {
                            closeMenu();
                            emptyTrash();
                            Game.gameMovementFreeze = false;
                        }
                        break;

                    case "Stats":
                        if (confirm == 1) {
                            if (action_selection == 0 || action_selection == 1) {
                                target_page = 0;
                                target_selection = 0;
                                target_selection_limit = 0;
                                menu_phase = 3;
                                break;
                            }
                        }
                        if (confirm == -1)
                        {
                            emptyTrash();
                            menu_phase = 0;
                            updateOptions = true;
                        }

                        if (updateOptions && menu_phase == 2) {
                            emptyTrash();

                            if (action_selection == 0 || action_selection == 1)
                            {
                                Character c = Game.characters[menu_selection];
                                Skills s = null;

                                if (c.skills.Count <= 2)
                                {
                                    target_selection = -2;
                                    target_selection_limit = -2;
                                    T_1.text = "You have no skills to choose from. Learn a few from the Wizard!";
                                    T_2.text = "";
                                    T_3.text = "";
                                    T_4.text = "";
                                }
                                else if (c.skills.Count > 2)
                                {
                                    target_page = 0;

                                    if ((c.skills.Count - 2) >= (target_page * 4) + 1)
                                    {
                                        T_1.text = ">> " + c.skills[(target_page * 4) + 2].skillName;
                                        target_selection_limit = 0;
                                    }
                                    else
                                    {
                                        T_1.text = "";
                                    }

                                    if ((c.skills.Count - 2) >= (target_page * 4) + 2)
                                    {
                                        T_2.text = ">> " + c.skills[(target_page * 4) + 3].skillName;
                                        target_selection_limit = 1;
                                    }
                                    else
                                    {
                                        T_2.text = "";
                                    }

                                    if ((c.skills.Count - 2) >= (target_page * 4) + 3)
                                    {
                                        T_3.text = ">> " + c.skills[(target_page * 4) + 4].skillName;
                                        target_selection_limit = 2;
                                    }
                                    else
                                    {
                                        T_3.text = "";
                                    }

                                    if ((c.skills.Count - 2) >= (target_page * 4) + 4)
                                    {
                                        T_4.text = ">> " + c.skills[(target_page * 4) + 5].skillName;
                                        target_selection_limit = 3;
                                    }
                                    else
                                    {
                                        T_4.text = "";
                                    }

                                }

                                if (action_selection == 0 && c.skill1 != null)
                                {
                                    s = c.skill1;
                                }
                                if (action_selection == 1 && c.skill2 != null)
                                {
                                    s = c.skill2;
                                }

                                if (s != null) {
                                    GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(ActionBox.transform.position.x, ActionBox.transform.position.y), ActionBox.transform.rotation);
                                    InfoBoxClone.transform.SetParent(InfoBox.transform);
                                    InfoBoxClone.name = "TEMP InfoBox" + action_selection;
                                    InfoBoxClone.GetComponent<TextManager>().text = TextManager.convertSkillDescription(c, s.skillName);
                                    trashCan.Add(InfoBoxClone);
                                    var targetPos = ActionBox.transform.position;
                                    targetPos.x += 517;
                                    StartCoroutine(Slide(InfoBoxClone, targetPos, false));
                                    updateOptions = false;
                                } else {
                                    GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(ActionBox.transform.position.x, ActionBox.transform.position.y), ActionBox.transform.rotation);
                                    InfoBoxClone.transform.SetParent(InfoBox.transform);
                                    InfoBoxClone.name = "TEMP InfoBox" + action_selection;
                                    InfoBoxClone.GetComponent<TextManager>().text = "This skill has not been selected yet. Learn skills from the Wizard and equip them here!";
                                    trashCan.Add(InfoBoxClone);
                                    var targetPos = ActionBox.transform.position;
                                    targetPos.x += 517;
                                    StartCoroutine(Slide(InfoBoxClone, targetPos, false));
                                    updateOptions = false;
                                }
                            } else if (action_selection == 2) {
                                Character c = Game.characters[menu_selection];
                                if (c.gear.id != 0)
                                {
                                    GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(ActionBox.transform.position.x, ActionBox.transform.position.y), ActionBox.transform.rotation);
                                    InfoBoxClone.transform.SetParent(InfoBox.transform);
                                    InfoBoxClone.name = "TEMP InfoBox" + action_selection;
                                    InfoBoxClone.GetComponent<TextManager>().text = c.gear.GetValues();
                                    InfoBoxClone.GetComponent<TextManager>().textBox.lineSpacing = 0.8f;
                                    trashCan.Add(InfoBoxClone);
                                    var targetPos = ActionBox.transform.position;
                                    targetPos.x += 517;
                                    StartCoroutine(Slide(InfoBoxClone, targetPos, false));
                                    updateOptions = false;
                                }
                                else
                                {
                                    GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(ActionBox.transform.position.x, ActionBox.transform.position.y), ActionBox.transform.rotation);
                                    InfoBoxClone.transform.SetParent(InfoBox.transform);
                                    InfoBoxClone.name = "TEMP InfoBox" + action_selection;
                                    InfoBoxClone.GetComponent<TextManager>().text = "There is no gear equipped right now!. Find and equip them at a gear pool!";
                                    trashCan.Add(InfoBoxClone);
                                    var targetPos = ActionBox.transform.position;
                                    targetPos.x += 517;
                                    StartCoroutine(Slide(InfoBoxClone, targetPos, false));
                                    updateOptions = false;
                                }  
                            }
                        }

                        break;
                }
            } else if (menu_phase == 3) {
                var targetPos = ActionBox.transform.position;
                TargetBox.transform.position = ActionBox.transform.position;
                targetPos.x += 517;
                StartCoroutine(Slide(TargetBox, targetPos));

                updateOptions = true;
                emptyTrash();

            } else if (menu_phase == 4) {
                int pageUpdate = updateSelection(ref target_selection, target_selection_limit);

                int confirm = getConfirmation();
                switch (menu_name)
                {
                    case "Wizard":
                        if (confirm == 1)
                        {
                            switch (target_selection)
                            {
                                case 0:
                                    Room r = Game.map[Game.row, Game.col];
                                    int cost = TextManager.getSkillCost(r.skillList[action_selection]);
                                    Game.gold -= cost;
                                    Character.addSkill(r.skillList[action_selection]);
                                    r.skillList[action_selection] = null;
                                    if (r.skillList[0] != null)
                                    {
                                        A_1.text = ">> Skill: " + r.skillList[0].skillName;
                                    }
                                    else
                                    {
                                        A_1.text = ">> [PURCHASED!]";
                                    }
                                    if (r.skillList[1] != null)
                                    {
                                        A_2.text = ">> Skill: " + r.skillList[1].skillName;
                                    }
                                    else
                                    {
                                        A_2.text = ">> [PURCHASED!]";
                                    }
                                    if (r.skillList[2] != null)
                                    {
                                        A_3.text = ">> Skill: " + r.skillList[2].skillName;
                                    }
                                    else
                                    {
                                        A_3.text = ">> [PURCHASED!]";
                                    }
                                    if (r.skillList[3] != null)
                                    {
                                        A_4.text = ">> Skill: " + r.skillList[3].skillName;
                                    }
                                    else
                                    {
                                        A_4.text = ">> [PURCHASED!]";
                                    }
                                    menu_phase = 2;
                                    break;
                                case 1:
                                    confirm = -1;
                                    break;
                            }
                        }
                        if (confirm == -1)
                        {
                            menu_phase = 2;
                        }
                        break;
                    case "Break":
                        switch (action_selection)
                        {
                            case 0 or 1:
                                Character c = Game.characters[menu_selection];
                                int maxPage = 0;
                                if (c.skills.Count <= 2)
                                {
                                    target_selection = -2;
                                    target_selection_limit = -2;
                                    T_1.text = "You have no skills to choose from. Learn a few from the Wizard!";
                                    T_2.text = "";
                                    T_3.text = "";
                                    T_4.text = "";
                                }
                                else if (c.skills.Count > 2)
                                {
                                    maxPage = (c.skills.Count - 3) / 4;
                                    if (target_page < 0)
                                    {
                                        target_page = maxPage;
                                        target_selection = (c.skills.Count - 2) % 4 - 1;
                                    }
                                    else if (target_page > maxPage)
                                    {
                                        target_page = 0;
                                        target_selection = 0;
                                    }

                                    if ((c.skills.Count - 2) >= (target_page * 4) + 1)
                                    {
                                        T_1.text = ">> " + c.skills[(target_page * 4) + 2].skillName;
                                        target_selection_limit = 0;
                                        if (pageUpdate == -1)
                                        {
                                            target_selection = target_selection_limit;
                                        }
                                    }
                                    else
                                    {
                                        T_1.text = "";
                                    }

                                    if ((c.skills.Count - 2) >= (target_page * 4) + 2)
                                    {
                                        T_2.text = ">> " + c.skills[(target_page * 4) + 3].skillName;
                                        target_selection_limit = 1;
                                        if (pageUpdate == -1)
                                        {
                                            target_selection = target_selection_limit;
                                        }
                                    }
                                    else
                                    {
                                        T_2.text = "";
                                    }

                                    if ((c.skills.Count - 2) >= (target_page * 4) + 3)
                                    {
                                        T_3.text = ">> " + c.skills[(target_page * 4) + 4].skillName;
                                        target_selection_limit = 2;
                                        if (pageUpdate == -1)
                                        {
                                            target_selection = target_selection_limit;
                                        }
                                    }
                                    else
                                    {
                                        T_3.text = "";
                                    }

                                    if ((c.skills.Count - 2) >= (target_page * 4) + 4)
                                    {
                                        T_4.text = ">> " + c.skills[(target_page * 4) + 5].skillName;
                                        target_selection_limit = 3;
                                        if (pageUpdate == -1)
                                        {
                                            target_selection = target_selection_limit;
                                        }
                                    }
                                    else
                                    {
                                        T_4.text = "";
                                    }

                                }
                                if (confirm == 1 && target_selection >= 0)
                                {
                                    switch (action_selection)
                                    {
                                        case 0:
                                            c.skill1 = c.skills[target_page * 4 + target_selection + 2];
                                            emptyTrash();
                                            updateOptions = true;
                                            menu_phase = 2;
                                            break;
                                        case 1:
                                            c.skill2 = c.skills[target_page * 4 + target_selection + 2];
                                            emptyTrash();
                                            updateOptions = true;
                                            menu_phase = 2;
                                            break;
                                    }
                                }
                                if (confirm == -1)
                                {
                                    emptyTrash();
                                    updateOptions = true;
                                    menu_phase = 2;
                                }

                                if (updateOptions && menu_phase == 4)
                                {
                                    emptyTrash();

                                    if (c.skills.Count > (target_page * 4) + target_selection + 2 && target_selection >= 0)
                                    {
                                        if (c.skills[(target_page * 4) + target_selection + 2] != null)
                                        {
                                            GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(TargetBox.transform.position.x, TargetBox.transform.position.y), TargetBox.transform.rotation);
                                            InfoBoxClone.transform.SetParent(InfoBox.transform);
                                            InfoBoxClone.name = "TEMP InfoBox" + target_selection;
                                            InfoBoxClone.GetComponent<TextManager>().text = TextManager.convertSkillDescription(c, c.skills[target_page * 4 + target_selection + 2].skillName);
                                            trashCan.Add(InfoBoxClone);
                                            var targetPos = TargetBox.transform.position;
                                            targetPos.y += 260;
                                            StartCoroutine(Slide(InfoBoxClone, targetPos, false));
                                            updateOptions = false;
                                        }
                                    }
                                }
                                break;
                            case 2:
                                if (confirm == 1)
                                {
                                    switch (target_selection)
                                    {
                                        case 0:
                                            if (Game.gold >= 5 * Game.characters[menu_selection].level)
                                            {
                                                Game.gold -= 5 * Game.characters[menu_selection].level;
                                                Game.characters[menu_selection].health = Game.characters[menu_selection].maxhealth;
                                                D_1.text = ">> " + Game.characters[0].character + " (HP: " + Game.characters[0].health + "/" + Game.characters[0].maxhealth + ")";
                                                D_2.text = ">> " + Game.characters[1].character + " (HP: " + Game.characters[1].health + "/" + Game.characters[1].maxhealth + ")";
                                                D_3.text = ">> " + Game.characters[2].character + " (HP: " + Game.characters[2].health + "/" + Game.characters[2].maxhealth + ")";
                                                D_4.text = ">> " + Game.characters[3].character + " (HP: " + Game.characters[3].health + "/" + Game.characters[3].maxhealth + ")";
                                                emptyTrash();
                                                menu_phase = 2;
                                            }
                                            break;
                                        case 1:
                                            confirm = -1;
                                            break;
                                    }
                                }
                                if (confirm == -1)
                                {
                                    emptyTrash();
                                    menu_phase = 2;
                                }
                                break;
                        }
                        break;

                    case "Shrine":
                        if (confirm == 1)
                        {
                            switch (target_selection)
                            {
                                case 0:
                                    if (Game.gold >= getShrineCost())
                                    {
                                        Game.gold -= getShrineCost();

                                        string text = "";
                                        List<string> shrines = new List<string>();
                                        shrines.Add("Nothing");
                                        shrines.Add("Nothing");
                                        shrines.Add("Refund");
                                        shrines.Add("Battle x2");
                                        shrines.Add("Battle x3");
                                        shrines.Add("Skill Up");
                                        shrines.Add("Heal");

                                        shrine = shrines[UnityEngine.Random.Range(0, shrines.Count)];

                                        switch (shrine)
                                        {
                                            case "Nothing":
                                                text = "You offer to the shrine, but gain nothing.";
                                                break;
                                            case "Refund":
                                                text = "It is unable to give anything in exchange, it simply gives you your money back.";
                                                break;
                                            case "Battle x2":
                                                text = "The shrine urges you to partake in a trial and it shall compensate you handsomely. Get ready for battle!";
                                                break;
                                            case "Battle x3":
                                                text = "The shrine's power overwhelms you. \"Fight this battle and you will see yourself in a greater light!\"";
                                                break;
                                            case "Skill Up":
                                                List<Skills> skills = new List<Skills>();
                                                foreach (Character c in Game.characters)
                                                {
                                                    foreach (Skills s in c.skills)
                                                    {
                                                        if (s.skillName != "Attack" && s.skillName != "Block")
                                                        {
                                                            skills.Add(s);
                                                        }
                                                    }
                                                }
                                                if (skills.Any())
                                                {
                                                    Skills skillUp = skills[UnityEngine.Random.Range(0, skills.Count)];
                                                    skillUp.stacks++;
                                                    text = "The shrine in its generosity enhanced the understanding of " + skillUp.skillUser + "\'s " + skillUp.skillName + "! One stack has been added to it.";
                                                }
                                                else
                                                {
                                                    text = "The shrine in its generosity offers you a chance to understand your skills better, but you haven't learned any.";
                                                }
                                                break;
                                            case "Heal":
                                                foreach (Character c in Game.characters)
                                                {
                                                    c.health = c.maxhealth;
                                                }
                                                text = "The shrine radiates with light. The next thing you know, all wounds you bore are gone.";
                                                break;
                                        }

                                        GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(TargetBox.transform.position.x, TargetBox.transform.position.y), TargetBox.transform.rotation);
                                        InfoBoxClone.transform.SetParent(InfoBox.transform);
                                        InfoBoxClone.name = "TEMP InfoBox" + target_selection;
                                        InfoBoxClone.GetComponent<TextManager>().text = text;
                                        trashCan.Add(InfoBoxClone);
                                        var targetPos = TargetBox.transform.position;
                                        targetPos.y += 260;
                                        StartCoroutine(Slide(InfoBoxClone, targetPos, false));

                                        menu_phase = 20;
                                    }
                                    break;
                                case 1:
                                    confirm = -1;
                                    break;
                            }
                        }
                        if (confirm == -1)
                        {
                            closeMenu();
                            emptyTrash();
                            Game.gameMovementFreeze = false;
                        }
                        break;

                    case "Gear":
                        Gear selectedGear = Game.gear[(menu_page * 4) + menu_selection];
                        if (confirm == 1)
                        {
                            Character c = Game.characters[target_selection];
                            if (action_selection == 0)
                            {
                                if (c.gear != null)
                                {
                                    c.gear.user = null;
                                }
                                if (selectedGear.user != null)
                                {
                                    selectedGear.user.gear = new Gear();
                                }
                                selectedGear.user = c;
                                c.gear = selectedGear;

                                GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(TargetBox.transform.position.x, TargetBox.transform.position.y), TargetBox.transform.rotation);
                                InfoBoxClone.transform.SetParent(InfoBox.transform);
                                InfoBoxClone.name = "TEMP InfoBox" + target_selection;
                                InfoBoxClone.GetComponent<TextManager>().text = "The gear is now equipped to " + c.character + "!";
                                trashCan.Add(InfoBoxClone);
                                var targetPos = TargetBox.transform.position;
                                targetPos.y += 260;
                                StartCoroutine(Slide(InfoBoxClone, targetPos, false));

                                menu_phase = 21;
                                foreach (Character chars in Game.characters)
                                {
                                    Debug.Log(chars.gear.GetDisplayName());
                                }
                            }
                            else if (action_selection == 1)
                            {
                                if (target_selection == 0)
                                {
                                    if (Gear.wallet > 0)
                                    {
                                        selectedGear.points++;
                                        selectedGear.levelUp(1);
                                        Gear.wallet--;

                                        GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(TargetBox.transform.position.x, TargetBox.transform.position.y), TargetBox.transform.rotation);
                                        InfoBoxClone.transform.SetParent(InfoBox.transform);
                                        InfoBoxClone.name = "TEMP InfoBox" + target_selection;
                                        InfoBoxClone.GetComponent<TextManager>().text = "The gear has been leveled up!";
                                        trashCan.Add(InfoBoxClone);
                                        var targetPos = TargetBox.transform.position;
                                        targetPos.y += 260;
                                        StartCoroutine(Slide(InfoBoxClone, targetPos, false));

                                        menu_phase = 21;
                                    }
                                }
                                else if (target_selection == 1)
                                {
                                    confirm = -1;
                                }

                            }
                            else if (action_selection == 2)
                            {
                                if (target_selection == 0)
                                {
                                    GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(TargetBox.transform.position.x, TargetBox.transform.position.y), TargetBox.transform.rotation);
                                    InfoBoxClone.transform.SetParent(InfoBox.transform);
                                    InfoBoxClone.name = "TEMP InfoBox" + target_selection;
                                    InfoBoxClone.GetComponent<TextManager>().text = "The gear is now discarded. You got " + selectedGear.points + " points back!";
                                    trashCan.Add(InfoBoxClone);
                                    var targetPos = TargetBox.transform.position;
                                    targetPos.y += 260;
                                    StartCoroutine(Slide(InfoBoxClone, targetPos, false));

                                    Gear.wallet += selectedGear.points;
                                    Game.gear.RemoveAt((menu_page * 4) + menu_selection);

                                    menu_phase = 21;
                                }
                                else if (target_selection == 1)
                                {
                                    confirm = -1;
                                }
                            }
                        }
                        if (confirm == -1)
                        {
                            emptyTrash();
                            menu_phase = 2;
                        }
                        break;
                    
                    case "Stats":
                        if (action_selection == 0 || action_selection == 1) {
                            Character c = Game.characters[menu_selection];
                            int maxPage = 0;
                            if (c.skills.Count <= 2) {
                                target_selection = -2;
                                target_selection_limit = -2;
                                T_1.text = "You have no skills to choose from. Learn a few from the Wizard!";
                                T_2.text = "";
                                T_3.text = "";
                                T_4.text = "";
                            } else if (c.skills.Count > 2) {
                                maxPage = (c.skills.Count - 3) / 4;
                                if (target_page < 0) {
                                    target_page = maxPage;
                                    target_selection = (c.skills.Count - 2) % 4 - 1;
                                } else if (target_page > maxPage) {
                                    target_page = 0;
                                    target_selection = 0;
                                }

                                if ((c.skills.Count - 2) >= (target_page * 4) + 1) {
                                    T_1.text = ">> " + c.skills[(target_page * 4) + 2].skillName;
                                    target_selection_limit = 0;
                                    if (pageUpdate == -1) {
                                        target_selection = target_selection_limit;
                                    }
                                } else {
                                    T_1.text = "";
                                }

                                if ((c.skills.Count - 2) >= (target_page * 4) + 2) {
                                    T_2.text = ">> " + c.skills[(target_page * 4) + 3].skillName;
                                    target_selection_limit = 1;
                                    if (pageUpdate == -1) {
                                        target_selection = target_selection_limit;
                                    }
                                } else {
                                    T_2.text = "";
                                }

                                if ((c.skills.Count - 2) >= (target_page * 4) + 3) {
                                    T_3.text = ">> " + c.skills[(target_page * 4) + 4].skillName;
                                    target_selection_limit = 2;
                                    if (pageUpdate == -1) {
                                        target_selection = target_selection_limit;
                                    }
                                } else {
                                    T_3.text = "";
                                }

                                if ((c.skills.Count - 2) >= (target_page * 4) + 4) {
                                    T_4.text = ">> " + c.skills[(target_page * 4) + 5].skillName;
                                    target_selection_limit = 3;
                                    if (pageUpdate == -1) {
                                        target_selection = target_selection_limit;
                                    }
                                } else {
                                    T_4.text = "";
                                }

                            }
                            if (confirm == 1 && target_selection >= 0) {
                                switch (action_selection) {
                                    case 0:
                                        c.skill1 = c.skills[target_page * 4 + target_selection + 2];
                                        emptyTrash();
                                        updateOptions = true;
                                        menu_phase = 2;
                                        break;
                                    case 1:
                                        c.skill2 = c.skills[target_page * 4 + target_selection + 2];
                                        emptyTrash();
                                        updateOptions = true;
                                        menu_phase = 2;
                                        break;
                                }
                            }
                            if (confirm == -1) {
                                emptyTrash();
                                updateOptions = true;
                                menu_phase = 2;
                            }

                            if (updateOptions && menu_phase == 4) {
                                emptyTrash();

                                if (c.skills.Count > (target_page * 4) + target_selection + 2 && target_selection >= 0) {
                                    if (c.skills[(target_page * 4) + target_selection + 2] != null) {
                                        GameObject InfoBoxClone = Instantiate(InfoBox, new Vector3(TargetBox.transform.position.x, TargetBox.transform.position.y), TargetBox.transform.rotation);
                                        InfoBoxClone.transform.SetParent(InfoBox.transform);
                                        InfoBoxClone.name = "TEMP InfoBox" + target_selection;
                                        InfoBoxClone.GetComponent<TextManager>().text = TextManager.convertSkillDescription(c, c.skills[target_page * 4 + target_selection + 2].skillName);
                                        trashCan.Add(InfoBoxClone);
                                        var targetPos = TargetBox.transform.position;
                                        targetPos.y += 260;
                                        StartCoroutine(Slide(InfoBoxClone, targetPos, false));
                                        updateOptions = false;
                                    }
                                }
                            }
                        }
                        break;
                        
                }
            } else if (menu_phase == 20) {
                int confirm = getConfirmation();
                if (confirm != 0) {
                    switch (shrine) {
                        case "Battle x2":
                            Battle.StartBattle(Game.getEnemyCount() + 2, 2, 2);
                            break;
                        case "Battle x3":
                            Battle.StartBattle(Game.getEnemyCount() + 2, 3, 3);
                            break;
                    }
                    shrine = null;
                    Game.map[Game.row, Game.col].wishes++;
                    closeMenu();
                    emptyTrash();
                    Game.gameMovementFreeze = false;
                }
            } else if (menu_phase == 21) {
                int confirm = getConfirmation();
                if (confirm != 0) {
                    closeMenu();
                    emptyTrash();
                    Game.gameMovementFreeze = false;
                }
            }
        }
    }

    public void emptyTrash() {
        while (trashCan.Any()) {
            GameObject t = trashCan[0];
            trashCan.RemoveAt(0);
            Destroy(t);
        }
    }
    
    public IEnumerator Slide(GameObject item, Vector3 targetPos)
    {
        slideCooldown = true;
        while (item != null && (targetPos - item.transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        if (item != null) {
            item.transform.position = targetPos;
        }

        if (menu_phase == 1) {
            menu_phase = 2;
        } else if (menu_phase == 3) {
            menu_phase = 4;
        }
        slideCooldown = false;
    }

    public IEnumerator Slide(GameObject item, Vector3 targetPos, bool isMenu)
    {
        slideCooldown = true;
        while (item != null && (targetPos - item.transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, targetPos, fastMoveSpeed * Time.deltaTime);
            yield return null;
        }

        if (item != null) {
            item.transform.position = targetPos;
        }
        slideCooldown = false;
    }

    public int getShrineCost() {
        Room r = Game.map[Game.row, Game.col];
        if (r.wishes <= 0){
            return 0;
        } 
        int cost = (int)(4 * Game.floorNumber * Math.Pow(2, r.wishes));
        return cost;
    }
    
}
