using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor.Experimental;
using UnityEngine;

public class Rays : MonoBehaviour
{

    public List<RaycastHit> _hits;
    public float _sphereRadius = 1f;
    public bool debug = false;
    public List<float> _heights = new List<float>();
    public float _maxDistance = 500;
    
    
    private RaycastHit GetRayCastHit(float maxDistance, float angle, float height)
    {
        _hits.Clear();
        int wallMask = 1 << LayerMask.NameToLayer("Wall");
        int coinMask = 1 << LayerMask.NameToLayer("Coin");
        int rampMask = 1 << LayerMask.NameToLayer("Ramp");
        int climbableMask = 1 << LayerMask.NameToLayer("Climbable");
        int planeMask = 1 << LayerMask.NameToLayer("Plane");

        int finalMask = wallMask | rampMask | planeMask | coinMask | climbableMask;
        
        
        RaycastHit hit;
        Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward;
        Vector3 pos = gameObject.transform.position;
        pos.y += height;

        if (Physics.SphereCast(pos, _sphereRadius, direction, out hit, maxDistance, finalMask))
        {
            // if(debug)
            Debug.DrawRay(pos, direction * hit.distance, Color.yellow, 1/60f);
            return hit;
        }
        else
        {
            // if(debug)
            Debug.DrawRay(pos, direction * maxDistance, Color.white, 1/60f);
            return hit;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _hits = new List<RaycastHit>();
    }

    public List<RayCollision> HitRays()
    {    
        List<RayCollision> _rayCollisions = new List<RayCollision>();
        foreach (float h in _heights)
        {
            for (int i = 0; i < 360; i += 45)
            {
                RaycastHit hit = GetRayCastHit(_maxDistance, i, h);
                GameObject hitGO = hit.collider != null ? hit.collider.gameObject : null;
                RayCollision rc = new RayCollision(hitGO, hit.distance);
                _rayCollisions.Add(rc);
            }       
        }
        
        return _rayCollisions;
    }

    public void FixedUpdate()
    {
    }
}
