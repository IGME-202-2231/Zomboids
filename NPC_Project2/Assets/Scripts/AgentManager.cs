using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    [SerializeField]
    //TagPlayer playerPrefab;
    Wanderer playerPrefab;

    List<Agent> agents;

    public Sprite[] tagSprites; //Sprite Array

    public List<Obstacle> obstacles;

    [SerializeField]
    private float countTimer;

    public float CountTimer { get { return countTimer; } }

    [SerializeField]
    uint playerCount;

    public List<Agent> Agents { get { return agents; } }

    public Agent itPlayer; //keep track of who is it

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

    }

    private void SpawnPlayer()
    {
        Wanderer newAgent = Instantiate(playerPrefab, transform);
        newAgent.AgentManager = this;
        agents.Add(newAgent);
        FlockManager.Instance.flock.Add(newAgent);
    }
}
