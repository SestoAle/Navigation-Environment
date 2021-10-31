using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Object = System.Object;

public class GameManager : MonoBehaviour
{

    [HideInInspector] // Singleton
    public static GameManager instance;
    
    // Files for replaying trajectories
    public TextAsset _trajectoryFile;
    public List<TextAsset> _trajectoryFiles;
    public TextAsset _graphFile;
    private List<GameObject> _lines;
    public Material _correctLineMaterial;
    public Material _wrongLineMaterial;
    
    // Some object pools, they may be unused
    public ObjectPool _cubePool;
    public ObjectPool _coinPool;
    public ObjectPool _pointPool;
    public List<Obstacle> _obstacles = new List<Obstacle>();

    // Env parameters for curriculum learning
    private EnvironmentParameters _envParameters;

    // Range of spawn of the obstacles. For PCG elements.
    public float _range_obstacles = 500f;

    public List<GameObject> _agents;
    public List<GameObject> _goalAreas;
    public List<XYZMovement> _movableObjectsToReset;

    private static System.Random rng = new System.Random();
    
    // Keys and doors
    public List<Key> _keys;
    public List<Door> _doorClosed;
    public List<Door> _doorOpened;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        // Get the envParameters
        _envParameters = Academy.Instance.EnvironmentParameters;
        //Random.seed = 6;
    }

    int _frameCount = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        //// Press L to show the last saved trajectory
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    if (_lines != null)
        //    {
        //        foreach (GameObject l in _lines)
        //        {

        //            Destroy(l);
        //        }

        //        _lines = null;
        //    }
        //    else
        //    {
        //        // The saved trajectories are normalized between [0,1].
        //        // Unnormalize them to world coordinates.
        //        _lines = drawTrajectory(loadTrajectories(), -250, 250, -250, 250, 1, 60);
        //    }
            
        //    // foreach(TextAsset tr in _trajectoryFiles)
        //    // {
        //    //     _lines = drawTrajectory(loadTrajectories(tr), -250, 250, -250, 250, 1, 60);
        //    // }
        //}
        
        //// Press L to show the last saved trajectory
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneReset();
        //}
        
        //// For plotting the coverage graph. Not used right now.
        ////if (Input.GetKeyDown(KeyCode.C))
        //if (false)
        //{
        //    _pointPool.destroyAllObjects();
        //    drawGraph(loadGraph());
        //}
    }
    
    // Destroy all pooled objects
    public void destroyPoolObjects()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene ();

        _cubePool.destroyAllObjects();
        _coinPool.destroyAllObjects();
    }

    // Spawn numObstacles object from the pool at random position,
    // without overlapping themselves
    public void spawnObstacles(int numObstacles)
    {
        // Clear the obstacles list
        _obstacles.Clear();

        for (int i = 0; i < numObstacles; i++)
        {
            // Get a new pooled object
            GameObject cube = _cubePool.getPooledObject();
            
            // Reset is touched
            cube.GetComponent<Obstacle>().setIsTouched(false);
            
            // Add to the list of current obstacle
            _obstacles.Add(cube.GetComponent<Obstacle>());
            
            // Get a random free position
            Vector3 spawnPosition = spawnAtRandom(5f, _range_obstacles);
            spawnPosition.y = cube.transform.position.y;
            // Change the position
            cube.transform.position = spawnPosition;
        }
    }

    // Get a random free position
    public Vector3 spawnAtRandom(float radiusToCheck, float range)
    {

        int obstaclesMask = 1 << 9;
        int agentMask = 1 << 10;
        int targetMask = 1 << 11;
        
        // Get a random position
        float x = Random.Range(-range, range);
        float z = Random.Range(-range, range);
        
        Vector3 spawnPos = new Vector3(x, 0, z);
        
        // Check if in that position there is already a cube,
        // Otherwise, re-try
        if (
            Physics.CheckSphere (spawnPos, radiusToCheck, agentMask) ||
            thereIsAnObstacle(spawnPos, radiusToCheck)
            ) 
        {
            return spawnAtRandom(radiusToCheck, range);
        } 
        
        return new Vector3(x, 0, z);
        
    }
    
    // Spawn an object randomly given a range.
    public void spawnAtRandom(GameObject gameObject, float radiusToCheck,
        float range)
    {
        Vector3 newPosition = spawnAtRandom(radiusToCheck, range);
        gameObject.transform.position = new Vector3(newPosition.x, gameObject.transform.position.y, newPosition.z);
    }

    // Spawn GameObject at a specific position
    public void spawnAtPosition(GameObject gameObject, float x, float z)
    {
        Vector3 newPosition = new Vector3(x, gameObject.transform.position.y, z);
        gameObject.transform.position = new Vector3(newPosition.x, gameObject.transform.position.y, newPosition.z);
    }
    public void spawnAtPosition(GameObject gameObject, float x, float z, float y)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        gameObject.transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
    }

    // Check if there is a obstacle at a given position before next frame.
    // Used for spawning randomly agent and/or obstacles.
    public bool thereIsAnObstacle(Vector3 position, float radiusToCheck)
    {
        bool thereIs = false;

        foreach (var obs in _cubePool.pool)
        {
            if (!obs.activeSelf)
            {
                continue;
            }

            if (Vector3.Distance(obs.transform.position, position) < radiusToCheck)
            {
                thereIs = true;
                break;
            }
        }

        foreach (var obs in _agents)
        {
            if (!obs.activeSelf)
            {
                continue;
            }

            if (Vector3.Distance(obs.transform.position, position) < radiusToCheck)
            {
                thereIs = true;
                break;
            }
        }

        return thereIs;
    }

    // This is equivalent to AcademyReset when we had the Academy.
    // Must be called by the Agent on OnEpisodeBegin().
    public void SceneReset()
    {
        // Destroy pulled objects
        destroyPoolObjects();

        // Reset keys and doors (if any)
        foreach (Key k in GameManager.instance._keys)
        {
            k.gameObject.SetActive(true);
            k._isPickedUp = false;
        }
        foreach (Door d in GameManager.instance._doorOpened)
        {
            d.gameObject.SetActive(false);
        }
        foreach (Door d in GameManager.instance._doorClosed)
        {
            d.gameObject.SetActive(true);
        }

        // Activate the desired goal area and disactivate all the others
        foreach (GameObject goalArea in GameManager.instance._goalAreas)
        {
            goalArea.SetActive(false);
        }
        _goalAreas[(int)_envParameters.GetWithDefault("goal_area", 1.0f) - 1].SetActive(true);

        // Reset the movable objects (platform, elevators, etc..) that must be reset after the end of the episode
        foreach (XYZMovement objects in _movableObjectsToReset)
        {
            objects.ResetMovement();
        }

        // Spawn agents
        foreach (GameObject agent in GameManager.instance._agents)
        {
            // Spawn the occupancy map
            if (agent.GetComponentInChildren<ThreeDGrid>() != null)
            {
                if (!agent.GetComponentInChildren<ThreeDGrid>().created)
                {
                    agent.GetComponentInChildren<ThreeDGrid>().createLocalGrid();
                }
            }

            // Spawn agent at specific position
            _envParameters.GetWithDefault("agent_spawn_x", 0.0f);
            float agentX = _envParameters.GetWithDefault("agent_spawn_x", 0.0f);
            float agentz = _envParameters.GetWithDefault("agent_spawn_z", 0.0f);
            float agenty = _envParameters.GetWithDefault("agent_spawn_y", 1.7f);
            spawnAtPosition(agent, agentX, agentz, agenty);

            // Spawn the agent randomly
            //GameManager.instance.spawnAtRandom(agent, 3f, GameManager.instance._range_target);
        }

    }

    // Check if there is an agent at a specific position before the next frame.
    // Used for spawning randomly agent and/or obstacles.
    public bool thereIsAnAgent(Vector3 position, float radiusToCheck)
    {
        bool thereIs = false;

        foreach (var obs in _agents)
        {
            if (!obs.activeSelf)
            {
                continue;
            }

            if (Vector3.Distance(obs.transform.position, position) < radiusToCheck)
            {
                thereIs = true;
                break;
            }
        }

        return thereIs;
    }
    
    // Check if the agent is pressing a given object.
    public bool objectIsPressed(GameObject ob)
    {
        return thereIsAnAgent(ob.transform.position, 2f);
    }

    // Shuffle a list
    public static void Shuffle<T>(List<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    
    // Load trajectory points from json. This are normalized point between -1 and 1
    public Trajectory loadTrajectories()
    {
        Trajectory traj = JsonUtility.FromJson<Trajectory>(_trajectoryFile.text);

        return traj;
    }
    public Trajectory loadTrajectories(TextAsset tr)
    {
        Trajectory traj = JsonUtility.FromJson<Trajectory>(tr.text);

        return traj;
    }
    
    // Load coverage graph from json. They are already normalized in world coordinates
    public CoverageGraph loadGraph()
    {
        CoverageGraph graph = JsonUtility.FromJson<CoverageGraph>(_graphFile.text);

        return graph;
    }
    
    // Draw the last saved coverage graph.
    public List<GameObject> drawGraph(CoverageGraph graph)
    {
        int length = graph.x.Length;
        List<GameObject> points = new List<GameObject>();

        for (int i = 0; i < length; i++)
        {
            GameObject point = DrawPoint(graph.x[i], graph.z[i], graph.y[i]);
            points.Add(point);
        }

        return points;
    }

    public GameObject DrawPoint(float x, float z, float y)
    {
        GameObject point = _pointPool.getPooledObject();
        point.transform.position = new Vector3(x, y, z);

        return point;
    }
    
    
    // Draw the a given trajectory. The color of the lines will be based on the save IM values saved in the same
    // trajectory.
    public List<GameObject> drawTrajectory(Trajectory traj, float x_min, float x_max, float z_min,float z_max, float y_min, float y_max)
    {
        int length = traj.x_s.Length;
        List<GameObject> lines = new List<GameObject>();

        // Compute the mean if IM and IL
        float im_mean = 0;
        // foreach(float im_v in traj.im_values)
        // {
        //     im_mean += im_v;
        // }
        // im_mean /= traj.im_values.Length;
        int meanLength = 0;
        Vector3 activeGoalAreaPosition = new Vector3();
        foreach (GameObject goalArea in _goalAreas)
        {
            if (goalArea.activeInHierarchy)
            {
                activeGoalAreaPosition = goalArea.transform.position;
                break;
            }
        }
        for (int i = 0; i < traj.im_values.Length; i++)
        {
            Vector3 c_point = new Vector3(traj.x_s[i], traj.y_s[i], traj.z_s[i]);

            // de-normalize
            c_point.x = ((c_point.x + 1) / 2) * (x_max - x_min) + x_min;
            c_point.z = ((c_point.z + 1) / 2) * (z_max - z_min) + z_min;
            c_point.y = ((c_point.y + 1) / 2) * (y_max - y_min) + y_min;
            

            if (Vector3.Distance(activeGoalAreaPosition, c_point) < 1)
            {
                break;
            }

            im_mean += traj.im_values[i];
            meanLength++;

        }
        im_mean /= meanLength;

        // Compute the mean if IM and IL
        float il_mean = 0;
        foreach (float il_v in traj.il_values)
        {
            il_mean += il_v;
        }

        for (int i = 0; i < length - 1; i++)
        {
            Vector3 c_point = new Vector3(traj.x_s[i], traj.y_s[i], traj.z_s[i]);
            Vector3 n_point = new Vector3(traj.x_s[i+1], traj.y_s[i+1], traj.z_s[i+1]);
            
            // de-normalize
            c_point.x = ((c_point.x + 1) / 2) * (x_max - x_min) + x_min;
            n_point.x = ((n_point.x + 1) / 2) * (x_max - x_min) + x_min;
            
            c_point.z = ((c_point.z + 1) / 2) * (z_max - z_min) + z_min;
            n_point.z = ((n_point.z + 1) / 2) * (z_max - z_min) + z_min;
            
            c_point.y = ((c_point.y + 1) / 2) * (y_max - y_min) + y_min;
            n_point.y = ((n_point.y + 1) / 2) * (y_max - y_min) + y_min;
            
            Debug.DrawLine(c_point, n_point, Color.yellow);
            if(traj.im_values[i] > im_mean)
            {
                lines.Add(DrawLine(c_point, n_point, _wrongLineMaterial));
            }
            else
            {
                lines.Add(DrawLine(c_point, n_point, _correctLineMaterial));
            }
        }

        return lines;
    }
    
    GameObject DrawLine(Vector3 start, Vector3 end, Material material)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = material;
        lr.SetWidth(.5f, .5f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        return myLine;
    }
    
}
