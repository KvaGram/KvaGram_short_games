using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KvaGames.Snakeclone
{
	public class Game : MonoBehaviour
	{
		public Rect tileSize = new Rect(0,0,1,1);
		public Rect GameSize = new Rect(0,0,32,32);

		private Player player;
		

		private void Awake( )
		{
			player = GetComponentInChildren<Player>();
		}
		// Use this for initialization
		void Start( )
		{
			
		}

		// Update is called once per frame
		void Update( )
		{

		}

	}

	/*
	public struct Tile
	{
		int x, z;
		TileTerrain terrain;
	}
	public enum TileTerrain
	{
		Ground, wall
	}
	*/
}