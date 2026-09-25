using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Look Sensivity")]
    public float sensX;
    public float sensY;
    [Header("Clamping")]
    public float minY;
    public float maxY;
    [Header("Spectator")]
    public float spectatorMoveSpeed;
    private float rotX;
    private float rotY;
    private bool isSpectator;

    void Start()
    {
        Cursor.lockState=CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        //if(Cursor.lockState==CursorLockMode.Locked){
            rotX += Input.GetAxis("Mouse X") * sensX;
            rotY += Input.GetAxis("Mouse Y") * sensY;
            rotY = Mathf.Clamp(rotY, minY, maxY);  
        //}
        if (isSpectator)
        {
            transform.rotation = Quaternion.Euler(-rotX, rotY, 0);
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            float y = 0;
            if (Input.GetKey(KeyCode.E))
            {
                y = 1;
            } else if (Input.GetKey(KeyCode.Q))
            {
                y = -1;
                Vector3 dir = transform.right * x + transform.up * y + transform.forward * z;
                transform.position += dir * spectatorMoveSpeed * Time.deltaTime;
            }
        }
        else
        {
            transform.localRotation = Quaternion.Euler(-rotY,0,0);
            transform.parent.localRotation = Quaternion.Euler(transform.rotation.x,rotX,0);
        }
    }

}
