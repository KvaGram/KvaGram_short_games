using System;
using UnityEngine;

namespace KvaGames.Hex
{
	public struct HexCoord
	{
		int q, r, s;
		public bool Valid { get { return (q + r + s == 0); } }
		public int Ring { get { return Mathf.Max(Mathf.Abs(q), Mathf.Abs(r), Mathf.Abs(s)); } }
		#region get neighbour
		public static HexCoord Neighbour(HexCoord origin, HexDirectionFlat dir)
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
		public static HexCoord Neighbour (HexCoord origin, HexDirectionPointy dir)
		{
			HexDirectionFlat flatConvert = (HexDirectionFlat)dir;
			return (Neighbour(origin, flatConvert));
		}
		public static HexCoord Neighbour(HexCoord origin, int dir)
		{
			HexDirectionFlat flatConvert = (HexDirectionFlat)dir;
			return (Neighbour(origin, flatConvert));
		}
		#endregion
		#region constants
		public static readonly HexCoord FLAT_UP			= new HexCoord { q = 0, r = 1, s = -1 };
		public static readonly HexCoord FLAT_UP_LEFT	= new HexCoord { q = -1, r = 1, s = 0 };
		public static readonly HexCoord FLAT_DOWN_LEFT	= new HexCoord { q = -1, r = 0, s = 1 };
		public static readonly HexCoord FLAT_DOWN		= new HexCoord { q = 0, r = -1, s = 1 };
		public static readonly HexCoord FLAT_DOWN_RIGHT	= new HexCoord { q = 1, r = -1, s = 0 };
		public static readonly HexCoord FLAT_UP_RIGHT	= new HexCoord { q = 1, r = 0, s = -1 };

		public static readonly HexCoord POINTY_LEFT_UP		= FLAT_UP;
		public static readonly HexCoord POINTY_LEFT			= FLAT_UP_LEFT;
		public static readonly HexCoord POINTY_LEFT_DOWN	= FLAT_DOWN_LEFT;
		public static readonly HexCoord POINTY_RIGHT_DOWN	= FLAT_DOWN;
		public static readonly HexCoord POINTY_RIGHT		= FLAT_DOWN_RIGHT;
		public static readonly HexCoord POINTY_RIGHT_UP		= FLAT_UP_RIGHT;
		#endregion
		#region operators
		public static HexCoord operator -(HexCoord a, HexCoord b)
		{
			return new HexCoord { q = a.q - b.q, r = a.r - b.r, s = a.s - b.s };
		}

		public static HexCoord operator +(HexCoord a, HexCoord b)
		{
			return new HexCoord { q = a.q + b.q, r = a.r + b.r, s = a.s + b.s };
		}

		public static HexCoord operator *(HexCoord a, int s)
		{
			return new HexCoord { q = a.q*s, r = a.r*s, s = a.s*s };
		}

		public static implicit operator Vector3(HexCoord a)
		{
			float x = Mathf.Sqrt(3) * (a.s/2 + a.q);
			float y = 0;
			float z = 0;
			return new Vector3(x, y, z);
		}
		#endregion
	}
}