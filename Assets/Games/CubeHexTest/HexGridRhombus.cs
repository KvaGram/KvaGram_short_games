using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KvaGames.Hex
{
	public class HexGridRhombus : HexGrid
	{
		[SerializeField]
		private Vector2Int qLength;
		[SerializeField]
		private Vector2Int rLength;

		public Vector2Int QLength
		{
			get
			{
				return qLength;
			}
			set
			{
				qLength = value;
				qLength.x = Mathf.Clamp(qLength.x, -1000, 0);
				qLength.y = Mathf.Clamp(qLength.y, 0, 1000);
			}
		}

		public Vector2Int RLength
		{
			get
			{
				return rLength;
			}
			set
			{
				rLength = value;
				rLength.x = Mathf.Clamp(rLength.x, -1000, 0);
				rLength.y = Mathf.Clamp(rLength.y, 0, 1000);
			}
		}

		void OnValidate()
		{
			QLength = qLength;
			RLength = rLength;
		}
		public override bool IsValidInMap(HexCoordCubic pos)
		{
			return pos.q >= qLength.x && pos.q <= qLength.y && pos.r >= rLength.x && pos.r <= rLength.y;
		}
		public override void OnTileBuilt(HexTile tile)
		{
			HexCoordCubic pos = tile.HexCoord;
			if (  Mathf.Approximately(pos.q, qLength.x)
			   || Mathf.Approximately(pos.q, qLength.y)
			   || Mathf.Approximately(pos.r, rLength.x)
			   || Mathf.Approximately(pos.r, rLength.y)
			  )
			{
				tile.Terrain = TerrainType.rock;
			}
		}


	}
}
