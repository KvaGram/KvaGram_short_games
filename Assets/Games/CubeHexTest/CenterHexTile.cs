using System;
using System.Collections;
using System.Collections.Generic;

namespace KvaGames.Hex
{
	//todo: rename
	public class CenterHexTile : HexTile
	{
		public int radius = 3;
		public bool flatHead = true; //todo: add support for tophead. Fornow: all code assumes flathead.
		public List<List<HexTile>> ringList; //holds refrence hex-ring by hex-ring outwards by QRS coordinates
		public List<List<HexTile>> QrList; //holds refrence row by row by QR coordinates

		public HexTile grassTilePrefab;
		public HexTile rockTilePrefab;

		//creates or updates map whenever radius has been changed!
		public void BuildMap()
		{
			ringList = ringList ?? new List<List<HexTile>>(radius+1);

			//Building map from rings
			for (int i = 0; i < radius; i++)
			{
				ringList[i] = ringList[i] ?? BuildRing(i);
			}
			//setting up QrList of the tiles.
			int length = radius*2 + 1;
			QrList = new List<List<HexTile>>(length);
			for (int i=0; i<length; i++)
			{
				QrList[0] = new List<HexTile>(length);
			}

		}
		public List<HexTile> BuildRing(int r)
		{
			List<HexTile> ring = new List<HexTile>(r*6);
			HexCoord tc = HexCoord.FLAT_UP * r;
			HexTile t = Instantiate<HexTile>(grassTilePrefab, transform);
			t.setupCoords(tc, this);
			ring.Add(t);
			if (r>0)
			{
				for(HexDirectionFlat dir = HexDirectionFlat.UP; dir <= HexDirectionFlat.UP_RIGHT; dir++)
				//for (int dir=0; dir < 6; dir++)
				{
					for(int j=0; j<r;j++)
					{
						tc = HexCoord.Neighbour(tc, dir);
						t = Instantiate<HexTile>(grassTilePrefab, transform);
						t.setupCoords(tc, this);
						ring.Add(t);
					}
				}
			}
			return ring;

//function cube_ring(center, radius):
//    var results = []
//    # this code doesn't work for radius == 0; can you see why?
//    var cube = cube_add(center, 
//                        cube_scale(cube_direction(4), radius))
//    for each 0 ≤ i < 6:
//        for each 0 ≤ j < radius:
//            results.append(cube)
//            cube = cube_neighbor(cube, i)
//    return results
		}
		public void CleanMap()
		{
			ringList = new List<List<HexTile>>();
			HexTile[] childeren = GetComponentsInChildren<HexTile>();
			for(int i = childeren.Length-1; i >= 0; i--)
			{
				HexTile child = childeren[i];
				Destroy(child); //yummy yummy murder...
			}
		}

		// Use this for initialization
		void Start( )
		{

		}

		// Update is called once per frame
		void Update( ) {

		}
	}
}