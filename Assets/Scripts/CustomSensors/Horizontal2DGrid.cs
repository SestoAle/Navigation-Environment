using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horizontal2DGrid : MonoBehaviour
{

    public int _gridWidth = 9;
    public GameObject _gridCube;
    public List<GameObject> gridCubes = new List<GameObject>();
    public float _gridScale = 1f;
    public float _gridHeight = 1f;
    public CubeCollision[,] gridMatrix;
    public GameObject agent;

    public bool created = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Create local grid
    public void createLocalGrid()
    {
        // Initialize gridMatrix
        gridMatrix = new CubeCollision[_gridWidth, _gridWidth];

        // Create gridPerception
        for (int i = -_gridWidth / 2; i < _gridWidth / 2 + 1; i++)
        {
            for (int j = -_gridWidth / 2; j < _gridWidth / 2 + 1; j++)
            {
                // Instantiate the cube and add it to the grid
                var newCube = Instantiate(_gridCube,
                    new Vector3(0, 0, 0),
                    Quaternion.identity);
                newCube.transform.parent = gameObject.transform;
                
                
                // Change the scale of the cube grid
                newCube.transform.localScale = new Vector3(_gridScale, _gridHeight, _gridScale);

                // Get the position in the grid
                // newCube.transform.position = new Vector3(i * _gridScale, _gridHeight/2 + 0.3f, j * _gridScale);
                // TODO: If 3D
                newCube.transform.position = new Vector3(i * _gridScale, 0, j * _gridScale);

                // Add to list
                gridCubes.Add(newCube);
            }
        }

        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0; j < _gridWidth; j++)
            {
                gridMatrix[i, j] = gridCubes[0].GetComponent<CubeCollision>();
                gridCubes.RemoveAt(0);
            }
        }

        created = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
