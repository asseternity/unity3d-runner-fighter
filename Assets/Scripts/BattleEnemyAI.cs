using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleMapSystem;
using BattleMovesSystem;
using UnityEngine;

namespace EnemyAI
{
    public class BattleEnemyAI
    {
        // this will hold methods that the ai will use to make its turn
        public bool canIAttack(Vector2 playerPosition, Vector2 myPosition, GameObject enemyObject)
        {
            // can I attack method - iterates through available moves, sees if there's a player within that distance
            bool result = false;
            float distanceX = Math.Abs(playerPosition.x - myPosition.x);
            float distanceY = Math.Abs(playerPosition.y - myPosition.y);
            float totalDistance = Math.Max(distanceX, distanceY);
            BattleEnemy script = enemyObject.GetComponent<BattleEnemy>();
            for (int i = 0; i < script.moves.Count; i++)
            {
                if (script.moves[i].range >= totalDistance)
                {
                    result = true;
                }
            }
            return result;
        }

        public BattleMoves whichAttackToUse(
            Vector2 playerPosition,
            Vector2 myPosition,
            GameObject enemyObject
        )
        {
            List<BattleMoves> availableMoves = new List<BattleMoves>();
            float distanceX = Math.Abs(playerPosition.x - myPosition.x);
            float distanceY = Math.Abs(playerPosition.y - myPosition.y);
            float totalDistance = Math.Max(distanceX, distanceY);
            BattleEnemy script = enemyObject.GetComponent<BattleEnemy>();
            for (int i = 0; i < script.moves.Count; i++)
            {
                if (script.moves[i].range >= totalDistance)
                {
                    availableMoves.Add(script.moves[i]);
                }
            }
            var sortedMoves = availableMoves
                .OrderByDescending(m => m.damage)
                .ThenByDescending(m => m.accuracy)
                .ToList();
            return sortedMoves[0];
        }

        public Vector2 whichDirectionIsPlayer(
            Vector2 playerPosition,
            Vector2 myPosition,
            List<Vector2> currentPositionOfCreatures
        )
        {
            // set up a list of priorities
            List<string> priorities = new List<string>();
            BattleObstacleTracker tracker = new BattleObstacleTracker();
            // calculates distance to player
            float differenceX = playerPosition.x - myPosition.x;
            float differenceY = playerPosition.y - myPosition.y;
            // grabs a random of two directions if both are not right
            if (differenceX != 0 && differenceY != 0)
            {
                int roll = UnityEngine.Random.Range(1, 3);
                if (roll == 1)
                {
                    if (differenceY > 0)
                    {
                        priorities.Add("up");
                    }
                    else
                    {
                        priorities.Add("down");
                    }
                    if (differenceX > 0)
                    {
                        priorities.Add("right");
                    }
                    else
                    {
                        priorities.Add("left");
                    }
                }
                else
                {
                    if (differenceX > 0)
                    {
                        priorities.Add("right");
                    }
                    else
                    {
                        priorities.Add("left");
                    }
                    if (differenceY > 0)
                    {
                        priorities.Add("up");
                    }
                    else
                    {
                        priorities.Add("down");
                    }
                }
            }
            // grabs one direction if one is not right
            else if (differenceX == 0 && differenceY != 0)
            {
                if (differenceY > 0)
                {
                    priorities.Add("up");
                }
                else
                {
                    priorities.Add("down");
                }
            }
            else if (differenceX != 0 && differenceY == 0)
            {
                if (differenceX > 0)
                {
                    priorities.Add("right");
                }
                else
                {
                    priorities.Add("left");
                }
            }
            // returns original cell if everything's right
            else
            {
                return myPosition;
            }
            // then fills the rest of the priorities with directions yet untested, in an orderly fashion
            while (priorities.Count < 4)
            {
                if (!priorities.Contains("down"))
                {
                    priorities.Add("down");
                }
                if (!priorities.Contains("right"))
                {
                    priorities.Add("right");
                }
                if (!priorities.Contains("left"))
                {
                    priorities.Add("left");
                }
                if (!priorities.Contains("up"))
                {
                    priorities.Add("up");
                }
            }
            // then go for a prioritized exiting loop of isCellEmpty
            for (int i = 0; i < priorities.Count; i++)
            {
                float xChecking = 0;
                float yChecking = 0;
                if (priorities[i] == "down")
                {
                    xChecking = myPosition.x;
                    yChecking = myPosition.y - 1;
                }
                else if (priorities[i] == "right")
                {
                    xChecking = myPosition.x + 1;
                    yChecking = myPosition.y;
                }
                else if (priorities[i] == "left")
                {
                    xChecking = myPosition.x - 1;
                    yChecking = myPosition.y;
                }
                else if (priorities[i] == "up")
                {
                    xChecking = myPosition.x;
                    yChecking = myPosition.y + 1;
                }
                bool cellEmpty = tracker.isCellEmpty(
                    new Vector2(xChecking, yChecking),
                    currentPositionOfCreatures
                );
                if (cellEmpty)
                {
                    return new Vector2(xChecking, yChecking);
                }
            }
            return new Vector2();
        }

