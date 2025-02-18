using UnityEngine;

public class HingeJointScript : MonoBehaviour
{
    private HingeJoint _cJoint;
    Rigidbody _rigidbodyConected;
    Rigidbody _rigidbodyOwn;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbodyConected = GameObject.Find("Connected").GetComponent<Rigidbody>();
        _rigidbodyOwn = gameObject.GetComponent<Rigidbody>();
        _cJoint = _rigidbodyOwn.gameObject.AddComponent<HingeJoint>();
        _cJoint.connectedBody = _rigidbodyConected;
        
        JointLimits limits = _cJoint.limits;
        limits.min = 0f;
        limits.max = 90f;
        _cJoint.limits = limits;
        _cJoint.useLimits = true;
        
        JointSpring spring = _cJoint.spring;
        spring.spring = 10f;
        spring.damper = 0.2f;
        _cJoint.spring = spring;
        _cJoint.useLimits = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
