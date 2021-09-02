using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertical2DGrid : Horizontal2DGrid
{
    public float _gridLength = 1f;
    public float offset = 0.5f;
    
    
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
                newCube.transform.localScale = new Vector3(_gridScale, _gridScale, _gridLength);

                // Get the position in the grid
                // TODO: If 3D
                newCube.transform.position = new Vector3(
                    i * _gridScale, 
                    (j * _gridScale) + (float)(_gridScale*_gridWidth/2) + offset, 
                    (int)(_gridLength / 2) + 1 + offset);

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
    
    private new void LateUpdate()
    {
        
    }
}
