using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KvaGames.Hex
{
	public class HexGridHexagon : HexGrid
	{
		public int size;
		public override bool IsValidInMap(HexCoordCubic pos)
		{
			return pos.Ring <= size;
		}
		public override void OnTileBuilt(HexTile tile)
		{
			if (tile.HexCoord.Ring == size)
			{
				tile.Terrain = TerrainType.rock;
				tile.transform.localScale = new Vector3(1, 1.8f, 1);
			}
			
		}
	}
}
