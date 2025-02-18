using Unity.Jobs;
using UnityEngine;

public class SpringJointScript : MonoBehaviour
{
    private SpringJoint _cJoint;
    Rigidbody _cRBConected;
    Rigidbody _cRBOwn;

    public float _vSpring = 5f, _vDamper = 0.5f, _vMaxDistance = 2f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cRBConected = GameObject.Find("Connected").GetComponent<Rigidbody>();
        _cRBOwn = GetComponent<Rigidbody>();
        _cJoint = _cRBOwn.gameObject.AddComponent<SpringJoint>();
        _cJoint.connectedBody = _cRBConected;
        _cJoint.spring = _vSpring;
        _cJoint.damper = _vDamper;
        _cJoint.maxDistance = _vMaxDistance;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
