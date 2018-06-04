using UnityEngine;

namespace KvaGames.Hex
{
	//TODO: move to seperate file
	public class HexTile : MonoBehaviour
	{
		[SerializeField]
		protected HexCoord hexCoord;
		protected CenterHexTile center;

		public void setupCoords(HexCoord hexCoord, CenterHexTile center)
		{
			this.hexCoord = hexCoord;
			this.center = center;

			//transform.localPosition =
		}


		public readonly HexTile[] neighbours;

		public CenterHexTile Center
		{
			get { return center; }
		}
		public HexCoord HexCoord
		{
			get {return hexCoord;}
			set
			{
				if (value.Valid)
					hexCoord=value;
				else
					Debug.LogError("Invalid HexCoord value detected! Ignoring.", this);
			}
		}
	}
}