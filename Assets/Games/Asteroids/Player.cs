using UnityEngine;
namespace KvaGames.Asteroids
{
    [RequireComponent(typeof(ShipShield))]
	[RequireComponent(typeof(Rigidbody))]
	public partial class Player : AstroBehaviour
    {
		[SerializeField]
		private int health;
		public int Health { get { return health; }}

		[SerializeField]
		private Bullet bulletPrefab;
		[SerializeField]
		private float bulletSpeed = 60;
		[SerializeField]
		private Bomb bombPrefab;
		[SerializeField]
		private float bombLanchSpeed = 5;
		[SerializeField]
		private float maxSpeed = 50;
		private ShipShield shield;
		private ParticleSystem engineEffect;

		private void Awake( )
		{
			rb = rb ?? GetComponent<Rigidbody>();
			shield = shield ?? GetComponentInChildren<ShipShield>();
			engineEffect = engineEffect ?? GetComponentInChildren<ParticleSystem>();
		}
		void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag == "Enemy" && !IsShieldActive)
			{
				Damage(1);
			}
		}
		public void Damage(int points)
		{
			health -= points;
			if (health <= 0)
			{
				Debug.Log("GAME OVER");
				Destroy(gameObject, 1);
			}
			if (Health <= 2)
				shield.State = ShipShieldState.danger;
			else
				shield.State = ShipShieldState.safe;
			shieldCooldownCounter = shieldCooldown;
		}

		[SerializeField]
		private float bulletCooldown = 0.2f;
		private float bulletCooldownCounter = 0;
		public bool IsBulletCooldown { get{ return bulletCooldownCounter > 0; } }

		[SerializeField]
		private float shieldCooldown = 0.5f;
		private float shieldCooldownCounter = 0;
		public bool IsShieldActive { get{ return shieldCooldownCounter > 0; } }

		[SerializeField]
		private float bombCooldown = 20.0f;
		private float bombCooldownCounter = 0;
		public bool IsBombCooldown { get { return bombCooldownCounter > 0; } }


        private new void Update()
		{
			//ensure z is always 0.
			Vector3 pos = transform.position;
			pos.z = 0;
			transform.position = pos;

			if (IsShieldActive)
			{
				shieldCooldownCounter -= Time.deltaTime;
				if (!IsShieldActive)
					shield.State = ShipShieldState.inactive;
			}
			if (IsBulletCooldown)
			{
				bulletCooldownCounter -= Time.deltaTime;				
			}
			if(IsBombCooldown)
			{
				bombCooldownCounter -= Time.deltaTime;
			}

			if(rb.velocity.sqrMagnitude > maxSpeed*maxSpeed)
				rb.velocity = Vector3.ClampMagnitude(rb.velocity, 50);
			if(Input.GetMouseButton(1))
			{
				Vector3 boostDir = transform.right;
				boostDir.z = 0;
				rb.AddForce(boostDir.normalized*10f, ForceMode.Acceleration);
				if(!engineEffect.isEmitting)
					engineEffect.Play(true);
			}
			else if(engineEffect.isEmitting)
			{
				
				engineEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			}
			if(Input.GetMouseButton(0) && !IsBulletCooldown)
			{
				Bullet b = Instantiate(bulletPrefab, parent:controller.transform);
				b.transform.position = transform.position + transform.right * 1.3f;
				b.Rb.velocity = rb.velocity + transform.right* bulletSpeed;

				bulletCooldownCounter = bulletCooldown;
			}
			if(Input.GetMouseButton(2) && !IsBombCooldown)
			{
				Bomb b = Instantiate(bombPrefab, parent: controller.transform);
				b.transform.position = transform.position + transform.right * 1.3f;
				b.Rb.velocity = rb.velocity + transform.right* bombLanchSpeed;

				bombCooldownCounter = bombCooldown;
			}
			//if(Input.mousePresent)
			//{
			Vector2 dir = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;

			//float tAngle = Mathf.Atan2(dir.y, dir.x)/6.28f * 360;

			//float cAngle = transform.rotation.y;
			//Vector2 playerPosScreen = Camera.main.WorldToViewportPoint(transform.position);
			//Vector2 mousePos = Input.mousePosition;

			float tAngle = Mathf.Atan2(dir.y, dir.x)*Mathf.Rad2Deg;
			float cAngle = transform.rotation.y;

			//float angleToRotate = tAngle - cAngle;
			Quaternion targetRot = Quaternion.AngleAxis(tAngle, Vector3.forward);
			Quaternion newRot = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime*5);
			//string.Format("{0} - {1}", Input.mousePosition, Camera.main.WorldToScreenPoint(transform.position));

			//if (angleToRotate < 10)
			//	rb.AddTorque(-1*Time.deltaTime);
			//else if(angleToRotate > 10)
			//	rb.AddTorque(1*Time.deltaTime);

			//Debug.Log( string.Format("{0}", angleToRotate  ));
			transform.rotation = newRot;//Quaternion.Euler(0, 0, tAngle);

            //}

            base.Update();
        }

        protected override void OutofboundsY(bool upper)
        {
            //throw new System.NotImplementedException();
            if (upper)
                controller.WarnPlayer(WarningType.UpperLimit);
            else
                controller.WarnPlayer(WarningType.LowerLimit);
        }

        protected override void HandleWarped(Vector3 warp)
        {
            controller.OnPlayerWarp(warp);
        }

        protected override void HandleEscaped()
        {
            throw new System.NotImplementedException();
        }
    }
}