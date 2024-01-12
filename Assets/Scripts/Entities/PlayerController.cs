using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static Vector3 lastPos;

    public float moveSpeed;

    private bool isMoving;

    private Vector2 input;

    private Animator animator;

    public LayerMask solidObjectsLayer;
    public LayerMask northLayer;
    public LayerMask eastLayer;
    public LayerMask southLayer;
    public LayerMask westLayer;

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
                    StartCoroutine(Move(targetPos));
                    Game.getRoom();
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

}