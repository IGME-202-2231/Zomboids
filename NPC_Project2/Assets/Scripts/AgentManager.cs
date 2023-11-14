using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    [SerializeField]
    Wanderer wandererPrefab;

    List<Agent> agents;

    [SerializeField]
    uint numWanderers;

    public List<Agent> Agents { get { return agents; } }

    // Start is called before the first frame update
    void Start()
    {
        agents = new List<Agent>();
        for(int i = 0; i < numWanderers; i++)
        {
            SpawnWanderer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnWanderer()
    {
        Wanderer wanderer = Instantiate(wandererPrefab, transform);
        wanderer.AgentManager = this;
        agents.Add(wanderer);
    }
}
