using System;
using UnityEngine;

namespace KvaGames.Hex
{
	[Serializable]
	public struct HexCoordAxial
	{
		public int q, r;
		public bool flat;

		public static implicit operator HexCoordCubic(HexCoordAxial a)
		{
			return new HexCoordCubic { q = a.q, r = a.r, s = (-a.q - a.r)};
		}

		public Vector3 ToVector3(bool flat)
		{
			if (flat)
				return new Vector3(3f / 2f * q, 0, Mathf.Sqrt(3) / 2f * q + Mathf.Sqrt(3) / 2f * r);
			else
				return new Vector3(Mathf.Sqrt(3) / 2f * q + Mathf.Sqrt(3) / 2f * r, 0, 3f / 2f * r);
		}
	}
}
