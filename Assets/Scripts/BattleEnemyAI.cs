using System;
using System.Collections;
using System.Collections.Generic;
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
            // 1) can I attack method - iterates through available moves, sees if there's a player within that distance
            bool result = false;
            float distanceX = Math.Abs(playerPosition.x - myPosition.y);
            float distanceY = Math.Abs(playerPosition.x - myPosition.y);
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

        public void AIPriorities(GameObject enemyObject)
        {
            // AI method
            // cycle through above two priorities until either:
            // (1) no more action or movement or (2) action still there but can't attack player
        }
    }
}
