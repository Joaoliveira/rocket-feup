using UnityEngine;

public class CameraCenter : MonoBehaviour
{
    
    public float minDistance = 5;
    public float followDistance;
    public Transform p1, p2;
    public float offset;
    float minY = 5f; //The minimum cam size (vertically) will be 5
    float orthoSize; //Our orthographic cam size
    Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void SetCamPos()
    {
        Vector3 pos = (p1.position + p2.position) * 0.5f;

        transform.position = new Vector3(pos.x, 0, transform.position.z); //we move it in X only, keep Y the same, and as it is 2d, we don't mess with Z
    }

    void SetCamSize()
    {
        float screenRatio = Screen.width / Screen.height;

        float minX = minY * screenRatio;

        float width = Mathf.Abs(p1.position.x - p2.position.x) * 0.5f;
        float height = Mathf.Abs(p1.position.y - p2.position.y) * 0.5f;

        float camX = Mathf.Max(width, minX);

        orthoSize = Mathf.Max(height, camX / screenRatio, minY);

        cam.orthographicSize = orthoSize + offset;
        
    }
    // Update is called once per frame
    void Update()
    {
        SetCamPos();
        SetCamSize();   
    }
}