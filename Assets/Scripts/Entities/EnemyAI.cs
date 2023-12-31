using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    
    public bool isAlive;

    public float moveSpeed;

    private bool isMoving;

    private Animator animator;

    public LayerMask solidObjectsLayer;

    public LayerMask playerObjectsLayer;

    public static void CreateEnemy(GameObject originalEnemy, int num) {
        for (int i = 0; i < num; i++) {
            GameObject EnemyClone = Instantiate(originalEnemy, new Vector3(Random.Range(1, 17) - 8, Random.Range(1, 17) - 8), originalEnemy.transform.rotation);
            EnemyClone.name = "EnemyClone" + (i + 1);
            EnemyAI obj = EnemyClone.GetComponent<EnemyAI>();
            obj.isAlive = true;
        }
    }

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isAlive) {
            Destroy(this.gameObject);
        } else if (Game.gameEvent == "Roaming" && !isMoving && !Game.gameMovementFreeze)
        {

            var playerPos = GameObject.Find("Player").transform.position;
            var pos = transform.position;
            var targetPos = transform.position;
            var targetPosX = transform.position;
            var targetPosY = transform.position;
            var horizontalCorrection = false;
            var verticalCorrection = false;

            if (pos.x > playerPos.x) {
                targetPosX.x -= 1;
                horizontalCorrection = true;
            } else if (pos.x < playerPos.x) {
                targetPosX.x += 1;
                horizontalCorrection = true;
            }
            
            if (!IsWalkable(targetPosX)) {
                horizontalCorrection = false;
            }

            if (pos.y > playerPos.y) {
                targetPosY.y -= 1;
                verticalCorrection = true;
            } else if (pos.y < playerPos.y) {
                targetPosY.y += 1;
                verticalCorrection = true;
            }

            if (!IsWalkable(targetPosY)) {
                verticalCorrection = false;
            }

            if (!PlayerInCollision(targetPos)) {
                if (verticalCorrection && horizontalCorrection) {
                    if (Vector3.Distance(playerPos, targetPosX) > Vector3.Distance(playerPos, targetPosY)) {
                        targetPos.y = targetPosY.y;
                        animator.SetFloat("moveX", 0);
                        animator.SetFloat("moveY", targetPosY.y - pos.y);
                    } else {
                        targetPos.x = targetPosX.x;
                        animator.SetFloat("moveX", targetPosX.x - pos.x);
                        animator.SetFloat("moveY", 0);
                    }
                    StartCoroutine(Move(targetPos));
                } else if (verticalCorrection) {
                    targetPos.y = targetPosY.y;
                    animator.SetFloat("moveX", 0);
                    animator.SetFloat("moveY", targetPosY.y - pos.y);
                    StartCoroutine(Move(targetPos));
                } else if (horizontalCorrection) {
                    targetPos.x = targetPosX.x;
                    animator.SetFloat("moveX", targetPosX.x - pos.x);
                    animator.SetFloat("moveY", 0);
                    StartCoroutine(Move(targetPos));
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

    private bool PlayerInCollision(Vector3 targetPos) {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, playerObjectsLayer) != null && Game.gameEvent == "Roaming") {
            Battle.StartBattle(this.gameObject);
            return true;
        }
        return false;
    }
}

