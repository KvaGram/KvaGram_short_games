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
		private short health;



	}
}