using UnityEngine;

public class CameraCenter : MonoBehaviour
{
    
    public float minDistance = 5;
    public float followDistance;
    public Transform p1, p2;
    public Transform ball;
    public float offset;
    public float minY = 5f; //The minimum cam size (vertically) will be 5
    float orthoSize; //Our orthographic cam size
    Camera cam;
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void SetCamPos()
    {
        Vector3 pos = (p1.position + p2.position) * 0.35f + ball.position * 0.15f;

        transform.position = new Vector3(pos.x, 0, transform.position.z); //we move it in X only, keep Y the same, and as it is 2d, we don't mess with Z
    }

    void SetCamSize()
    {
        float screenRatio = Screen.width / Screen.height;

        float minX = minY * screenRatio;

        float widthPlayers = Mathf.Abs(p1.position.x - p2.position.x) * 0.5f;
        float heightPlayers = Mathf.Abs(p1.position.y - p2.position.y) * 0.5f;

        float width = widthPlayers * 0.7f + ball.position.x * 0.15f; //80% for the middle of the players, 20% for the ball position
        float height = heightPlayers * 0.7f + ball.position.y * 0.15f; //80% for the middle of the players, 20% for the ball position

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