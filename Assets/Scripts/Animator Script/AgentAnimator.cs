using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimator : MonoBehaviour
{
    private Animator _animator;

    private Rigidbody _rigidbody;

    private BugAgent _agentComponent;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _agentComponent = GetComponent<BugAgent>();
    }

    private void FixedUpdate()
    {
        Vector3 velocity = _rigidbody.linearVelocity;
        Vector3 isMoving = new Vector3(_agentComponent._horizontal, _agentComponent._jump , _agentComponent._vertical);

        _animator.SetFloat("agent_velocity", isMoving.magnitude);
        _animator.SetBool("agent_jump", !_agentComponent._isGrounded);
        _animator.SetBool("agent_double_jump", !_agentComponent._doubleJump);
        _animator.SetBool("agent_climbing", _agentComponent._isAttached);
        
        
    }
}
