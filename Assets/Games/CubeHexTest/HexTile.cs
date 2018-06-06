using UnityEngine;
using System.Linq;
namespace KvaGames.Hex
{
	//TODO: move to seperate file
	public class HexTile : MonoBehaviour
	{
		public TerrainType terrain;
		public Material[] terrainMaterials;
		private bool flathead;
		public HexTile[] neighbours;

		[SerializeField]
		protected HexCoordCubic hexCoord;
		//protected HexTile center;

		public void setup(HexCoordCubic hexCoord, bool flathead)
		{
			//Debug.Log(string.Format("Setting up hex tile at q {0} r {1} s {2}", hexCoord.q, hexCoord.r, hexCoord.s));
			//this.center = center;
			HexCoord = hexCoord;
			this.flathead = flathead;
			neighbours = new HexTile[6];
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
		void OnValidate()
		{
			Material m = terrainMaterials.ElementAtOrDefault((int)terrain);
			if (!m)
				return;
			GetComponentInChildren<Renderer>().material = m;
		}
	}
	public enum TerrainType{test, grass, rock}
}