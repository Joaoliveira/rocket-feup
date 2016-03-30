using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour
{
	public GameObject explosion;        // Prefab of explosion effect.


	void Start()
	{
		// Destroy the rocket after 2 seconds if it doesn't get destroyed before then.
		Destroy(gameObject, 7);

		print("rocket in layer: " + gameObject.layer);
	}


	void OnExplode()
	{
		// Create a quaternion with a random rotation in the z-axis.
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		// Instantiate the explosion where the rocket is with the random rotation.
		Instantiate(explosion, transform.position, randomRotation);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		print("collided with " + col.tag);

		// If it hits an enemy...
		if (col.tag == "Ball")
		{
			// ... find the Enemy script and call the Hurt function.
			// col.gameObject.GetComponent<Enemy>().Hurt();

			// Call the explosion instantiation.
			OnExplode();

			// Destroy the rocket.
			Destroy(gameObject);
		}
		else if (col.tag == "Nomad_MIEIC")
		{
			print("Collided with Nomad_MIEIC");

			OnExplode();
			Destroy(gameObject);

			// jumpleft / right
		}
		else if (col.tag == "Nomad_MIEEC")
		{
			print("Collided with Nomad_MIEEC");
			// do something
		}
		else { // environment
			print("rocket collided with: " + col.gameObject.tag);
			OnExplode();
			Destroy(gameObject);
		}


	}
}
