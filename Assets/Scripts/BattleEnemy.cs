using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemy : MonoBehaviour
{
    [Tooltip("Reference to the Grid GameObject in the scene")]
    public Grid grid;

    [Tooltip("The starting grid position (x,y) where the player starts")]
    public Vector3Int startingGridPosition = new Vector3Int(3, 5, 0);

    [Tooltip("The current grid position (x,y) where the player currently is")]
    public Vector3Int currentGridPosition;

    // stats
    public int health = 10;

    // ui
    public GameObject healthData;

    // Start is called before the first frame update
    void Start()
    {
        // Convert the grid coordinate to a world position.
        // Adding half a cell offset centers the player in the cell.
        Vector3 worldPos = grid.CellToWorld(startingGridPosition) + grid.cellSize / 2f;
        transform.position = worldPos;
        currentGridPosition = startingGridPosition;
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject, 1f);
        }
        else
        {
            TextMeshPro healthDataText = healthData.GetComponentInChildren<TextMeshPro>();
            healthDataText.text = health.ToString();
        }
    }
}
