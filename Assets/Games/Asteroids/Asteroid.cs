using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KvaGames.Asteroids
{
	[RequireComponent(typeof(Rigidbody))]
	public class Asteroid : MonoBehaviour
	{
		[SerializeField]
		private byte size;
		private int health;
		public byte Size {get{return size;}}
		public int Health {get{return health;}}

		private Rigidbody rb;

		private void Start( )
		{
			rb = rb ?? GetComponent<Rigidbody>();
			rb.AddTorque(Random.insideUnitSphere, ForceMode.VelocityChange);
			rb.AddForce(Random.insideUnitSphere, ForceMode.Impulse);
			health = 10 * Size;
			EventHandeler.TriggerAsteroidSpawn(this);
		}
		private void OnCollisionEnter(Collision collision)
		{
			if (collision.relativeVelocity.magnitude > 20)
				AddDamage(1);
		}
		public void AddDamage(int points)
		{
			health -= points;
			EventHandeler.TriggerAsteroidDamage(this);

		}
		void Update()
		{
			//ensure z is always 0.
			Vector3 pos = transform.position;
			pos.z = 0;
			transform.position = pos;
		}
	}
}