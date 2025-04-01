using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public void StartMoving(GameObject enemyObject, Vector2 nextDesiredMove)
    {
        StartCoroutine(MoveWithDelay(enemyObject, nextDesiredMove));
    }

    private IEnumerator MoveWithDelay(GameObject enemyObject, Vector2 nextDesiredMove)
    {
        BattleEnemy script = enemyObject.GetComponent<BattleEnemy>();
        script.Move(nextDesiredMove);
        yield return new WaitForSeconds(0.1f);
    }
}
