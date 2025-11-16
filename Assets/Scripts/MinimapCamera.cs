using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    void LateUpdate()
    {
        transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);
        transform.rotation = Quaternion.Euler(90f, Camera.main.transform.eulerAngles.y, 0f);    
    }
}
