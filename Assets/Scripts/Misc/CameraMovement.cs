using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Quaternion _defaultRotation;
    private Vector3 _defaultPosition;
    private Vector3 _offset;
    public BugAgent _agent;
    public float _rotationSpeed = 5f;
    public float _lookSpeed = 10f;
    public bool _lookAgent = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _offset = _agent.transform.position - transform.position;
        _defaultRotation = transform.localRotation;
        _defaultPosition = transform.localPosition;
    }
    
    private void FixedUpdate()
    {
        Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * _rotationSpeed, Vector3.up);
        _offset = camTurnAngle * _offset;
        
        Vector3 newPos = _agent.transform.position - _offset;
        transform.position = Vector3.Slerp(transform.position, newPos, 0.5f);
        
        // Look at the player
        if (_lookAgent)
            transform.LookAt(_agent.transform);
        
        // var targetRotation = Quaternion.LookRotation(_agent.transform.position - transform.position);
        
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _lookSpeed * Time.deltaTime);
        // transform.rotation = Quaternion.LookRotation(_agent.transform.position - transform.position, transform.right);
    }
}
