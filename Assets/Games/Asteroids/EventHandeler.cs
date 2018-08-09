using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KvaGames.Asteroids
{
	class EventHandeler : MonoBehaviour
	{
		#region GAME singleton
		private static EventHandeler _EVENT;
		public static EventHandeler EVENT
		{
			get
			{
				if (!_EVENT) //Should be set in Awake, but just in case:
					_EVENT = FindObjectOfType<EventHandeler>();
				return _EVENT;
			}
		}
		#endregion

		private void Awake( )
		{
			if(!_EVENT) //sets up the singleton.
				_EVENT = this;
		}

		public delegate void AsteroidEventHandler(Asteroid asteroid);
		public static event AsteroidEventHandler AsteroidDamage;
		public static event AsteroidEventHandler AsteroidSpawn;

		public static void TriggerAsteroidDamage(Asteroid asteroid)
		{
			if (AsteroidDamage != null)
				AsteroidDamage(asteroid);
		}
		public static void TriggerAsteroidSpawn(Asteroid asteroid)
		{
			if (AsteroidSpawn != null)
				AsteroidSpawn(asteroid);

		}
	}
}
