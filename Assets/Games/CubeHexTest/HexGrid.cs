using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KvaGames.Hex
{
	//This class builds a small hex-shaped map of hex-tiles radiating out from a center tile.
	//Can be inherited to create different shapes of different sizes.
	public class HexGrid : MonoBehaviour
	{
		[SerializeField]
		private HexTile tileZero;
		public HexTile TilePrefab;
		public bool flathead;
		protected Dictionary<HexCoordAxial, HexTile> tileMap = new Dictionary<HexCoordAxial, HexTile>();
		public Dictionary<HexCoordAxial, HexTile> TileMap
		{
			get
			{
				return tileMap;
			}
		}

		public HexTile TileZero
		{
			get
			{
				return tileZero;
			}
		}

		public virtual void BuildMap()
		{
			CleanMap();
			BuildOrGetTile(HexCoordCubic.ZERO, out tileZero);


			BuildMap2(tileZero);
		}
		protected virtual void BuildMap2(HexTile t)
		{
			for (int i = 0; i < t.neighbours.Length; i++)
			{
				HexCoordCubic n = HexCoordCubic.Neighbour(t.HexCoord, i);
				HexTile tn;
				if (!t.neighbours[i])
				{
					if(BuildOrGetTile(n, out tn))
					{
						int j = (i + 3) % 6;
						t.neighbours[i] = tn;
						tn.neighbours[j] = t;
						BuildMap2(tn);
					}
				}
			}
		}
		protected virtual void CleanMap()
		{
			List<HexTile> childeren = GetComponentsInChildren<HexTile>().ToList();
			foreach (HexTile child in childeren)
			{
				if (Application.isEditor)
					DestroyImmediate(child.gameObject);
				else
					Destroy(child.gameObject);
			}
			tileMap = new Dictionary<HexCoordAxial, HexTile>();
		}

		public virtual bool IsValidInMap(HexCoordCubic pos)
		{
			return pos.Ring <= 3;
		}

		public bool BuildOrGetTile(HexCoordCubic pos, out HexTile tile)
		{
			if(tileMap.TryGetValue(pos, out tile))
			{
				return true;
			}
			if (IsValidInMap(pos))
			{
				tile = Instantiate(TilePrefab, transform);
				tile.Setup(pos, flathead);
				tileMap.Add(pos, tile);
				OnTileBuilt(tile);
				return true;
			}
			return false;
		}

		public virtual void OnTileBuilt(HexTile tile)
		{
			tile.Terrain = TerrainType.rock;

		}
	}
}
