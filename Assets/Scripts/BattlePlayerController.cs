using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlayerController : MonoBehaviour
{
    [Tooltip("Reference to the Grid GameObject in the scene")]
    public Grid grid;

    [Tooltip("The starting grid position (x,y) where the player starts")]
    public Vector3Int startingGridPosition = new Vector3Int(0, 0, 0);

    [Tooltip("The current grid position (x,y) where the player currently is")]
    private Vector3Int currentGridPosition;

    // ui
    public GameObject turnData;
    public GameObject movementData;
    public GameObject endTurnButton;

    // turn data trackers
    private bool playersTurn = true;
    private int playerMovementLeft = 6;

    // to do here:
    // [v] 1) create a battle system ui:
    // that shows whose turn it is
    // that lets you end your turn
    // [v] 2) make movement only possible when it's your turn
    // [v] 3) make movement a limited resource that drains as you move and resets on next turn
    // [v] 4) add movement as a bar to ui

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
        if (playersTurn && playerMovementLeft > 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Vector3Int desiredGridPosition = new Vector3Int(
                    currentGridPosition.x - 1,
                    currentGridPosition.y,
                    currentGridPosition.z
                );
                Vector3 worldPos = grid.CellToWorld(desiredGridPosition) + grid.cellSize / 2f;
                transform.position = worldPos;
                currentGridPosition = desiredGridPosition;
                playerMovementLeft--;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Vector3Int desiredGridPosition = new Vector3Int(
                    currentGridPosition.x + 1,
                    currentGridPosition.y,
                    currentGridPosition.z
                );
                Vector3 worldPos = grid.CellToWorld(desiredGridPosition) + grid.cellSize / 2f;
                transform.position = worldPos;
                currentGridPosition = desiredGridPosition;
                playerMovementLeft--;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Vector3Int desiredGridPosition = new Vector3Int(
                    currentGridPosition.x,
                    currentGridPosition.y + 1,
                    currentGridPosition.z
                );
                Vector3 worldPos = grid.CellToWorld(desiredGridPosition) + grid.cellSize / 2f;
                transform.position = worldPos;
                currentGridPosition = desiredGridPosition;
                playerMovementLeft--;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Vector3Int desiredGridPosition = new Vector3Int(
                    currentGridPosition.x,
                    currentGridPosition.y - 1,
                    currentGridPosition.z
                );
                Vector3 worldPos = grid.CellToWorld(desiredGridPosition) + grid.cellSize / 2f;
                transform.position = worldPos;
                currentGridPosition = desiredGridPosition;
                playerMovementLeft--;
            }
        }
        // now update ui
        Text movementDataText = movementData.GetComponent<Text>();
        movementDataText.text = playerMovementLeft.ToString();
    }

    public void EndTurn()
    {
        playersTurn = false;

        // update ui
        Text turnDataText = turnData.GetComponent<Text>();
        turnDataText.text = "enemy";

        EnemyTurn();
    }

    async void EnemyTurn()
    {
        await Task.Delay(2000);
        playersTurn = true;
        playerMovementLeft = 6;

        // update ui
        Text turnDataText = turnData.GetComponent<Text>();
        turnDataText.text = "player";
    }
}
