using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KvaGames.Hex
{
	public class HexGridRect : HexGrid
	{
		[SerializeField]
		private Vector2Int xLength;
		[SerializeField]
		private Vector2Int zLength;

		public Vector2Int XLength
		{
			get
			{
				return xLength;
			}
			set
			{
				xLength = value;
				xLength.x = Mathf.Clamp(xLength.x, -1000, 0);
				xLength.y = Mathf.Clamp(xLength.y, 0, 1000);
			}
		}

		public Vector2Int ZLength
		{
			get
			{
				return zLength;
			}
			set
			{
				zLength = value;
				zLength.x = Mathf.Clamp(zLength.x, -1000, 0);
				zLength.y = Mathf.Clamp(zLength.y, 0, 1000);
			}
		}
		void OnValidate()
		{
			XLength = xLength;
			ZLength = zLength;
		}
		public override bool IsValidInMap(HexCoordCubic pos)
		{
			Vector3 p = pos.ToVector3(flathead);

			return p.x >= xLength.x && p.x <= xLength.y && p.z >= zLength.x && p.z <= zLength.y;
		}
		public override void OnTileBuilt(HexTile tile)
		{
			
		}
	}
}
