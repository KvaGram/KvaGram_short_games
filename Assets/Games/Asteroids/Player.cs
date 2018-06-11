using UnityEngine;
namespace KvaGames.Asteroids
{
	[RequireComponent(typeof(Rigidbody))]
	public class Player : MonoBehaviour
	{
		private Rigidbody rb;
		private void Awake( )
		{
			rb = rb ?? GetComponent<Rigidbody>();
		}

		private void Update( )
		{
			if(Input.GetMouseButton(1))
			{
				rb.AddForce(transform.right, ForceMode.Acceleration);
			}
			if(Input.GetMouseButton(0))
			{
				//FIRE!!!!
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