using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    
    // The door that will obe opened by the key
    public Door _doorToOpen;
    public Door _doorToClose;
    public int _id = 0;
    
    public bool _isPickedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // If picked up, disable rendering
        if (_isPickedUp)
            GetComponent<MeshRenderer>().enabled = false;

        // if there is an agent around
        if (GameManager.instance.objectIsPressed(gameObject))
        {
            // If it is not already picked up
            if (!_isPickedUp)
            {
                _isPickedUp = true;
                // Open the door
                _doorToOpen.gameObject.SetActive(false);
                
                // Close the door, if any door has to be closed
                if(_doorToClose != null)
                    _doorToClose.gameObject.SetActive(true);

                // Make the key disappear
                gameObject.SetActive(false);
            }
        }
        
    }
    
    
}
