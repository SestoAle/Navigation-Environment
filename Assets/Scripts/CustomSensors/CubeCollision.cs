using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeCollision : MonoBehaviour
{

    public float distance;
    public GameObject collisionObject;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Return the object colliding this collision cube
    public GameObject getGameObject()
    {
        int wallMask = 1 << LayerMask.NameToLayer("Wall");
        int coinMask = 1 << LayerMask.NameToLayer("Coin");
        int rampMask = 1 << LayerMask.NameToLayer("Ramp");
        int planeMask = 1 << LayerMask.NameToLayer("Plane");

        int finalMask = wallMask | rampMask | planeMask | coinMask;

        Collider[] hitColliders =
            Physics.OverlapBox(gameObject.transform.position,
            transform.localScale / 2, Quaternion.identity, finalMask);


        if (hitColliders.Length > 0)
        {
            return hitColliders[0].gameObject;
        }

        return null;
    }
}
