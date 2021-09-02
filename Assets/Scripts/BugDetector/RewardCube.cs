using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCube : MonoBehaviour
{
    public bool _rewardAlreadyTaken = false;
    public float reward = 20f;

    public GameObject _target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Agent"))
        {
            if (!_rewardAlreadyTaken)
            {
                other.gameObject.GetComponent<BugAgent>().SetReward(reward);
                //gameObject.SetActive(false);
                //_rewardAlreadyTaken = true;
            }
             
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Agent"))
        {
            if (!_rewardAlreadyTaken)
            {
                other.gameObject.GetComponent<BugAgent>().SetReward(reward);
                //gameObject.SetActive(false);
                //_rewardAlreadyTaken = true;
            }
             
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
