using UnityEngine;

public class BattlePlayerController : MonoBehaviour
{
    [Tooltip("Reference to the Grid GameObject in the scene")]
    public Grid grid;

    [Tooltip("The starting grid position (x,y) where the player starts")]
    public Vector3Int startingGridPosition = new Vector3Int(0, 0, 0);

    [Tooltip("The current grid position (x,y) where the player currently is")]
    private Vector3Int currentGridPosition;

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
        }
    }
}
