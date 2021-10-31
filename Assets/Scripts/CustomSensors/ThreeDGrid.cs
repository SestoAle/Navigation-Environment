using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDGrid : Horizontal2DGrid
{
    public float offset = 0.01f;
    public CubeCollision[,,] gridMatrix;

    // Create local grid
    new public void createLocalGrid()
    {
        // Initialize gridMatrix
        gridMatrix = new CubeCollision[_gridWidth, _gridWidth, _gridWidth];
        
        // Statistics for normalizing distances
        float maxDistance = -9999999;
        float minDistance = 9999999;
        
        // Create gridPerception
        for (int i = -_gridWidth / 2; i < _gridWidth / 2 + 1; i++)
        {
            for (int j = -_gridWidth / 2; j < _gridWidth / 2 + 1; j++)
            {
                for (int z = -_gridWidth / 2; z < _gridWidth / 2 + 1; z++)
                {
                    // Instantiate the cube and add it to the grid
                    var newCube = Instantiate(_gridCube,
                        new Vector3(0, 0, 0),
                        Quaternion.identity);
                    newCube.transform.parent = gameObject.transform;


                    // Change the scale of the cube grid
                    newCube.transform.localScale = new Vector3(_gridScale, _gridScale, _gridScale);

                    // Get the position in the grid
                    // TODO: If 3D
                    
                    // Spacing the cubes
                    float _localOffset_i = 0;
                    float _localOffset_j = 0;
                    float _localOffset_z = 0;

                    if (i < 0)
                    {
                        _localOffset_i = -offset;
                    }
                    else if (i > 0)
                    {
                        _localOffset_i = offset;
                    }
                    
                    if (j < 0)
                    {
                        _localOffset_j = -offset;
                    }
                    else if (j > 0)
                    {
                        _localOffset_j = offset;
                    }
                    
                    if (z < 0)
                    {
                        _localOffset_z = -offset;
                    }
                    else if (z > 0)
                    {
                        _localOffset_z = offset;
                    }
                    
                    newCube.transform.position = new Vector3(
                        i * _gridScale + _localOffset_i * Math.Abs(i),
                        j * _gridScale + _localOffset_j * Math.Abs(j),
                        z * _gridScale + _localOffset_z * Math.Abs(z));
                    
                    float distance =
                        Vector3.Distance(newCube.transform.position, new Vector3(0, 0, 0));
                    newCube.GetComponent<CubeCollision>().distance = distance;

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                    }

                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                    }
                    
                    // Add to list
                    gridCubes.Add(newCube); 
                }
                
            }
        }
        
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int z = 0; z < _gridWidth; z++)
            {
                for (int j = 0; j < _gridWidth; j++)
                {
                    // Normalizing distance between 0 and 1
                    gridCubes[0].GetComponent<CubeCollision>().distance /= maxDistance;
                    gridMatrix[i, j, z] = gridCubes[0].GetComponent<CubeCollision>();
                    gridCubes.RemoveAt(0);
                }
            }
        }

        created = true;
    }
}
