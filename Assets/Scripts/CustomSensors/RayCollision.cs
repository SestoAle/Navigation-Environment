using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCollision
{
    private GameObject _gameObject;
    private float _distance;

    public RayCollision(GameObject gameObject, float distance)
    {
        _gameObject = gameObject;
        _distance = distance;
    }

    public GameObject getGameObject()
    {
        return _gameObject;
    }

    public float getDistance()
    {
        return _distance;
    }
}
