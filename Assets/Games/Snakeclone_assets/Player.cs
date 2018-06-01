using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KvaGames.Snakeclone
{
	public class Player : MonoBehaviour
	{
		[SerializeField]
		private KeyCode
		moveUp = KeyCode.UpArrow,
		moveLeft = KeyCode.LeftArrow,
		moveRight = KeyCode.RightArrow,
		moveDown = KeyCode.DownArrow;

		[SerializeField]
		private float tickLength = 0.5f;
		private float tickTime = 0;
		private uint tickCount = 0;

		public float TickLength	{get{ return tickLength; } }
		public float TickTime	{get{ return tickTime; } }
		public uint	TickCount	{get{ return tickCount; } }

		[SerializeField]
		private PlayerHit playerHit;
		
		//readable version of playerHit!
		public string HitStatus
		{
			get
			{
				string ret = "";
				switch (playerHit)
				{
					case PlayerHit.Safe:
						ret = "safe";
						break;
					case PlayerHit.Target:
						ret = "safe, and hits the target!";
						break;
					case PlayerHit.Wall:
						ret = "DEADLY, will hit a wall!";
						break;
				}
				return ret;
			}
		}
		

		// Use this for initialization
		void Start( )
		{

		}

		// Update is called once per frame
		void Update( )
		{
			if		(Input.GetKeyDown(moveUp))
			{
				transform.localRotation = Quaternion.Euler(0, 0, 0);
			}
			else if (Input.GetKeyDown(moveLeft))
			{
				transform.localRotation = Quaternion.Euler(0, 270, 0);
			}
			else if (Input.GetKeyDown(moveRight))
			{
				transform.localRotation = Quaternion.Euler(0, 90, 0);
			}
			else if (Input.GetKeyDown(moveDown))
			{
				transform.localRotation = Quaternion.Euler(0, 180, 0);
			}
			//TODO: move this to a game controller
			tickTime += Time.deltaTime;
			if(tickTime >= tickLength)
			{
				tickTime -= tickLength;
				GameTick();
			}
			RaycastHit rayhit;
			if(Physics.Raycast(transform.position, transform.forward, out rayhit, 1f))
			{
				playerHit = PlayerHit.Wall;
			}
			else
			{
				playerHit = PlayerHit.Safe;
			}
		}

		void GameTick ()
		{
			if(playerHit == PlayerHit.Wall)
			{
				Debug.Log("GAME OVER!");
				return;
			}
			transform.position += transform.forward;
			tickCount++;
		}
	}
	public enum PlayerHit
	{
		Safe, Target, Wall
	}
}