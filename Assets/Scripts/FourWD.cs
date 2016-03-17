using UnityEngine;
using System.Collections;


public class FourWD : MonoBehaviour
{
	//reference to the wheel joints
	WheelJoint2D[] wheelJoints;
	
	//center of mass of the car
	public Transform centerOfMass;
	//reference tot he motor joint
	JointMotor2D motorBack;
	JointMotor2D motorFront;
	//horizontal movement keyboard input
	float dir = 0f;
	//input for rotation of the car
	public float torqueDir = 0f;
	//max fwd speed which the car can move at
	float maxFwdSpeed = -5000;
	//max bwd speed
	float maxBwdSpeed = 2000f;
	//the rate at which the car accelerates
	float accelerationRate = 1000;
	//the rate at which car decelerates
	float decelerationRate = -200;
	//how soon the car stops on braking
	float brakeSpeed = 2500f;
	//acceleration due to gravity
	float gravity = 9.81f;
	//angle in which the car is at wrt the ground
	float slope = 0;
	//reference to the wheels

	public Transform rearWheel;
	public Transform frontWheel;

	// Use this for initialization 
	void Start()
	{
        //set the center of mass of the car
        GetComponent<Rigidbody2D>().centerOfMass = centerOfMass.transform.localPosition;
        print("car center of mass (before): " + GetComponent<Rigidbody2D>().centerOfMass);
        GetComponent<Rigidbody2D>().centerOfMass += new Vector2(3.0f, -1); // move it down

        print("car center of mass (after): " + GetComponent<Rigidbody2D>().centerOfMass);

        // print("car x: " + GetComponent<Rigidbody2D>().centerOfMass.x);
        // print("car y: " + GetComponent<Rigidbody2D>().centerOfMass.y);
        // print("car width: " + GetComponent<Rigidbody2D>().centerOfMass

        //add ability to rotate the car around its axis
        torqueDir = Input.GetAxis("Horizontal");


        //get the wheeljoint components
        wheelJoints = gameObject.GetComponents<WheelJoint2D>();
		
		//get the reference to the motor of front wheels joint
		motorBack = wheelJoints[1].motor;
		motorFront = wheelJoints[0].motor; // qual é a frente?

		//motorBack = rearWheel.GetComponent<WheelJoint2D>();
		// motorFront = frontWheel.GetComponent<WheelJoint2D>().motor;

		// wheelJoints = gameObject.GetComponents<WheelJoint2D>();
		//hingeJoints = rearWheel.GetComponents<HingeJoint2D>();
	}

	//all physics based assignment done here
	void FixedUpdate()
	{
		//add ability to rotate the car around its axis
		torqueDir = Input.GetAxis("Horizontal");
		if (torqueDir != 0)
		{
			// GetComponent<Rigidbody2D>().AddTorque(100 * Mathf.PI * torqueDir, ForceMode2D.Force);
			GetComponent<Rigidbody2D>().AddTorque(300 * Mathf.PI * (-1 * torqueDir), ForceMode2D.Force);
		}
		else {
			GetComponent<Rigidbody2D>().AddTorque(0);
		}

		//determine the cars angle wrt the horizontal ground
		slope = transform.localEulerAngles.z;

		//convert the slope values greater than 180 to a negative value so as to add motor speed 
		//based on the slope angle
		if (slope >= 180)
			slope = slope - 360;
		//horizontal movement input. same as torqueDir. Could have avoided it, but decided to 
		//use it since some of you might want to use the Vertical axis for the torqueDir
		dir = Input.GetAxis("Vertical");

		//explained in the post in detail
		//check if there is any input from the user
		if (dir != 0)
		{
			//add speed accordingly
			motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - (dir * accelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, maxFwdSpeed, maxBwdSpeed);
			motorFront.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - (dir * accelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, maxFwdSpeed, maxBwdSpeed);
		}
		//if no input and car is moving forward or no input and car is stagnant and is on an inclined plane with negative slope
		if ((dir == 0 && motorBack.motorSpeed < 0) || (dir == 0 && motorBack.motorSpeed == 0 && slope < 0))
		{
			//decelerate the car while adding the speed if the car is on an inclined plane
			motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - (decelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, maxFwdSpeed, 0);
			motorFront.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - (decelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, maxFwdSpeed, 0);
		}
		//if no input and car is moving backward or no input and car is stagnant and is on an inclined plane with positive slope
		else if ((dir == 0 && motorBack.motorSpeed > 0) || (dir == 0 && motorBack.motorSpeed == 0 && slope > 0))
		{
			//decelerate the car while adding the speed if the car is on an inclined plane
			motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - (-decelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, 0, maxBwdSpeed);
			motorFront.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - (-decelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, 0, maxBwdSpeed);
		}



		//apply brakes to the car
		if (Input.GetKey(KeyCode.Space) && motorBack.motorSpeed > 0)
		{
			motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - brakeSpeed * Time.deltaTime, 0, maxBwdSpeed);
			motorFront.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - brakeSpeed * Time.deltaTime, 0, maxBwdSpeed);
		}
		else if (Input.GetKey(KeyCode.Space) && motorBack.motorSpeed < 0)
		{
			motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed + brakeSpeed * Time.deltaTime, maxFwdSpeed, 0);
			motorFront.motorSpeed = Mathf.Clamp(motorBack.motorSpeed + brakeSpeed * Time.deltaTime, maxFwdSpeed, 0);
		}
		
		//connect the motor to the joint
		
		wheelJoints[1].motor = motorBack; // para quê isto???
		wheelJoints[0].motor = motorFront; // com isto, a roda da frente não se mexe sem aplicar velocidade no código

	}

}