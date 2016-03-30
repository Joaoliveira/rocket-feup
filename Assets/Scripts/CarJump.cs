using UnityEngine;
using System.Collections;
using System;

public class CarJump : MonoBehaviour
{

    public float jumpForce;
    public Rigidbody2D frontWheel, rearWheel;
    public string jumpButton, flipButton, horizontalAxis;
    public bool facingRight = true;			// For determining which way the car is currently facing.

    private Rigidbody2D rigidBody;
    private LayerMask layerMask;
    private int jumpState;
    private float torqueDir; // to detect whether torque is being applied
    private float distanceToGround;
	// int groundLayer = 18; // Debug.Log(LayerMask.NameToLayer("Ground"));
	private float timeUpsideDown = 0f;

	// Use this for initialization
	void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        layerMask = LayerMask.GetMask(new string[] { "Ground" });
        jumpState = 0;

        distanceToGround = 1.0f; // idealmente iríamos buscar isto tipo collider.bounds.extents.y; mas parece que não há para 2d
        // Flip();
    }

    public bool wheelsGrounded() // mudar para wheels -> istouching ou parecido
    {
        /*
		CircleCollider2D rearWheelCollider = rearWheel.GetComponentInChildren<CircleCollider2D>();
		CircleCollider2D frontWheelCollider = frontWheel.GetComponentInChildren<CircleCollider2D>();

		if (rearWheelCollider.IsTouchingLayers(groundLayer))
			print("rear wheel touching Ground layer");
		if (frontWheelCollider.IsTouchingLayers(groundLayer))
			print("front wheel touching Ground layer");

        print("rear wheel collider: " + rearWheelCollider.ToString());
        print("front wheel collider: " + frontWheelCollider.ToString());

        return rearWheelCollider.IsTouchingLayers(groundLayer) && rearWheelCollider.IsTouchingLayers(groundLayer);
        */

        RaycastHit2D rayHitsFront = Physics2D.Raycast(frontWheel.position, -Vector2.up, distanceToGround + 0.1f, layerMask);
        RaycastHit2D rayHitsRear = Physics2D.Raycast(rearWheel.position, -Vector2.up, distanceToGround + 0.1f, layerMask);

        return (rayHitsFront.collider != null) && (rayHitsRear.collider != null);
    }

	void OnCollisionStay2D(Collision2D col)
	{
		print(col.gameObject.name);
		print(col.gameObject.tag);

		if (col.gameObject.tag == "Ground") {
			timeUpsideDown += Time.deltaTime;
			print("timeUpsideDown: " + timeUpsideDown);
			if (timeUpsideDown > 2)
			{
				jumpFlip();
				timeUpsideDown = 0f;
			}
		}
	}

    public bool carUpsideDownGrounded() // mudar para wheels -> istouching ou parecido
    {
        RaycastHit2D rayHitsTop = Physics2D.Raycast(rigidBody.position, -Vector2.up, distanceToGround + 0.5f, layerMask);
        return rayHitsTop.collider != null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (wheelsGrounded()) jumpState = 0;
        torqueDir = Input.GetAxis(horizontalAxis);
        if (!facingRight) torqueDir *= -1;
        if (jumpState < 2)
        {
            if (Input.GetKeyDown(jumpButton))
            {
                if (carUpsideDownGrounded())
                    jumpFlip();

                if (torqueDir > 0) jumpFront();
                else
                if (torqueDir < 0) jumpBack();
                else
                    jumpCenter();
            }
        }

        if (Input.GetKeyDown(flipButton))
            Flip();
       
    }

    private void Flip()
    {
        // Switch the way the car is labelled as facing.
        facingRight = !facingRight;

        if (facingRight) print("Car is now facing right");
        if (!facingRight) print("Car is now facing left");

        // Multiply the player's x local scale by -1.
        /*
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        */
        Vector3 theScale = transform.localScale; // mudar para carro completo (incl. rodas)
        theScale.x *= -1.0f;
        transform.localScale = theScale;
        print(theScale);
    }

    private void jumpCenter()
    {
        rigidBody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        jumpState++;
    }

    private void jumpBack()
    {
        frontWheel.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        rigidBody.AddForce(-transform.right * jumpForce * 0.2f, ForceMode2D.Impulse);
        jumpState++;
    }

    private void jumpFront()
    {
        rearWheel.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        rigidBody.AddForce(transform.right * jumpForce * 0.2f, ForceMode2D.Impulse);
        jumpState++;
    }

    private void jumpFlip()
    {
        rigidBody.AddForce(-1.0f * transform.up * jumpForce, ForceMode2D.Impulse);
    }
}
