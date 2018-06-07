using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KvaGames.Hex;
namespace KvaGames.Snakeclone
{
	public class Worm : MonoBehaviour
	{
		private HexTile currentTile;
		private HexDirectionFlat dir;
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
	}
}
