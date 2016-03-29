using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
    public GameObject opponent;
    public GameObject friendly;
    private Vector3 startingPositionBall;
    private Vector3 startingPositionFriendly;
    private Vector3 startingPositionOpponent;
    private Transform opponentChassis;
    private Transform friendlyChassis;
    private GameObject ball;

    void Start()
    {
        ball = GameObject.Find("Ball");
        opponentChassis = opponent.transform.Find("NomadChassis");
        friendlyChassis = friendly.transform.Find("NomadChassis");
        startingPositionOpponent = opponentChassis.position;
        startingPositionFriendly = friendlyChassis.position;
        startingPositionBall = ball.transform.position;
    }

	void OnTriggerEnter2D(Collider2D other) {
        if (other.name != "Ball") return;
        //make changes to opponent here
        //opponent = GameObject.FindGameObjectWithTag("Player");
        opponent.GetComponent<Score>().incrementScore();

        opponentChassis.GetComponent<FourWD>().Reset();
        friendlyChassis.GetComponent<FourWD>().Reset();

        Transform[] allChildren = opponent.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            try
            {
                Rigidbody2D rb = child.gameObject.GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0.0f;

            }catch(MissingComponentException)
            {

            }
            
        }

        allChildren = friendly.GetComponentsInChildren<Transform>();

        foreach (Transform child in allChildren)
        {
            try
            {
                Rigidbody2D rb = child.gameObject.GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0.0f;

            }
            catch (MissingComponentException)
            {

            }

        }

        opponent.transform.Find("NomadChassis").position = startingPositionOpponent;
        opponent.transform.Find("NomadChassis").rotation = Quaternion.identity;
        friendly.transform.Find("NomadChassis").position = startingPositionFriendly;
        friendly.transform.Find("NomadChassis").rotation = Quaternion.identity;


        ball.transform.position = startingPositionBall;
        ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        ball.GetComponent<Rigidbody2D>().angularVelocity = 0.0f;
    }
}
