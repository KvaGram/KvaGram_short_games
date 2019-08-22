using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KvaGames.Asteroids
{
	public class Bullet : AstroBehaviour
	{
		private float deathcount = 20;
		private Rigidbody rb;
		public Rigidbody Rb
		{ get { if (rb) return Rb; rb = GetComponent<Rigidbody>(); return rb; } }

        protected override void OutofboundsY(bool upper)
        {
            //throw new System.NotImplementedException();
        }
        protected override void HandleWarped(Vector3 warp)
        {
            //throw new System.NotImplementedException();
        }

        void OnCollisionEnter(Collision collision)
		{
			Destroy(gameObject, 0.1f);
		}


		// Use this for initialization
		void Start()
		{

		}

		// Update is called once per frame
		private new void Update()
		{
			deathcount -= Time.deltaTime;
			if (deathcount < 0)
				Destroy(gameObject);
            else
                base.Update();

		}
	}
}