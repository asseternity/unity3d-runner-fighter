using System.Collections;
using System.Collections.Generic;
using BattleMovesSystem;
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

    // moves available to this enemy
    BattleMoves basicAttack = new BattleMoves("Basic attack", 1, 10, 1);
    public List<BattleMoves> moves = new List<BattleMoves>();

    // stats
    public int health = 10;
    public int movement = 3;
    public int action = 1;

    // starting stats
    public int startingMovement = 3;
    public int startingActions = 1;
    public int startingHealth = 10;

    // ui
    public GameObject healthData;

    // Start is called before the first frame update
    void Start()
    {
        // populate moves
        moves.Add(basicAttack);

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

    // method to move
    public void Move(Vector2 desiredGridPosition)
    {
        Debug.Log("AI movement sequence commenced!");
        Vector3Int desiredGridPositionConverted = new Vector3Int(
            (int)desiredGridPosition.x,
            (int)desiredGridPosition.y,
            0
        );
        Vector3 worldPos = grid.CellToWorld(desiredGridPositionConverted) + grid.cellSize / 2f;
        transform.position = worldPos;
        currentGridPosition = desiredGridPositionConverted;
        movement--;
        Debug.Log("Movement left: " + movement.ToString());
    }

    public IEnumerator MoveWithDelay(Vector2 nextDesiredMove, float delay)
    {
        Move(nextDesiredMove); // Your existing Move method
        yield return new WaitForSeconds(delay); // Adjust delay to control speed
    }

    // method to act
    public void UseAction(GameObject playerObject, BattleMoves move)
    {
        Debug.Log("AI action sequence commenced!");
        BattlePlayerController targetScript = playerObject.GetComponent<BattlePlayerController>();
        System.Random rnd = new System.Random();
        int attackRoll = rnd.Next(1, 20) + move.accuracy;
        if (attackRoll > 10)
        {
            targetScript.playerHealth = targetScript.playerHealth - move.damage;
        }
        action--;
        Debug.Log("Actions left: " + movement.ToString());
    }
}
