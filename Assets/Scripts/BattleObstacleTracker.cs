using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleMapSystem
{
    public class BattleObstacleTracker
    {
        public List<Vector2> populateCoordsList(List<GameObject> enemies)
        {
            List<Vector2> result = new List<Vector2>();
            for (int i = 0; i < enemies.Count; i++)
            {
                BattleEnemy script = enemies[i].GetComponent<BattleEnemy>();
                int x = script.currentGridPosition.x;
                int y = script.currentGridPosition.y;
                Vector2 position = new Vector2(x, y);
                result.Add(position);
            }
            return result;
        }

        public bool isCellEmpty(Vector2 cell, List<Vector2> enemyPositions)
        {
            bool cellEmpty = true;
            for (int i = 0; i < enemyPositions.Count; i++)
            {
                float x = enemyPositions[i].x;
                float y = enemyPositions[i].y;
                if (cell.x == x && cell.y == y)
                {
                    cellEmpty = false;
                }
            }
            return cellEmpty;
        }
    }
}
