using System;
using System.Collections;
using System.Collections.Generic;
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

        public Vector2 whichDirectionIsPlayer(Vector2 playerPosition, Vector2 myPosition)
        {
            // 2) which direction is the player? - calculates distance to player, returns which direction to take the next step
            // [_] later add pathfinding by iterating through all four directions and move only where you can
            return new Vector2();
        }

        public void AIPriorities(GameObject enemyObject)
        {
            // 3) finally an AI method through these two priorities until either
            // (1) no more action or movement or (2) action still there but can't attack player
        }
    }
}
