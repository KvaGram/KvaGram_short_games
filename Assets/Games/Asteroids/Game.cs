using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KvaGames.Asteroids
{
	public class Game : MonoBehaviour
	{
		private Player player;
		private void Awake( )
		{
			player = player ?? GetComponentInChildren<Player>();
		}

		private void Start( )
		{
			EventHandeler.AsteroidSpawn  += OnAsteroidSpawn;
			EventHandeler.AsteroidDamage += OnAsteroidDamage;

			
		}

		private void OnAsteroidSpawn(Asteroid asteroid)
		{
			throw new System.NotImplementedException();
		}
		private void OnAsteroidDamage(Asteroid asteroid)
		{
			throw new System.NotImplementedException();
		}
	}
}