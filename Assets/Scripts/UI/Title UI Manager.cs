using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class TitleUIManager : MonoBehaviour
{

    public static bool hasOpened = false;
    public static bool firstRun = true;

    public static float moveSpeed = 2000;
    public static float fastMoveSpeed = 3000;

    public GameObject Tip;

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

    public static int action_selection;
    public static int action_selection_limit;

    public static int target_selection;
    public static int target_selection_limit;
    public static int target_page;

    public bool updateOptions;
    public bool slideCooldown;
    public bool animationCooldown;

    List<GameObject> trashCan = new List<GameObject>();

    public void openMenu(string menuType) {
        menu_name = menuType;
        switch(menuType) {
            case "Title":
                OptionBox.SetActive(true);
                ActionBox.transform.position = DescriptionBox.transform.position;
                D_1.text = "Welcome to Down and Down!";
                D_2.text = "Use the arrow keys to navigate!";
                D_3.text = "C to Confirm / X to Cancel";
                D_4.text = "Z to Toggle Map";
                A_1.text = ">> Start Game";
                A_2.text = ">> Change Settings";
                A_3.text = "";
                A_4.text = "";
                T_1.text = ">> Starting Gold";
                T_2.text = ">> Level Requirement";
                T_3.text = "Confirm to select and";
                T_4.text = "arrow keys to change!";
                menu_type = "Highlight";
                menu_phase = 1;
                menu_selection = 0;
                menu_selection_limit = 0;
                action_selection = 0;
                action_selection_limit = 1;
                target_selection = 0;
                target_selection_limit = 1;
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
        
        if (getConfirmation() == 1 && !hasOpened) {
            Tip.SetActive(false);
            hasOpened = true;
            if (firstRun) {
                firstRun = false;
                openMenu("Title");
            }
        }

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

        T_1.text = ">> Starting Gold: " + Game.gold;
        T_2.text = ">> Level Requirement: " + Character.L_UP_REQ;

        if (!slideCooldown && !animationCooldown) {
            if (menu_phase == 1) {
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
                switch (menu_name) {
                    case "Title":
                        if (confirm == 1) {
                            if (action_selection == 0) {
                                SceneManager.LoadScene("Overworld Scene");
                            } else if (action_selection == 1) {
                                menu_phase = 3;
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
                switch (menu_name) {
                    case "Title":
                        if (confirm == 1) {
                            switch (target_selection) {
                                case 0:
                                    menu_phase = 5;
                                    break;
                                case 1:
                                    menu_phase = 5;
                                    break;
                            }
                        }
                        if (confirm == -1) {
                            menu_phase = 2;
                        }
                        break;
                }
            } else if (menu_phase == 5) {
                switch (menu_name) {
                    case "Title":
                        int confirm = getConfirmation();
                        int h = GetHorizontal();
                        int v = GetVertical();
                        if (target_selection == 0) {
                            if (h > 0 || v < 0) {
                                Game.gold += 5;
                            } else if (h < 0 || v > 0) {
                                Game.gold -= 5;
                            }
                            if (Game.gold < 0) {
                                Game.gold = 0;
                            }
                        } else if (target_selection == 1) {
                            if (h > 0 || v < 0) {
                                Character.L_UP_REQ++;
                            } else if (h < 0 || v > 0) {
                                Character.L_UP_REQ--;
                            }
                            if (Character.L_UP_REQ < 1) {
                                Character.L_UP_REQ = 1;
                            }
                        }

                        if (confirm != 0) {
                            menu_phase = 4;
                        }
                        break;
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
    
}
