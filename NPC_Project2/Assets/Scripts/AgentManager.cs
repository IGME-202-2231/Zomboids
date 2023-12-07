using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    [SerializeField]
    FSM humanPrefab;

    [SerializeField]
    FSM zombiePrefab;

    List<Agent> agents;

    public Sprite[] tagSprites; //Sprite Array

    public List<Obstacle> obstacles;

    [SerializeField]
    private float countTimer;

    public float CountTimer { get { return countTimer; } }

    [SerializeField]
    uint playerCount;

    public List<Agent> Agents { get { return agents; } }

    public Agent zombie; //keep track of first zombie

    private bool zombieSpawned = false;

    public bool ZombieSpawned { get {  return zombieSpawned; } }

    // Start is called before the first frame update
    void Start()
    {
        agents = new List<Agent>();
        for (int i = 0; i < playerCount; i++)
        {
            SpawnPlayer();
        }
        /*
        if(agents.Count > 0)
        {
            ((TagPlayer)agents[0]).SetState(TagStates.Counting);
            itPlayer = agents[0];   //Assign who is it
        } */
    }

    // Update is called once per frame
    void Update()
    {
        SpawnZombie();
    }

    private void SpawnPlayer()
    {
        FSM newAgent = Instantiate(humanPrefab, transform);
        newAgent.AgentManager = this;
        agents.Add(newAgent);
        FlockManager.Instance.flock.Add(newAgent);
    }

    private void SpawnZombie()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        // Check for LMB press
        if (Input.GetMouseButtonDown(0))
        {
            zombieSpawned = true;
            FSM zombie = Instantiate(zombiePrefab, mousePos, Quaternion.identity);
            zombie.AgentManager = this;
            zombie.SetState(States.Zombie);
            agents.Add(zombie);
            FlockManager.Instance.flock.Add(zombie);
        }
    }
}
