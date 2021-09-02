using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using UnityEngine.SocialPlatforms;

public class BugAcademy : Academy
{
    private void Start() 
    {
        AcademyReset();
    }

    public override void AcademyReset()
    {
        // Destroy pulled objects
        GameManager.instance.destroyPoolObjects();

        // Reset keys and doors (if any)
        foreach(Key k in GameManager.instance._keys)
        {
            k.gameObject.SetActive(true);
            k._isPickedUp = false;
        }
        foreach(Door d in GameManager.instance._doorOpened)
        {
            d.gameObject.SetActive(false);
        }
        foreach(Door d in GameManager.instance._doorClosed)
        {
            d.gameObject.SetActive(true);
        }

        // Spawn agents
        foreach (GameObject agent in GameManager.instance._agents)
        {
            // Spawn the occupancy map
            if (agent.GetComponentInChildren<ThreeDGrid>() != null)
            {
                if(!agent.GetComponentInChildren<ThreeDGrid>().created)
                {
                    agent.GetComponentInChildren<ThreeDGrid>().createLocalGrid();
                }
            }

            // Spawn agent at specific position
            float agentX = resetParameters["agent_spawn_x"];
            float agentz = resetParameters["agent_spawn_z"];
            GameManager.instance.spawnAtPosition(agent, agentX, agentz, 1);

            // Spawn the agent randomly
            //GameManager.instance.spawnAtRandom(agent, 3f, GameManager.instance._range_target);
        }
            
    }

}