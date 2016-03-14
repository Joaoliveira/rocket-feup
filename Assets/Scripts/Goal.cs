using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
    private GameObject opponent;

	void OnTriggerEnter2D(Collider2D other) {
        //make changes to opponent here
        opponent = GameObject.FindGameObjectWithTag("Player");
        opponent.GetComponent<FourWD>().score++;
    }
}
