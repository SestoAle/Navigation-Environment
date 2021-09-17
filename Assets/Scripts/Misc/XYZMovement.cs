using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XYZMovement : MonoBehaviour
{
    public float _rangeX;

    public float _rangeY;

    public float _rangeZ;

    public int _waitTime;

    public int _time;
    int elapsedFrames = 0;

    private Vector3 _startingPosition;

    private Vector3 _endPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (elapsedFrames == 0)
        {
            _rangeX = -_rangeX;
            _rangeY = -_rangeY;
            _rangeZ = -_rangeZ;
            _startingPosition = gameObject.transform.position;
            _endPosition = _startingPosition + new Vector3(_rangeX, 0, _rangeY);
        }

        float interpolationRatio = (float)elapsedFrames / _time;
        Vector3 interpolatedPosition = Vector3.Lerp(_startingPosition, _endPosition, interpolationRatio);
        elapsedFrames = (elapsedFrames + 1) % (_time + _waitTime + 1);
        if ((elapsedFrames + 1) <= _time + 1)
        {
            gameObject.transform.position = interpolatedPosition;   
        }
    }
}
