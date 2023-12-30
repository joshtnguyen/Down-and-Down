using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public float moveSpeed;

    private bool isMoving;

    private Animator animator;

    public LayerMask solidObjectsLayer;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
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
            
            if (!isWalkable(targetPosX)) {
                horizontalCorrection = false;
            }

            if (pos.y > playerPos.y) {
                targetPosY.y -= 1;
                verticalCorrection = true;
            } else if (pos.y < playerPos.y) {
                targetPosY.y += 1;
                verticalCorrection = true;
            }

            if (!isWalkable(targetPos)) {
                verticalCorrection = false;
            }

            if (verticalCorrection && horizontalCorrection) {
                if (Vector3.Distance(playerPos, targetPosX) > Vector3.Distance(playerPos, targetPosY)) {
                    targetPos.y = targetPosY.y;
                    animator.SetFloat("moveY", targetPosY.y - pos.y);
                } else {
                    targetPos.x = targetPosX.x;
                    animator.SetFloat("moveX", targetPosX.x - pos.x);
                }
                StartCoroutine(Move(targetPos));
            } else if (verticalCorrection) {
                targetPos.y = targetPosY.y;
                animator.SetFloat("moveY", targetPosY.y - pos.y);
                StartCoroutine(Move(targetPos));
            } else if (horizontalCorrection) {
                targetPos.x = targetPosX.x;
                animator.SetFloat("moveX", targetPosX.x - pos.x);
                StartCoroutine(Move(targetPos));
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

    private bool isWalkable(Vector3 targetPos) {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null) {
            return false;
        }
        return true;
    }
}

