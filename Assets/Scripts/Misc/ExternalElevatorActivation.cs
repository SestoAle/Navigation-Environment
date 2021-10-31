using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalElevatorActivation : MonoBehaviour
{

    public XYZMovement _elevator;
    public float _distanceThreshold = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Agent"))
        {
            if(Vector3.Distance(_elevator.gameObject.transform.position, transform.position) > _distanceThreshold)
                _elevator.SetActivated();
        }
    }
}
