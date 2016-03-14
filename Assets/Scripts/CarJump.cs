using UnityEngine;
using System.Collections;

public class CarJump : MonoBehaviour {

    public float jumpForce;
    public Rigidbody2D frontWheel, rearWheel;

    private Rigidbody2D rigidBody;
    private float distToGround;
    private LayerMask layerMask;
    private int jumpState;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        layerMask = LayerMask.GetMask(new string[] { "Default" });
        jumpState = 0;
    }

    bool wheelsGrounded()
    {
        RaycastHit2D rayHitsFront = Physics2D.Raycast(frontWheel.position, -Vector2.up, 1.0f, layerMask);
        RaycastHit2D rayHitsRear = Physics2D.Raycast(rearWheel.position, -Vector2.up, 1.0f, layerMask);
        return rayHitsFront.collider != null && rayHitsRear.collider != null;
    }

	// Update is called once per frame
	void Update () {
        if (wheelsGrounded()) jumpState = 0;

        if (jumpState < 2)
        {
            if(Input.GetKeyUp("s"))
            {
                jumpCenter();
            }else if (Input.GetKeyUp("d"))
            {
                jumpFront();
            }
            else if (Input.GetKeyUp("a"))
            {
                jumpBack();
            }
        }
	}

    private void jumpCenter()
    {
        rigidBody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        jumpState++;
    }

    private void jumpBack()
    {
        frontWheel.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        jumpState++;
    }

    private void jumpFront()
    {
        rearWheel.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        jumpState++;
    }
}
