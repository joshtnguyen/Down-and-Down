using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{

    public static Vector3 lastPos;

    public float moveSpeed;

    private bool isMoving;

    private Vector2 input;

    private Animator animator;

    public LayerMask solidObjectsLayer;
    public LayerMask teleporterLayer;
    public LayerMask wizardLayer;
    public LayerMask breakLayer;
    public LayerMask northLayer;
    public LayerMask eastLayer;
    public LayerMask southLayer;
    public LayerMask westLayer;

    public GameObject UI;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    void Start() {
        if (lastPos != null) {
            transform.position = lastPos;
        }
    }
    
    private void Update()
    {
        if (Game.gameEvent == "Roaming" && !isMoving && !Game.gameMovementFreeze)
        {

            int confirm = getConfirmation();

            if (confirm == 1) {
                if (Game.map[Game.row, Game.col].roomType == "Exit" && Game.map[Game.row, Game.col].enemiesLeft <= 0 && IsOnTeleporter()) {
                    UI.GetComponent<GameUIManager>().openMenu("Teleporter");
                }

                if (Game.map[Game.row, Game.col].roomType == "Wizard Room" && IsOnWizard()) {
                    UI.GetComponent<GameUIManager>().openMenu("Wizard");
                }

                if (Game.map[Game.row, Game.col].roomType == "Break Room" && IsOnBreak()) {
                    UI.GetComponent<GameUIManager>().openMenu("Break");
                }

            }


            if (Game.gameEvent == "Roaming" && !isMoving && !Game.gameMovementFreeze) {
                lastPos = transform.position;

                input.x = Input.GetAxisRaw("Horizontal");
                input.y = Input.GetAxisRaw("Vertical");

                if (input.x != 0) {
                    input.y = 0;
                }

                if (input != Vector2.zero)
                {
                    animator.SetFloat("moveX", input.x);
                    animator.SetFloat("moveY", input.y);
                    var targetPos = transform.position;
                    targetPos.x += input.x;
                    targetPos.y += input.y;

                    
                    if (IsWalkable(targetPos)) {
                        Game.steps++;
                        StartCoroutine(Move(targetPos));
                    } else {
                        var pos = transform.position;
                        if (goingNorth(targetPos)) {
                            pos.y = -9;
                            transform.position = pos;
                            Game.row--;
                            Game.updateEnemies = true;
                        }
                        if (goingSouth(targetPos)) {
                            pos.y = 10;
                            transform.position = pos;
                            Game.row++;
                            Game.updateEnemies = true;
                        }
                        if (goingEast(targetPos)) {
                            pos.x = -11;
                            transform.position = pos;
                            Game.col++;
                            Game.updateEnemies = true;
                        }
                        if (goingWest(targetPos)) {
                            pos.x = 11;
                            transform.position = pos;
                            Game.col--;
                            Game.updateEnemies = true;
                        }
                    }
                }
            }
        }

        animator.SetBool("isMoving", isMoving);

    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos) {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null) {
            return false;
        }
        return true;
    }
    private bool IsOnTeleporter() {
        if (Physics2D.OverlapCircle(transform.position, 0.1f, teleporterLayer) != null) {
            return true;
        }
        return false;
    }

    private bool IsOnWizard() {
        if (Physics2D.OverlapCircle(transform.position, 1f, wizardLayer) != null) {
            return true;
        }
        return false;
    }

    private bool IsOnBreak() {
        if (Physics2D.OverlapCircle(transform.position, 1f, breakLayer) != null) {
            return true;
        }
        return false;
    }

    private bool goingNorth(Vector3 targetPos) {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, northLayer) != null) {
            return true;
        }
        return false;
    }

    private bool goingEast(Vector3 targetPos) {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, eastLayer) != null) {
            return true;
        }
        return false;
    }

    private bool goingSouth(Vector3 targetPos) {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, southLayer) != null) {
            return true;
        }
        return false;
    }

    private bool goingWest(Vector3 targetPos) {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, westLayer) != null) {
            return true;
        }
        return false;
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

}