using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XYZMovement : MonoBehaviour
{
    public float _rangeX;

    public float _rangeY;

    public float _rangeZ;

    public bool _mustBeActivated;

    public int _waitTime;

    private bool started = false;

    public int _time;
    int elapsedFrames = 0;

    public Vector3 _startingPosition;
    public Vector3 _endPosition;

    private GameObject _agent;
    public Vector3 _offset;
    public Vector3 _initialPosition;
    public Vector3 _initialRange;

    private bool _activated;
    // Start is called before the first frame update
    void Start()
    {
        _activated = false;
        started = true;
        _initialPosition = transform.position;
        _initialRange = new Vector3(_rangeX, _rangeY, _rangeZ);
        ResetMovement();
    }

    public void ResetMovement()
    {
        if(started)
        {
            transform.position = _initialPosition;
            _rangeX = _initialRange.x;
            _rangeY = _initialRange.y;
            _rangeZ = _initialRange.z;
            _activated = false;
            elapsedFrames = 0;
        }
    }

    public void SetActivated()
    {
        _activated = true;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Agent"))
        {
            collision.attachedRigidbody.MovePosition(collision.attachedRigidbody.position + _offset);
            if (_mustBeActivated)
                _activated = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (_mustBeActivated && !_activated)
            return;
        
        if (elapsedFrames == 0)
        {
            _rangeX = -_rangeX;
            _rangeY = -_rangeY;
            _rangeZ = -_rangeZ;
            _startingPosition = gameObject.transform.position;
            _endPosition = _startingPosition + new Vector3(_rangeX, _rangeY, _rangeZ);
            _activated = false;
        }

        Vector3 previousPosition = transform.position;
        float interpolationRatio = (float)elapsedFrames / _time;
        Vector3 interpolatedPosition = Vector3.Lerp(_startingPosition, _endPosition, interpolationRatio);
        elapsedFrames = (elapsedFrames + 1) % (_time + _waitTime + 1);
        if ((elapsedFrames + 1) <= _time + 1)
        {
            gameObject.transform.position = interpolatedPosition;
        }

        _offset = transform.position - previousPosition;
    }
}
