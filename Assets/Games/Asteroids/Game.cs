using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace KvaGames.Asteroids
{
	public class Game : MonoBehaviour
	{
		[SerializeField]
		private Player player;
		[SerializeField]
		private Asteroid[] asteroidPrefabs;
		[SerializeField]
		private Rect playarea;
		[SerializeField]
		private AsteroidDivisionList[] AsteroidDivisionTable;

		private void Awake( )
		{
			player = player ?? GetComponentInChildren<Player>();
		}

		private void Start( )
		{
			EventHandeler.AsteroidSpawn  += OnAsteroidSpawn;
			EventHandeler.AsteroidDamage += OnAsteroidDamage;
		}
		private void OnDisable( )
		{
			EventHandeler.AsteroidSpawn  -= OnAsteroidSpawn;
			EventHandeler.AsteroidDamage -= OnAsteroidDamage;
		}
		public void SpawnAsteroid(byte size, Vector3? source = null)
		{
			if (size == 0)
			{
				Debug.LogWarning("Asteroid of size 0 somehow requested. Ignoring.");
			}
			if (asteroidPrefabs.Any(a => a.Size == size))
			{
				Vector3 loc = new Vector3();
				if (source == null)
				{
					do
					{
						loc.x = Random.Range(0f, playarea.width) + playarea.x;
						loc.y = Random.Range(0f, playarea.height) + playarea.y;
					} while (Vector3.Distance(loc, player.transform.position) < 100);
				}
				else
				{
					loc = source.Value;
				}

				List<Asteroid> list = asteroidPrefabs.Where(a => a.Size == size).ToList();
				Asteroid ast = Instantiate(list[Random.Range(0, list.Count)], loc, new Quaternion(), this.transform);
			}
			else
			{
				Debug.LogWarning("No asteroid of size " + size + "! Requesting smaller");
				SpawnAsteroid(size--);
			}


		}

		private void OnAsteroidSpawn(Asteroid asteroid)
		{
			//Warn player?
		}
		private void OnAsteroidDamage(Asteroid asteroid)
		{
			if (asteroid.Health <= 0)
			{
				Vector3 source = asteroid.transform.position;
				Destroy(asteroid.gameObject);
				if(AsteroidDivisionTable.Any(d => d.size == asteroid.Size))
				{
					int c;
					AsteroidDivisionList list = AsteroidDivisionTable.Where(d => d.size == asteroid.Size).First();
					foreach(AsteroidDivisionChance chance in list.list)
					{

					}
				}
			}
			
			//sound effects?
		}

		//when an asteroid is destroyed, it may divide into smaller asteroids.
		//These structs explains what size of asteroids devide into what.
		//unlisted asteroids simply disappear.

		[System.Serializable]
		struct AsteroidDivisionList
		{
			public byte size; //What size of asteroid this entry is for.
			public AsteroidDivisionChance[] list; //The possible outcomes from blowing up this size asteroid. if left empty, it simply disappears.
		}
		[System.Serializable]
		struct AsteroidDivisionChance
		{
			public short weight; //Not all outcomes are equal. Higher weight mean more likely to happen.
			public byte[] result; //The number of asteroids that comes from this, and how big they are. Can be left empty.
		}
	}
	
}