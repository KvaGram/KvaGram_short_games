using UnityEngine;
namespace KvaGames.Asteroids
{
	[RequireComponent(typeof(Rigidbody))]
	public class Player : MonoBehaviour
	{
		[SerializeField]
		private int health;
		public int Health { get { return health; }}

		[SerializeField]
		private Bullet bulletPrefab;
		[SerializeField]
		private float maxSpeed = 50;
		private Rigidbody rb;
		private void Awake( )
		{
			rb = rb ?? GetComponent<Rigidbody>();
		}
		void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.tag == "Enemy")
			{
				health--;
				if (health <= 0)
				{
					Debug.Log("GAME OVER");
					Destroy(gameObject, 1);
				}
			}
		}

		private Bullet[] bullets = new Bullet[100];
		private int bulletIndex = 0;

		[SerializeField]
		private float bulletCooldown = 0.2f;
		private float bulletCooldownCounter = 0;

		private void Update( )
		{
			//ensure z is always 0.
			Vector3 pos = transform.position;
			pos.z = 0;
			transform.position = pos;


			bulletCooldownCounter += Time.deltaTime;

			if(rb.velocity.sqrMagnitude > maxSpeed*maxSpeed)
				rb.velocity = Vector3.ClampMagnitude(rb.velocity, 50);
			if(Input.GetMouseButton(1))
			{
				Vector3 boostDir = transform.right;
				boostDir.z = 0;
				rb.AddForce(boostDir.normalized*10f, ForceMode.Acceleration);
			}
			if(Input.GetMouseButton(0) && bulletCooldownCounter > bulletCooldown)
			{
				Bullet b = Instantiate(bulletPrefab);
				b.transform.position = transform.position + transform.right * 1.3f;
				b.Rb.velocity = rb.velocity + transform.right* 60;

				//if (bullets[bulletIndex])
				//	Destroy(bullets[bulletIndex].gameObject);
				//else
				//	bullets[bulletIndex] = Instantiate(bulletPrefab);
				
				//bullets[bulletIndex].transform.position = transform.position + transform.right * 1.3f;
				//bullets[bulletIndex].Rb.velocity = rb.velocity + transform.right* 60;
				//bulletIndex++;
				//if (bulletIndex >= 200)
					//bulletIndex = 0;
				//Destroy(b.gameObject, 120);
			}
			if(Input.GetMouseButton(2))
			{
				//FIRE TORPEDO!!
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
		}
	}
}