using UnityEngine;

public class FixedJointScript : MonoBehaviour
{
    
    // Components
    private Rigidbody _connected;
    private Rigidbody _cube;
    private FixedJoint _joint;
    
    // Variables
    private float _vBreakForce = 500f, _vTorqueForce = 500f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _cube = gameObject.GetComponent<Rigidbody>();
        _connected = GameObject.Find("Connected").GetComponent<Rigidbody>();
        _joint = _cube.gameObject.AddComponent<FixedJoint>();
        _joint.connectedBody = _connected;
        _joint.breakForce = _vBreakForce;
        _joint.breakTorque = _vTorqueForce;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_joint == null) Debug.Log("Joints are null");
    }
}