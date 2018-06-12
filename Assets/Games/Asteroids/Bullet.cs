using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KvaGames.Asteroids
{
	public class Bullet : MonoBehaviour
	{
		private float deathcount = 20;
		private Rigidbody rb;
		public Rigidbody Rb
		{ get { if (rb) return Rb; rb = GetComponent<Rigidbody>(); return rb; } }
		void OnCollisionEnter(Collision collision)
		{
			Destroy(gameObject, 0.1f);
		}


		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		void Update()
		{
			deathcount -= Time.deltaTime;
			if (deathcount < 0)
				Destroy(gameObject);
		}
	}
}