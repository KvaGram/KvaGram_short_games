using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KvaGames.Hex;
using System.Linq;
using UnityEngine.SceneManagement;

namespace KvaGames.Snakeclone
{
	public class Player : Worm
	{
		[SerializeField]
		private HexGridHexagon map;

		[SerializeField]
		private Target targetPrefab;
		[SerializeField]
		private Worm tailPrefab;



		[SerializeField]
		private KeyCode
		moveUp = KeyCode.W,
		moveUpLeft = KeyCode.Q,
		moveDownLeft = KeyCode.A,
		moveDown = KeyCode.S,
		moveDownRight = KeyCode.D,
		moveUpRight = KeyCode.E,
		rotateLeft = KeyCode.LeftArrow,
		rotateRight = KeyCode.RightArrow;

		[SerializeField]
		Target target;
		[SerializeField]
		private List<Worm> tail;

		[SerializeField]
		private float tickLength = 0.5f;
		private float tickTime = 0;
		private uint tickCount = 0;

		public float TickLength	{get{ return tickLength; } }
		public float TickTime	{get{ return tickTime; } }
		public uint	TickCount	{get{ return tickCount; } }

//		[SerializeField]
//		private PlayerHit playerHit;
		
		//readable version of playerHit!
		//public string HitStatus
		//{
		//	get
		//	{
		//		string ret = "";
		//		switch (playerHit)
		//		{
		//			case PlayerHit.Safe:
		//				ret = "safe";
		//				break;
		//			case PlayerHit.Target:
		//				ret = "safe, and hits the target!";
		//				break;
		//			case PlayerHit.Wall:
		//				ret = "DEADLY, will hit a wall!";
		//				break;
		//		}
		//		return ret;
		//	}
		//}

		void Awake()
		{
			
		}
		// Use this for initialization
		void Start( )
		{
			CurrentTile = map.TileZero;
			tail = new List<Worm>();
			SetTarget();
		}

		// Update is called once per frame
		void Update( )
		{
			//todo: implment hex direction

			//if		(Input.GetKeyDown(moveUp))
			//{
			//	transform.localRotation = Quaternion.Euler(0, 0, 0);
			//}
			//else if (Input.GetKeyDown(moveLeft))
			//{
			//	transform.localRotation = Quaternion.Euler(0, 270, 0);
			//}
			//else if (Input.GetKeyDown(moveRight))
			//{
			//	transform.localRotation = Quaternion.Euler(0, 90, 0);
			//}
			//else if (Input.GetKeyDown(moveDown))
			//{
			//	transform.localRotation = Quaternion.Euler(0, 180, 0);
			//}
			//TODO: move this to a game controller
			tickTime += Time.deltaTime;
			if(tickTime >= tickLength)
			{
				tickTime -= tickLength;
				GameTick();
			}

			if      (Input.GetKeyDown(moveUp))
			{
				Dir = HexDirectionFlat.UP;
			}
			else if (Input.GetKeyDown(moveUpLeft))
			{
				Dir = HexDirectionFlat.UP_LEFT;
			}
			else if (Input.GetKeyDown(moveDownLeft))
			{
				Dir = HexDirectionFlat.DOWN_LEFT;
			}
			else if (Input.GetKeyDown(moveDown))
			{
				Dir = HexDirectionFlat.DOWN;
			}
			else if (Input.GetKeyDown(moveDownRight))
			{
				Dir = HexDirectionFlat.DOWN_RIGHT;
			}
			else if (Input.GetKeyDown(moveUpRight))
			{
				Dir = HexDirectionFlat.UP_RIGHT;
			}

			else if(Input.GetKeyDown(rotateLeft))
			{
				int i = (int)Dir;
				i++;
				while (i >= 6) i -= 6;
				Dir = (HexDirectionFlat)i;
			}
			else if (Input.GetKeyDown(rotateRight))
			{
				int i = (int)Dir;
				i--;
				while (i < 0) i += 6;
				Dir = (HexDirectionFlat)i;
			}

			//RaycastHit rayhit;
			//if(Physics.Raycast(transform.position, transform.forward, out rayhit, 1f))
			//{
			//	playerHit = PlayerHit.Wall;
			//}
			//else
			//{
			//	playerHit = PlayerHit.Safe;
			//}
		}
		bool IsColliding(HexTile tile)
		{
			foreach(Worm w in tail)
			{
				if (w.CurrentTile == tile)
					return true;
			}

			//todo see if collides with tail.
			return false;
		}
		bool IsTarget(HexTile tile)
		{
			//todo see if tile has target
			if (!target)
				return false;
			return target.CurrentTile == tile;
		}
		void ConsumeTarget()
		{
			Destroy(target.gameObject);
			target = null;
			AddTail();
		}
		void SetTarget()
		{
			//ahh.. the power of linq.
			//Someone should make a parody game of Legend of Zelda,
				//where the hero is named Linq, and all puzzles are about sorting and filtering lists.
			List<HexTile> validTiles = map.GetComponentsInChildren<HexTile>()
			                              .Where((arg1) => arg1.Terrain!=TerrainType.rock)
			                              .Where((arg1) => !IsColliding(arg1))
			                              .Where((arg1) => arg1 != CurrentTile)
			                              .ToList();
			if (validTiles.Count < 1)
			{
				Debug.Log("No place to add new target. Game won?");
				OnGameOver();
				return;
			}
			target = Instantiate(targetPrefab, transform.parent);
			HexTile tile = validTiles.ElementAt(Random.Range(0, validTiles.Count));
			target.CurrentTile = tile;

		}
		public void AddTail()
		{
			Worm w = Instantiate(tailPrefab, transform.parent);
			tail.Add(w);
			w.name = string.Format("Tail-segment #{0}", tail.Count);
			w.CurrentTile = CurrentTile;
		}

		void GameTick ()
		{
			HexTile nextTile = CurrentTile.Neighbour(Dir);
			if(!nextTile || nextTile.Terrain == TerrainType.rock || IsColliding(nextTile))
			{
				Debug.Log("GAME OVER!");
				OnGameOver();
				return;
			}

			for (int i = tail.Count - 1; i >= 0; i--)
			{
				Worm w = tail[i];
				Worm wn = (i==0) ? this : tail[i-1];
				w.Dir = wn.Dir;
				w.CurrentTile = wn.CurrentTile;
			}


			CurrentTile = nextTile;
			if (IsTarget(CurrentTile))
			{
				ConsumeTarget();
				SetTarget();
			}
			tickCount++;
		}
		public void OnGameOver()
		{
			SceneManager.LoadScene("Snake");
		}

	}
}