using UnityEngine;

public class Drop : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    Rigidbody rigidbody;
    bool nearFloor = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, Vector3.down * 10);
        
        RaycastHit hit;
        
        Physics.Raycast(ray, out hit, 10f);
        Debug.DrawRay(transform.position, Vector3.down * 10, Color.red);
        if (!nearFloor && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            meshRenderer.enabled = true;
            rigidbody.linearDamping = 5f;
            nearFloor = true;
        }
    }
}
