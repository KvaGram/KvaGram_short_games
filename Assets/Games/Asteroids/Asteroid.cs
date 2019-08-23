using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KvaGames.Asteroids
{
	[RequireComponent(typeof(Rigidbody))]
	public class Asteroid : AstroBehaviour
	{
		[SerializeField]
		private byte size;

        public byte Size {get{return size;}}
        public int health;


        private void Awake( )
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

        private new void Update()
		{
            base.Update();
            if (escaped)
                return;
			//ensure z is always 0.
			Vector3 pos = transform.position;
			pos.z = 0;
			transform.position = pos;
            
		}


        protected override void OutofboundsY(bool upper)
        {
            if (escaped)
                return;
            Escape();
        }
        protected override void HandleWarped(Vector3 warp)
        {
            //throw new System.NotImplementedException();
        }

        protected override void HandleEscaped() => controller.ReplaceAsteroid(this);
    }
}