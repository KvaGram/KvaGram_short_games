using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KvaGames.Hex
{
	//todo: rename
	public class CenterHexTile : HexTile
	{
		public int radius = 3;
		public bool flatHead = true; //todo: add support for tophead. Fornow: all code assumes flathead.
		public List<List<HexTile>> ringList;

		public GameObject grassTilePrefab;
		public GameObject rockTilePrefab;

		//creates or updates map whenever radius has been changed!
		public void BuildMap()
		{
			ringList = ringList ?? new List<List<HexTile>>();

			for (int i = 0; i < radius; i++)
			{

			}
		}
		public void CleanMap()
		{
			ringList = new List<List<HexTile>>();
			HexTile[] childeren = GetComponentsInChildren<HexTile>();
			for(int i = childeren.Length-1; i >= 0; i--)
			{
				HexTile child = childeren[i];
				Destroy(child); //yummy yummy murder...
			}
		}

		// Use this for initialization
		void Start( )
		{

		}

		// Update is called once per frame
		void Update( ) {

		}
	}
	//TODO: move to seperate file
	public class HexTile : MonoBehaviour
	{
		public readonly HexTile[] neighbours;


		
	}

	public struct HexCoord : IEquatable<HexCoord>
	{
		int x, y, z;
		bool Valid( )
		{
			return (x + y + z == 0);
		}

		static HexCoord Neighbour(HexCoord origin, HexDirectionFlat dir)
		{
			switch(dir)
			{
				case HexDirectionFlat.UP:
					return origin + FLAT_UP;
				case HexDirectionFlat.UP_LEFT:
					return origin + FLAT_UP_LEFT;
				case HexDirectionFlat.DOWN_LEFT:
					return origin + FLAT_DOWN_LEFT;
				case HexDirectionFlat.DOWN:
					return origin + FLAT_DOWN;
				case HexDirectionFlat.DOWN_RIGHT:
					return origin + FLAT_DOWN_RIGHT;
				case HexDirectionFlat.UP_RIGHT:
					return origin + FLAT_UP_RIGHT;
			}
			//default - should never happen
			return (origin);
		}
		static HexCoord Neighbour (HexCoord origin, HexDirectionPointy dir)
		{
			HexDirectionFlat flatConvert = (HexDirectionFlat)dir;
			return (Neighbour(origin, flatConvert));
		}
		#region constants
		static readonly HexCoord FLAT_UP		= new HexCoord { x = 0, y = 1, z = -1 };
		static readonly HexCoord FLAT_UP_LEFT	= new HexCoord { x = -1, y = 1, z = 0 };
		static readonly HexCoord FLAT_DOWN_LEFT	= new HexCoord { x = -1, y = 0, z = 1 };
		static readonly HexCoord FLAT_DOWN		= new HexCoord { x = 0, y = -1, z = 1 };
		static readonly HexCoord FLAT_DOWN_RIGHT= new HexCoord { x = 1, y = -1, z = 0 };
		static readonly HexCoord FLAT_UP_RIGHT	= new HexCoord { x = 1, y = 0, z = -1 };

		static readonly HexCoord POINTY_LEFT_UP		= FLAT_UP;
		static readonly HexCoord POINTY_LEFT		= FLAT_UP_LEFT;
		static readonly HexCoord POINTY_LEFT_DOWN	= FLAT_DOWN_LEFT;
		static readonly HexCoord POINTY_RIGHT_DOWN	= FLAT_DOWN;
		static readonly HexCoord POINTY_RIGHT		= FLAT_DOWN_RIGHT;
		static readonly HexCoord POINTY_RIGHT_UP	= FLAT_UP_RIGHT;
		#endregion

		#region operators
		public static HexCoord operator -(HexCoord a, HexCoord b)
		{
			return new HexCoord { x = a.x - b.x, y = a.y - b.y, z = a.z - b.z };
		}

		public static HexCoord operator +(HexCoord a, HexCoord b)
		{
			return new HexCoord { x = a.x + b.x, y = a.y + b.y, z = a.z + b.z };
		}

		public override bool Equals(object obj)
		{
			return obj is HexCoord && Equals((HexCoord)obj);
		}

		public bool Equals(HexCoord other)
		{
			return x==other.x&&
				   y==other.y&&
				   z==other.z;
		}

		public static bool operator ==(HexCoord coord1, HexCoord coord2)
		{
			return coord1.Equals(coord2);
		}

		public static bool operator !=(HexCoord coord1, HexCoord coord2)
		{
			return !(coord1==coord2);
		}
		#endregion
	}
	public enum HexDirectionFlat
	{
		UP, UP_LEFT, DOWN_LEFT, DOWN, DOWN_RIGHT, UP_RIGHT
	}
	public enum HexDirectionPointy
	{
		LEFT_UP, LEFT, LEFT_DOWN, RIGHT_DOWN, RIGHT, RIGHT_UP
	}
}