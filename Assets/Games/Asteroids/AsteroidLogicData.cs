using UnityEngine;
namespace KvaGames.Asteroids
{
	//when an asteroid is destroyed, it may divide into smaller asteroids.
	//These structs explains what size of asteroids devide into what.
	//unlisted asteroids simply disappear.
	[CreateAssetMenu(fileName = "AsteroidData", menuName = "Games/Asteroids")]
	class AsteroidLogicData : ScriptableObject
	{
		[SerializeField]
		private byte asteroidSize; //What size of asteroid this entry is for.
		public byte AsteroidSize { get { return asteroidSize; } }
		[SerializeField]
		private AsteroidDivisionChance[] list; //The possible outcomes from blowing up this size asteroid. if left empty, it simply disappears.
		public byte[] List { get { return GetRandomList(); }}
		private int combinedchance; //Used as a param to generate random number. should be a round number, but don't need be.



		void OnValidate()
		{

			//reset combinedchance
			combinedchance = 0;
			foreach (AsteroidDivisionChance c in list)
				combinedchance += c.chance;
		}
		private byte[] GetRandomList()
		{
			int r = Random.Range(0, combinedchance);
			int s = 0;
			foreach (AsteroidDivisionChance c in list)
			{
				s += c.chance;
				if (s >= r)
					return c.result;
			}
			return new byte[0];
		}


		[System.Serializable]
		private struct AsteroidDivisionChance
		{
			public int chance; //Not all outcomes are equal. Higher value mean more likely to happen.
			public byte[] result; //The number of asteroids that comes from this, and how big they are. Can be left empty.
		}
	}
}