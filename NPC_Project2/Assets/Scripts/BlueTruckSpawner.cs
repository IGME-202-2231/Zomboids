using System.Collections;
using UnityEngine;

public class BlueTruckSpawner : MonoBehaviour
{
    public GameObject truckPrefab;
    public float truckSpeed = 5.0f;
    public float spawnCooldown = 2.0f;

    private bool canSpawn = true;

    private AgentManager agentManager;

    private void Start()
    {
        // Find the AgentManager in the scene
        agentManager = FindObjectOfType<AgentManager>();

        // Start the initial cooldown
        StartCoroutine(SpawnCooldown());
    }

    void Update()
    {
        // Check for spacebar press and cooldown status
        if (Input.GetKeyDown(KeyCode.Space) && canSpawn)
        {
            SpawnTruck();
        }
    }

    void SpawnTruck()
    {
        // Set cooldown flag to prevent spawning during cooldown
        canSpawn = false;

        // Calculate the spawn position at the right side of the screen
        float spawnX = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
        float spawnY = Camera.main.ScreenToWorldPoint(new Vector3(0, (Screen.height / 2), 0)).y;

        // Instantiate the truck prefab at the calculated spawn position
        GameObject truckObject = Instantiate(truckPrefab, new Vector3(spawnX, spawnY, -2), Quaternion.identity);

        // Get the Obstacle component from the spawned truck
        Obstacle truckObstacle = truckObject.GetComponent<Obstacle>();

        // Add the truck to the obstacles list in AgentManager
        if (truckObstacle != null && agentManager != null)
        {
            agentManager.obstacles.Add(truckObstacle);
        }

        // Move the truck to the left side of the screen
        StartCoroutine(MoveTruck(truckObject));
    }

    IEnumerator MoveTruck(GameObject truckObject)
    {
        // Calculate the left boundary of the screen
        float rightBoundary = Camera.main.ScreenToWorldPoint(new Vector3(1500, 0, 0)).x;

        while (truckObject.transform.position.x < rightBoundary)
        {
            // Move the truck to the left
            truckObject.transform.Translate(Vector3.right * truckSpeed * Time.deltaTime);

            yield return null;
        }

        // Remove the truck from the obstacles list before destroying it
        Obstacle truckObstacle = truckObject.GetComponent<Obstacle>();
        if (truckObstacle != null && agentManager != null)
        {
            agentManager.obstacles.Remove(truckObstacle);
        }

        // Destroy the truck object after it goes off the right side
        Destroy(truckObject);

        // Set cooldown flag to allow spawning after cooldown period
        StartCoroutine(SpawnCooldown());
    }

    IEnumerator SpawnCooldown()
    {
        // Wait for the specified cooldown time
        yield return new WaitForSeconds(spawnCooldown);

        // Set cooldown flag to allow spawning
        canSpawn = true;
    }


}
