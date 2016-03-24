using UnityEngine;
using System.Collections;

public class CarJump : MonoBehaviour
{

    public float jumpForce;
    public Rigidbody2D frontWheel, rearWheel;
    public string jumpButton, horizontalAxis;

    private Rigidbody2D rigidBody;
    private float distToGround;
    private LayerMask layerMask;
    private int jumpState;

    float torqueDir;
        // same as in FourWD.cs
        // alternativa a isto? Variável partilhada por carro? (atenção para múltiplos carros)

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        layerMask = LayerMask.GetMask(new string[] { "Default" });
        jumpState = 0;

        //torqueDir to detect whether torque is being applied
    }

    bool wheelsGrounded() // mudar para wheels -> istouching ou parecido
    {
        RaycastHit2D rayHitsFront = Physics2D.Raycast(frontWheel.position, -Vector2.up, 1.1f, layerMask);
        RaycastHit2D rayHitsRear = Physics2D.Raycast(rearWheel.position, -Vector2.up, 1.1f, layerMask);
        return rayHitsFront.collider != null && rayHitsRear.collider != null;
    }

    // Update is called once per frame
    void Update()
    {
        if (wheelsGrounded()) jumpState = 0;
        torqueDir = Input.GetAxis(horizontalAxis);

        if (jumpState < 2)
        {
            if (Input.GetKeyDown(jumpButton))
            {
                if (torqueDir > 0) jumpFront();
                else
                if (torqueDir < 0) jumpBack();
                else
                    jumpCenter();
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
        rigidBody.AddForce(-transform.right * jumpForce, ForceMode2D.Impulse);
        jumpState++;
    }

    private void jumpFront()
    {
        rearWheel.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        rigidBody.AddForce(transform.right * jumpForce, ForceMode2D.Impulse);
        jumpState++;
    }
}
