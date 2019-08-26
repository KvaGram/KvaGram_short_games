using System;
using UnityEngine;
using EventHandler = KvaGames.Asteroids.EventHandeler;

namespace KvaGames.Asteroids
{
    public class BulletEffect : MonoBehaviour
    {
        public GameObject PrefabExplosion;
        public int Buffersize = 100;
        private int index = 0;
        [SerializeField]
        private ParticleSystem[] explosions;
        private void Start()
        {
            explosions = new ParticleSystem[Buffersize];
            //EventHandler
            EventHandler.BulletHit += SendExplosion;
        }
        private void OnDisable()
        {
            EventHandler.BulletHit -= SendExplosion;
        }
        public void SendExplosion(Bullet bullet, Collision collision)
        {
            ParticleSystem ex;
            ex = explosions[index];
            if (ex == null)
            {
                ex = Instantiate(PrefabExplosion, transform).GetComponent<ParticleSystem>();
                explosions[index] = ex;
            }
            index = Mathf.Clamp(index + 1, 0, Buffersize-1);
            ex.transform.position = bullet.transform.position;
            ex.Clear();
            ex.Play();
        }

    }
}