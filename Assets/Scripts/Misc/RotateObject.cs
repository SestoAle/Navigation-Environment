using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{

    public float speed = 8;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up * Time.deltaTime * speed);
    }
}
