using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckCollision : MonoBehaviour
{
    [SerializeField]
    Splatter splatter;

    //public GameObject bloodPrefab; // Reference to the blood prefab
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has an FSM script
        FSM fsm = collision.gameObject.GetComponent<FSM>();

        // Check if the collided object is a human or a zombie
        if (fsm != null && (fsm.currentState == States.Transformer || fsm.currentState == States.HumanWander || fsm.currentState == States.Zombie || fsm.currentState == States.HumanFlee || fsm.currentState == States.Corgi))
        {
            // Set the state to Blood
            fsm.SetState(States.Blood);
            splatter.PlaySplatSound();
            
        }
    }

}
