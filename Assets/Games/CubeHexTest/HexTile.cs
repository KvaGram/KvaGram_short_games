using UnityEngine;
using System.Linq;
namespace KvaGames.Hex
{
	//TODO: move to seperate file
	public class HexTile : MonoBehaviour
	{
		public TerrainType terrain;
		public Material[] terrainMaterials;

		[SerializeField]
		protected HexCoordCubic hexCoord;
		protected CenterHexTile center;

		public void setupCoords(HexCoordCubic hexCoord, CenterHexTile center)
		{
			Debug.Log(string.Format("Setting up hex tile at q {0} r {1} s {2}", hexCoord.q, hexCoord.r, hexCoord.s));
			this.center = center;
			HexCoord = hexCoord;
		}


		public readonly HexTile[] neighbours;

		public CenterHexTile Center
		{
			get { return center; }
		}
		public HexCoordCubic HexCoord
		{
			get {return hexCoord;}
			set
			{
				if (value.Valid)
				{
					hexCoord = value;
					transform.localPosition = value.ToVector3(center.flatHead);
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