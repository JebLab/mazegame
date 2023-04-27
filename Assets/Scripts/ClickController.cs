using System;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.EventSystems.EventTrigger;

public class ClickController : MonoBehaviour
{

    public Camera enemyView;
    public NavMeshAgent agent;
    public Vector2 currentPos;



    // for random traversal
    bool randomTraverse = true;
    private System.Random random = new System.Random();
    private int length, width;
    private Tile ChosenTile;
    public Tile[,] grid { get; set; }
    private Vector3 newPos;
    float cooldown = 2.0f;

    // for chasing
    float forgetfullness = 0.0f;
    GameObject player;


    private GameObject gridContainer;
    // Update is called once per frame

    private void Start()
    {
        Debug.Log("Start: " + GetInstanceID());

        gridContainer = GameObject.Find("MazeGenerator");
        grid = gridContainer.GetComponent<GridGen>().getGrid();

        Mathf.Clamp(forgetfullness, 0.0f, 5.0f);
    }

    void Update()
    {
        // two options, depending on movement mode

        // player not in view, so randomly move
        if (!playerInLOS())
        {
            setNewDestination();
        }
    }

    public void setNewDestination()
    {
        cooldown -= Time.deltaTime;
        if (agent.velocity == Vector3.zero && cooldown <= 0)
        {
            cooldown = 2.0f;


            length = grid.GetLength(1);
            width = grid.GetLength(0);

            ChosenTile = grid[random.Next(0, length), random.Next(0, width)];

            Debug.Log("Chosen tile: " + ChosenTile.coord);
            Debug.Log("Getting a new destination: " + newPos);

            ChosenTile.setText("HERE");

            newPos = ChosenTile.floor.transform.position;

            agent.SetDestination(newPos);
        }
    }

    public bool playerInLOS()
    {
        //Debug.DrawRay(enemyView.transform.position, enemyView.transform.forward * 10, Color.red);

        // Define the number of rays and the angle between them
        int numRays = 5;
        float angleBetweenRays = 15.0f;

        // Cast a ray for each angle
        for (int i = 0; i < numRays; i++)
        {
            // Calculate the angle for this ray
            float angle = (i - (numRays / 2)) * angleBetweenRays;

            // Calculate the direction of the ray based on the angle
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            // Cast the ray;
            RaycastHit hitInfo;
            Debug.DrawRay(enemyView.transform.position, direction * 10, Color.red);
            Physics.Raycast(enemyView.transform.position, direction, out hitInfo, 10);

            if (hitInfo.transform != null && hitInfo.transform.name == "Player(Clone)")
            {
                // this is where the AI should begin chasing the player
                Debug.Log(hitInfo.transform.name);
                agent.SetDestination(hitInfo.transform.position);
                player = hitInfo.transform.gameObject;
                forgetfullness = 5.0f;
                return true;
            }
            else if (forgetfullness > 0)
            {
                forgetfullness -= Time.deltaTime;
                Debug.Log("Forgetting..." + forgetfullness);
                agent.SetDestination(player.transform.position);
            }
        }

        return false;
    }

}
