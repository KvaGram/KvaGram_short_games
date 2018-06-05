using System;
using UnityEngine;

namespace KvaGames.Hex
{
	[Serializable]
	public struct HexCoordCubic
	{
		public int q, r, s;
		public bool Valid { get { return (q + r + s == 0); } }
		public int Ring { get { return Mathf.Max(Mathf.Abs(q), Mathf.Abs(r), Mathf.Abs(s)); } }
		#region get neighbour
		public static HexCoordCubic Neighbour(HexCoordCubic origin, HexDirectionFlat dir)
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
		public static HexCoordCubic Neighbour (HexCoordCubic origin, HexDirectionPointy dir)
		{
			HexDirectionFlat flatConvert = (HexDirectionFlat)dir;
			return (Neighbour(origin, flatConvert));
		}
		public static HexCoordCubic Neighbour(HexCoordCubic origin, int dir)
		{
			HexDirectionFlat flatConvert = (HexDirectionFlat)dir;
			return (Neighbour(origin, flatConvert));
		}
		#endregion
		#region constants
		private static readonly HexCoordCubic[] directions =
		{
			new HexCoordCubic { q = 0, r = 1, s = -1 },
			new HexCoordCubic { q = -1, r = 1, s = 0 },
			new HexCoordCubic { q = -1, r = 0, s = 1 },
			new HexCoordCubic { q = 0, r = -1, s = 1 },
			new HexCoordCubic { q = 1, r = -1, s = 0 },
			new HexCoordCubic { q = 1, r = 0, s = -1 }
		};

		public static HexCoordCubic ZERO = new HexCoordCubic { q = 0, r = 0, s = 0 };

		public static HexCoordCubic FLAT_UP 			{ get { return directions[0]; } }
		public static HexCoordCubic FLAT_UP_LEFT		{ get { return directions[1]; } }
		public static HexCoordCubic FLAT_DOWN_LEFT		{ get { return directions[2]; } }
		public static HexCoordCubic FLAT_DOWN			{ get { return directions[3]; } }
		public static HexCoordCubic FLAT_DOWN_RIGHT		{ get { return directions[4]; } }
		public static HexCoordCubic FLAT_UP_RIGHT		{ get { return directions[5]; } }

		public static HexCoordCubic POINTY_LEFT_UP		{ get { return directions[0]; } }
		public static HexCoordCubic POINTY_LEFT			{ get { return directions[1]; } }
		public static HexCoordCubic POINTY_LEFT_DOWN	{ get { return directions[2]; } }
		public static HexCoordCubic POINTY_RIGHT_DOWN	{ get { return directions[3]; } }
		public static HexCoordCubic POINTY_RIGHT		{ get { return directions[4]; } }
		public static HexCoordCubic POINTY_RIGHT_UP		{ get { return directions[5]; } }
		#endregion
		#region operators
		public static HexCoordCubic operator -(HexCoordCubic a, HexCoordCubic b)
		{
			return new HexCoordCubic { q = a.q - b.q, r = a.r - b.r, s = a.s - b.s};
		}

		public static HexCoordCubic operator +(HexCoordCubic a, HexCoordCubic b)
		{
			return new HexCoordCubic { q = a.q + b.q, r = a.r + b.r, s = a.s + b.s};
		}

		public static HexCoordCubic operator *(HexCoordCubic a, int s)
		{
			return new HexCoordCubic { q = a.q*s, r = a.r*s, s = a.s*s};
		}
		public static implicit operator HexCoordAxial (HexCoordCubic a)
		{
			return new HexCoordAxial { q = a.q, r = a.r};
		}
		#endregion
		public Vector3 ToVector3(bool flat)
		{
			float threeroot = Mathf.Sqrt(3);
			float threehalfs = 3f / 2f;
			float threeroothalfs = threeroot / 2f;

			if (flat)
				return new Vector3(threehalfs * q, 0, threeroothalfs * q + threeroot * r);
			else
				return new Vector3(threeroot * q + threeroothalfs * r, 0, threehalfs * r);
		}
	}
}