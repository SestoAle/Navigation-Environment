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
        
        // Activate the desired goal area and disactivate all the others
        foreach (GameObject goalArea in GameManager.instance._goalAreas)
        {
            goalArea.SetActive(false);
        }
        GameManager.instance._goalAreas[(int)resetParameters["goal_area"] - 1].SetActive(true);
        
        // Reset the movable objects (platform, elevators, etc..) that must be reset after the end of the episode
        foreach (XYZMovement objects in GameManager.instance._movableObjectsToReset)
        {
            objects.ResetMovement();
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
            float agenty = resetParameters["agent_spawn_y"];
            GameManager.instance.spawnAtPosition(agent, agentX, agentz, agenty);

            // Spawn the agent randomly
            //GameManager.instance.spawnAtRandom(agent, 3f, GameManager.instance._range_target);
        }
            
    }

}