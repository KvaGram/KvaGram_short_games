using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KvaGames.Asteroids
{
	public class Bomb : AstroBehaviour
	{
		private Rigidbody rb;
		public Rigidbody Rb
		{ get { if (rb) return Rb; rb = GetComponent<Rigidbody>(); return rb; } }

		[SerializeField]
		float fuseTime = 20f;
		[SerializeField]
		float radius = 100f;
		[SerializeField]
		float explosionForce = 50;
		[SerializeField]
		int baseDamage = 1000;

		[SerializeField]
		private GameObject radarObject;
		// Use this for initialization
		void Start( )
		{

		}

        // Update is called once per frame
        private new void Update()
		{
			if (fuseTime > 0)
			{
				fuseTime -= Time.deltaTime;
				if (fuseTime<=0)
				{
					Detonate();
					radarObject.transform.localScale = new Vector3(radius, radius, radius);
					Destroy(gameObject, 1f);
				}
			}
            base.Update();
		}
		private void FixedUpdate( )
		{
			if(Mathf.CeilToInt(fuseTime) > 0)
				Debug.Log(Mathf.CeilToInt(fuseTime));
		}
		public void Detonate()
		{
			Debug.Log("BOOM!");
			Vector3 posA = transform.position;

			Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
			foreach(Collider c in colliders)
			{
				Vector3 posB = c.transform.position;
				Vector3 dir = posB - posA;
				int damageForce = Mathf.CeilToInt( (radius-dir.magnitude)/radius * baseDamage);

				Rigidbody rb = c.GetComponent<Rigidbody>();
				Asteroid ast = c.transform.parent?.GetComponent<Asteroid>();
				Player plr = c.GetComponent<Player>();
				if(rb)
				{
					rb.AddExplosionForce(explosionForce, transform.position, radius, 0, ForceMode.Force);
				}
				if(ast)
				{
					ast.AddDamage(damageForce);

				}
				if(plr)
				{
					plr.Damage(1); //being kind to the player here...
				}
			}
		}

        protected override void OutofboundsY(bool upper)
        {
            //throw new System.NotImplementedException();
        }

        protected override void HandleWarped(Vector3 warp)
        {
            //throw new System.NotImplementedException();
        }
    }
}