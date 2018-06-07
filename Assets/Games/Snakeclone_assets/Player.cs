using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KvaGames.Hex;
namespace KvaGames.Snakeclone
{
	public class Player : MonoBehaviour
	{
		private HexTile currentTile;
		private HexDirectionFlat dir;

		[SerializeField]
		private HexGridHexagon map;

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

		public HexTile CurrentTile
		{
			get
			{
				return currentTile;
			}

			set
			{
				currentTile = value;
				transform.position = value.transform.position;
			}
		}

		public HexDirectionFlat Dir
		{
			get
			{
				return dir;
			}

			set
			{
				
				dir = value;
				transform.localRotation = Quaternion.Euler(0, -60 * (int)dir, 0);
			}
		}

		void Awake()
		{
			
		}
		// Use this for initialization
		void Start( )
		{
			currentTile = map.TileZero;
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
				int i = (int)dir;
				i++;
				while (i >= 6) i -= 6;
				Dir = (HexDirectionFlat)i;
			}
			else if (Input.GetKeyDown(rotateRight))
			{
				int i = (int)dir;
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
			//todo see if collides with tail.
			return false;
		}
		bool IsTarget(HexTile tile)
		{
			//todo see if tile has target
			return false;
		}
		void ConsumeTarget()
		{
			//todo add target to tail
		}
		void SetTarget()
		{
			//todo add new target on map
		}

		void GameTick ()
		{
			HexTile nextTile = CurrentTile.Neighbour(dir);
			if(!nextTile || nextTile.Terrain == TerrainType.rock || IsColliding(nextTile))
			{
				Debug.Log("GAME OVER!");
				return;
			}
			CurrentTile = nextTile;
			if (IsTarget(CurrentTile))
			{
				ConsumeTarget();
				SetTarget();
			}
			tickCount++;
		}
	}
}