        public void AIPriorities(
            GameObject enemyObject,
            GameObject playerObject,
            List<Vector2> currentPositionOfCreatures
        )
        {
            Debug.Log("AI Priorities commenced!");
            // AI method
            // grab turn resources
            BattleEnemy script = enemyObject.GetComponent<BattleEnemy>();
            // first, see if you can attack
            BattlePlayerController p = playerObject.GetComponent<BattlePlayerController>();
            float playerX = p.currentGridPosition.x;
            float playerY = p.currentGridPosition.y;
            Vector2 playerPosition = new Vector2(playerX, playerY);
            float myX = script.currentGridPosition.x;
            float myY = script.currentGridPosition.y;
            Vector2 myPosition = new Vector2(myX, myY);
            bool attackPossible = canIAttack(playerPosition, myPosition, enemyObject);
            if (attackPossible)
            {
                Debug.Log("I can attack!");
                // if you can, do and end your turn
                BattleMoves moveToUse = whichAttackToUse(playerPosition, myPosition, enemyObject);
                script.UseAction(playerObject, moveToUse);
                p.EndEnemyTurn();
                return;
            }
            else
            {
                Debug.Log("I cannot attack.");
                int timesLopped = 0;
                while (true)
                {
                    if (timesLopped > 20)
                    {
                        p.EndEnemyTurn();
                        return;
                    }
                    // if you can't, we enter a cycle:
                    // step 1 of the cycle - check which direction to move, then move to that cell
                    Vector2 nextDesiredMove = whichDirectionIsPlayer(
                        playerPosition,
                        myPosition,
                        currentPositionOfCreatures
                    );
                    script.StartCoroutine(script.MoveWithDelay(nextDesiredMove, 1f));
                    Debug.Log("I moved to" + nextDesiredMove.ToString() + ". Can I attack now?");
                    // grab updated enemy position
                    myPosition = new Vector2(
                        script.currentGridPosition.x,
                        script.currentGridPosition.y
                    );
                    // step 2 of the cycle - check if you can attack. if can - do and end your turn, if can't, proceed
                    bool attackPossible2 = canIAttack(playerPosition, myPosition, enemyObject);
                    if (attackPossible2)
                    {
                        Debug.Log("Now I can attack.");
                        // if you can, do and end your turn
                        BattleMoves moveToUse = whichAttackToUse(
                            playerPosition,
                            myPosition,
                            enemyObject
                        );
                        script.UseAction(playerObject, moveToUse);
                        p.EndEnemyTurn();
                        return;
                    }
                    Debug.Log("I still cannot attack");
                    // step 3 of the cycle - if you're out of moves but cannot attack, end your turn
                    if (script.movement == 0 && !attackPossible2)
                    {
                        p.EndEnemyTurn();
                        return;
                    }
                    // step 4 of the cycle - if you're out of moves and action, end your turn
                    if (script.movement == 0 && script.action == 0)
                    {
                        p.EndEnemyTurn();
                        return;
                    }
                    timesLopped++;
                }
            }
        }
    }
}
