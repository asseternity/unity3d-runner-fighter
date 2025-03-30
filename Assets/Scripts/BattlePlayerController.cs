using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleMapSystem;
using BattleMovesSystem;
using Unity.VisualScripting;
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
    public GameObject healthData;
    public GameObject targetData;
    public GameObject actionsData;
    public GameObject actionButtonPrefab;
    public RectTransform actionsPanel;

    // turn data trackers
    private bool playersTurn = true;
    private int playerMovementLeft = 6;
    private int playerActionsLeft = 1;

    // player stats
    private int playerHealth = 20;

    // creation of moves
    BattleMoves basicAttack = new BattleMoves("Basic attack", 1, 10, 1);
    List<BattleMoves> playersMoves = new List<BattleMoves>();

    // targeting
    private GameObject target;

    // identifying obstacles
    List<Vector2> currentPositionsOfCreatures = new List<Vector2>();
    public List<GameObject> creaturesOnMap = new List<GameObject>();
    BattleObstacleTracker tracker = new BattleObstacleTracker();

    // to do here:
    // [v] 1) create a battle system ui:
    // that shows whose turn it is
    // that lets you end your turn
    // [v] 2) make movement only possible when it's your turn
    // [v] 3) make movement a limited resource that drains as you move and resets on next turn
    // [v] 4) add movement as a bar to ui
    // [v] 5) create a class for a 'move' where moves where all moves will be stored
    // [v] 6) create the first move: attack
    // [v] 7) add playerStats: health and list of available moves
    // [v] 8) put health on the ui
    // [v] 9) put an autopopulated list of available moved on the ui
    // [v] 10) add a resource - action
    // [v] 11) add that to the ui
    // [v] 12) create an enemy, put it under the 'creature' tag
    // [v] 13) create a script for enemies
    // [v] 14) in the enemies' script, attach them to the grid
    // [v] 15) and put the grid info on a public var (for calculating position)
    // [v] 16) give them a public var of health
    // [v] 17) delete them if health goes <= 0
    // [v] 18) display health on a thing attached over their heads
    // [v] 19) finish attacking
    // [v] 20) action economy
    // [v] 21) fill enemies with available moves
    // [v] 22) check if there is an enemy on the cell and block movement if there is:
    // [v] make a List of coordinates that are taken
    // [v] add every enemy to that list
    // [v] before the move, iterate over the list to see if that block is not in the list
    // [_] 23) make an List of enemies, a method for doing their turns, and iterate through the List with the method for ai
    // [_] 24) little animations for:
    // [_] attacks
    // [_] turn switches
    // [_] hits and misses
    // [_] fight log

    void Start()
    {
        // populate playersMoves and ui
        playersMoves.Add(basicAttack);
        PopulatePlayerAttacks();

        // Convert the grid coordinate to a world position.
        // Adding half a cell offset centers the player in the cell.
        Vector3 worldPos = grid.CellToWorld(startingGridPosition) + grid.cellSize / 2f;
        transform.position = worldPos;
        currentGridPosition = startingGridPosition;
    }

    void Update()
    {
        SelectTarget();
        currentPositionsOfCreatures = tracker.populateCoordsList(creaturesOnMap);

        if (playersTurn && playerMovementLeft > 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Vector3Int desiredGridPosition = new Vector3Int(
                    currentGridPosition.x - 1,
                    currentGridPosition.y,
                    currentGridPosition.z
                );
                Vector2 desiredGridPositionTester = new Vector2(
                    desiredGridPosition.x,
                    desiredGridPosition.y
                );
                if (tracker.isCellEmpty(desiredGridPositionTester, currentPositionsOfCreatures))
                {
                    Vector3 worldPos = grid.CellToWorld(desiredGridPosition) + grid.cellSize / 2f;
                    transform.position = worldPos;
                    currentGridPosition = desiredGridPosition;
                    playerMovementLeft--;
                }
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Vector3Int desiredGridPosition = new Vector3Int(
                    currentGridPosition.x + 1,
                    currentGridPosition.y,
                    currentGridPosition.z
                );
                Vector2 desiredGridPositionTester = new Vector2(
                    desiredGridPosition.x,
                    desiredGridPosition.y
                );
                if (tracker.isCellEmpty(desiredGridPositionTester, currentPositionsOfCreatures))
                {
                    Vector3 worldPos = grid.CellToWorld(desiredGridPosition) + grid.cellSize / 2f;
                    transform.position = worldPos;
                    currentGridPosition = desiredGridPosition;
                    playerMovementLeft--;
                }
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                Vector3Int desiredGridPosition = new Vector3Int(
                    currentGridPosition.x,
                    currentGridPosition.y + 1,
                    currentGridPosition.z
                );
                Vector2 desiredGridPositionTester = new Vector2(
                    desiredGridPosition.x,
                    desiredGridPosition.y
                );
                if (tracker.isCellEmpty(desiredGridPositionTester, currentPositionsOfCreatures))
                {
                    Vector3 worldPos = grid.CellToWorld(desiredGridPosition) + grid.cellSize / 2f;
                    transform.position = worldPos;
                    currentGridPosition = desiredGridPosition;
                    playerMovementLeft--;
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Vector3Int desiredGridPosition = new Vector3Int(
                    currentGridPosition.x,
                    currentGridPosition.y - 1,
                    currentGridPosition.z
                );
                Vector2 desiredGridPositionTester = new Vector2(
                    desiredGridPosition.x,
                    desiredGridPosition.y
                );
                if (tracker.isCellEmpty(desiredGridPositionTester, currentPositionsOfCreatures))
                {
                    Vector3 worldPos = grid.CellToWorld(desiredGridPosition) + grid.cellSize / 2f;
                    transform.position = worldPos;
                    currentGridPosition = desiredGridPosition;
                    playerMovementLeft--;
                }
            }
        }
        // now update ui
        Text movementDataText = movementData.GetComponent<Text>();
        Text actionsDataText = actionsData.GetComponent<Text>();
        Text healthDataText = healthData.GetComponent<Text>();
        movementDataText.text = playerMovementLeft.ToString();
        actionsDataText.text = playerActionsLeft.ToString();
        healthDataText.text = playerHealth.ToString();
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
        // do the turn
        await Task.Delay(2000);

        playersTurn = true;
        playerMovementLeft = 6;
        playerActionsLeft = 1;

        // update ui
        Text turnDataText = turnData.GetComponent<Text>();
        turnDataText.text = "player";
    }

    // method for populating the player's attacks list
    // with buttons for each attacks available to the player
    // create a list of BattleMoves available to the player
    // and a thing loops and creates sub-buttons for a pop-up menu
    // and attaches the "do move" method to the buttons with the argument of the move
    void PopulatePlayerAttacks()
    {
        for (int i = 0; i < playersMoves.Count; i++)
        {
            GameObject btn = Instantiate(actionButtonPrefab, actionsPanel);
            var btnComp = btn.GetComponent<Button>();
            int moveIndex = i;
            btnComp.onClick.AddListener(() => UseMove(playersMoves[moveIndex]));
        }
    }

    // method for choosing the target
    // when the mouse clicks:
    // check if it has the tag 'creature'
    // if it does, put that gameobject into a var
    void SelectTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.collider.gameObject;
                if (clickedObject.CompareTag("creature"))
                {
                    target = clickedObject;
                    Text targetDataText = targetData.GetComponent<Text>();
                    targetDataText.text = target.name;
                }
            }
        }
    }

    // method for parsing the move
    // then it checks rage --- are you within the range?
    // rolls for attack, adds accuracy
    // if hits, minuses the hp from the target
    void UseMove(BattleMoves move)
    {
        if (playerActionsLeft > 0)
        {
            // grab the target's grid position
            BattleEnemy targetScript = target.GetComponent<BattleEnemy>();
            int targetX = targetScript.currentGridPosition.x;
            int targetY = targetScript.currentGridPosition.y;

            // grab the player's grid position
            int playerX = currentGridPosition.x;
            int playerY = currentGridPosition.y;

            // calculate how many squares are in between us
            int distanceX = Math.Abs(targetX - playerX);
            int distanceY = Math.Abs(targetY - playerY);
            int totalDistance = Math.Max(distanceX, distanceY);

            if (move.range >= totalDistance)
            {
                System.Random rnd = new System.Random();
                int attackRoll = rnd.Next(1, 20) + move.accuracy;
                if (attackRoll > 10)
                {
                    targetScript.health = targetScript.health - move.damage;
                }
            }

            playerActionsLeft--;
        }
    }
}
