using UnityEngine;
using System.Linq;
using System;

namespace KvaGames.Hex
{
	//TODO: move to seperate file
	public class HexTile : MonoBehaviour
	{
		[SerializeField]
		private TerrainType terrain;
		public Material[] terrainMaterials;
		private bool flathead;
		public HexTile[] neighbours;
		private Renderer rend;

		[SerializeField]
		protected HexCoordCubic hexCoord;
		//protected HexTile center;

		public void Setup(HexCoordCubic hexCoord, bool flathead)
		{
			//Debug.Log(string.Format("Setting up hex tile at q {0} r {1} s {2}", hexCoord.q, hexCoord.r, hexCoord.s));
			//this.center = center;
			this.flathead = flathead;
			HexCoord = hexCoord;
			neighbours = new HexTile[6];

			rend = GetComponentInChildren<Renderer>();
			if (flathead)
				rend.transform.localRotation = Quaternion.Euler(-90, 30, 0);
			else
				rend.transform.localRotation = Quaternion.Euler(-90, 0, 0);
		}

		//public HexTile Center
		//{
		//	get { return center; }
		//}
		public HexCoordCubic HexCoord
		{
			get {return hexCoord;}
			set
			{
				if (value.Valid)
				{
					hexCoord = value;
					transform.localPosition = value.ToVector3(flathead);
				}
				else
					Debug.LogError("Invalid HexCoord value detected! Ignoring.", this);
			}
		}

		public TerrainType Terrain
		{
			get
			{
				return terrain;
			}

			set
			{
				terrain = value;
				Material m = terrainMaterials.ElementAtOrDefault((int)value);
				if (!m)
					return;
				GetComponentInChildren<Renderer>().material = m;
			}
		}

		void OnValidate()
		{
			Terrain = terrain;


		}

	}
	public enum TerrainType{test, grass, rock}
}