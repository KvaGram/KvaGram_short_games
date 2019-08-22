using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KvaGames.Asteroids
{
	public class Game : MonoBehaviour
	{
		[SerializeField] new //hides obsolete legacy inhertience
		private Transform camera;
        [SerializeField]
        private Transform ghostCamera;
        [SerializeField]
		private float CameraFollowSpeed = 5;
		[SerializeField]
		private Player player;


		[SerializeField]
		private Asteroid[] asteroidPrefabs;
		[SerializeField]
		private Rect playarea;
		[SerializeField]
		private AsteroidLogicData[] AsteroidLogicTable;

        public Rect Playarea { get => playarea;}

        private void Awake()
		{
			camera = camera ?? GetComponentInChildren<Camera>().transform.parent;
            //TODO: find some way to look for camera and ghost camera seperatly.
			player = player ?? GetComponentInChildren<Player>();
		}

		private void Start()
		{
			EventHandeler.AsteroidSpawn += OnAsteroidSpawn;
			EventHandeler.AsteroidDamage += OnAsteroidDamage;

			SpawnAsteroid(12);
		}
		private void OnDisable()
		{
			EventHandeler.AsteroidSpawn -= OnAsteroidSpawn;
			EventHandeler.AsteroidDamage -= OnAsteroidDamage;
		}
		private void Update()
		{
			//Vector3 ctop = player.transform.position - camera.position;
			if(player)//(ctop.magnitude > 10)
			{
				//ctop = ctop.normalized;
				camera.position = Vector3.Lerp(camera.position, player.transform.position, Time.deltaTime* CameraFollowSpeed);
                UpdateCamGhost();

            }
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
					int tries = 0;
					do
					{
						loc.x = Random.Range(0f, playarea.width) + playarea.x;
						loc.y = Random.Range(0f, playarea.height) + playarea.y;
						tries++;
					} while (Vector3.Distance(loc, player.transform.position) < 100 && tries < 20);
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
				if (AsteroidLogicTable.Any(d => d.AsteroidSize == asteroid.Size))
				{
					AsteroidLogicData data = AsteroidLogicTable.First(d => d.AsteroidSize == asteroid.Size);
					byte[] newAsteroids = data.List;
					foreach (byte n in newAsteroids)
					{
						SpawnAsteroid(n, source);
					}
				}
			}

			//sound effects?
		}

        internal void OnPlayerWarp(Vector3 warp)
        {
            camera.position += warp;
            UpdateCamGhost();
        }

        private void UpdateCamGhost()
        {
            if (ghostCamera == null)
                return;

            float playerx = player.transform.position.x;
            float lowerXDist = Mathf.Abs( Playarea.x - playerx); 
            float upperXDist = Mathf.Abs(Playarea.width + Playarea.x - playerx);

            if(lowerXDist < upperXDist)
            {
                ghostCamera.position = camera.position + playarea.width * Vector3.right;
            }
            else
            {
                ghostCamera.position = camera.position + playarea.width * Vector3.left;
            }
            
        }
    }
}