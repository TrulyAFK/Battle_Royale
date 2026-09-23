using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed;
    public float jumpForce;
    [Header("Components")]
    public Rigidbody rig;

    private void Update()
    {
        Move();
        if (Input.GetKey(KeyCode.Space))
        {
            TryJump();
        }
    }
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 dir = (transform.forward*z+transform.right*x)*moveSpeed;
        dir.y = rig.linearVelocity.y;
        rig.linearVelocity = dir;
    }
    void TryJump()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, 1.5f))
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
