using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KvaGames.Asteroids
{
	public class Bomb : AstroBehaviour
	{
        public float fuselength = 20f;
		private float fuseTimer = 0;
		[SerializeField]
		float radius = 100f;
		[SerializeField]
		float explosionForce = 50;
		[SerializeField]
		int baseDamage = 1000;

        private bool detonated = false;


        [SerializeField]
        private Light[] warnlights;
        [SerializeField]
        private GameObject mesh;
		[SerializeField]
		private GameObject radarObject;
		// Use this for initialization
		void Start( )
		{
            fuseTimer = 0;
            //warnlight = warnlight ?? GetComponentInChildren<Light>();
            
        }

        // Update is called once per frame
        private new void Update()
		{
            if (detonated)
                return;
			fuseTimer += Time.deltaTime;
			if (fuseTimer >= fuselength)
			{
				Detonate(); 
			}
            float freq = fuseTimer / fuselength * 10;
            foreach (Light warnlight in warnlights)
                warnlight.color = Color.white * Mathf.PingPong(fuseTimer*freq, 1);
            base.Update();
		}
		private void FixedUpdate( )
		{
			//if(Mathf.CeilToInt(fuseTimer) > 0)
			//	Debug.Log(Mathf.CeilToInt(fuseTimer));
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

            //Enlarges the radar object so to visualize the boom
            radarObject.transform.localScale = new Vector3(radius, radius, radius);
            //disables the ridigbody
            rb.isKinematic = true;
            //hides the normal mesh
            Destroy(mesh);
            //sets the rest of the object to die soon.
            Destroy(gameObject, 1f);

            detonated = true;
        }

        protected override void OutofboundsY(bool upper)
        {
            Escape();
            //throw new System.NotImplementedException();
        }

        protected override void HandleWarped(Vector3 warp)
        {
            //throw new System.NotImplementedException();
        }

        protected override void HandleEscaped()
        {
            Destroy(gameObject);
        }
    }
}