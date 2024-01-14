using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{

    public static float moveSpeed = 2000;

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

    public static string menu_name;
    public static string menu_type;
    public static int menu_phase;
    public static int menu_selection;
    public static int menu_selection_limit;

    public static int action_selection;
    public static int action_selection_limit;

    public bool slideCooldown;
    public bool animationCooldown;

    public void openMenu(string menuType) {
        menu_name = menuType;
        switch(menuType) {
            case "Teleporter":
                OptionBox.SetActive(true);
                ActionBox.transform.position = DescriptionBox.transform.position;
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
                Game.gameMovementFreeze = true;
                break;
            case "Wizard":
                OptionBox.SetActive(true);
                ActionBox.transform.position = DescriptionBox.transform.position;
                D_1.text = "Welcome to my humble shop. I can teach you some skills for your future battles. For a price that is...";
                D_2.text = "";
                D_3.text = "";
                D_4.text = "";
                A_1.text = ">> ";
                A_2.text = ">> ";
                A_3.text = ">> ";
                A_4.text = ">> ";
                menu_type = "Highlight";
                menu_phase = 1;
                menu_selection = -1;
                menu_selection_limit = -1;
                action_selection = 0;
                action_selection_limit = 3;
                Game.gameMovementFreeze = true;
                break;
            case "Break":
                OptionBox.SetActive(true);
                ActionBox.transform.position = DescriptionBox.transform.position;
                D_1.text = ">> Heal an Ally";
                D_2.text = ">> Change Skills";
                D_3.text = "";
                D_4.text = "";
                A_1.text = ">> ";
                A_2.text = ">> ";
                A_3.text = ">> ";
                A_4.text = ">> ";
                menu_type = "Highlight";
                menu_phase = 0;
                menu_selection = 0;
                menu_selection_limit = 1;
                action_selection = 0;
                action_selection_limit = 3;
                Game.gameMovementFreeze = true;
                break;
        }
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
        menu_type = "";
        menu_phase = -1;
        menu_selection = -1;
        action_selection = -1;
        action_selection_limit = -1;
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

        D_1.fontStyle = FontStyle.Normal;
        D_2.fontStyle = FontStyle.Normal;
        D_3.fontStyle = FontStyle.Normal;
        D_4.fontStyle = FontStyle.Normal;
        A_1.fontStyle = FontStyle.Normal;
        A_2.fontStyle = FontStyle.Normal;
        A_3.fontStyle = FontStyle.Normal;
        A_4.fontStyle = FontStyle.Normal;

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
        }

        if (!slideCooldown && !animationCooldown) {
            if (menu_phase == 0) {
                ActionBox.transform.position = DescriptionBox.transform.position;
                updateSelection(ref menu_selection, menu_selection_limit);

                int confirm = getConfirmation();
                switch (menu_name) {
                    case "Break":
                        if (confirm == 1) {
                            if (action_selection == 0) {
                                
                            } else if (action_selection == 1) {
                                
                            }
                        }
                        if (confirm == -1) {
                            closeMenu();
                            Game.gameMovementFreeze = false;
                        }
                        break;
                }
            } else if (menu_phase == 1) {
                var targetPos = DescriptionBox.transform.position;
                ActionBox.transform.position = DescriptionBox.transform.position;
                targetPos.x += 517;
                StartCoroutine(Slide(ActionBox, targetPos));
            } else if (menu_phase == 2) {
                updateSelection(ref action_selection, action_selection_limit);

                int confirm = getConfirmation();
                switch (menu_name) {
                    case "Teleporter":
                        if (confirm == 1) {
                            if (action_selection == 0) {
                                SceneManager.LoadScene("Debrief Scene");
                            } else if (action_selection == 1) {
                                confirm = -1;
                            }
                        }
                        if (confirm == -1) {
                            closeMenu();
                            Game.gameMovementFreeze = false;
                        }
                        break;
                    case "Wizard":
                        if (confirm == 1) {
                            switch (action_selection == 0) {

                            }
                        }
                        if (confirm == -1) {
                            closeMenu();
                            Game.gameMovementFreeze = false;
                        }
                        break;
                    case "Break":
                        if (confirm == 1) {
                            switch (action_selection == 0) {
                                
                            }
                        }
                        if (confirm == -1) {
                            closeMenu();
                            Game.gameMovementFreeze = false;
                        }
                        break;
                }
            }
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

        menu_phase = 2;
        slideCooldown = false;
    }
}
