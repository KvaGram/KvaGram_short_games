using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KvaGames.Hex
{
	//todo: rename
	public class CenterHexTile : MonoBehaviour// : HexTile
	{
		public int radius = 3;
		public bool flatHead = true; //todo: add support for tophead. Fornow: all code assumes flathead.
		public List<List<HexTile>> ringList; //holds refrence hex-ring by hex-ring outwards by QRS coordinates
		public List<List<HexTile>> QrList; //holds refrence row by row by QR coordinates

		public HexTile tilePrefab;

		//-- recursive cunstructor
		public HexTile center;
		public void BuildMapRecursive()
		{
			center = Instantiate(tilePrefab);
			center.setup(HexCoordCubic.ZERO, flatHead);
			BuildTileRecursive(center);
		}
		public void BuildTileRecursive(HexTile t)
		{
			List<int> todo = new List<int>();
			for (int i = 0; i < t.neighbours.Length; i++)
			{
				HexCoordCubic n = HexCoordCubic.Neighbour(t.HexCoord, i);
				if(!t.neighbours[i] && LegalCoordinates(n))
				{
					HexTile tn = Instantiate(tilePrefab, transform);
					tn.setup(n, flatHead);
					int j = (i + 3) % 6;
					t.neighbours[i] = tn;
					tn.neighbours[j] = t;
					todo.Add(i);
				}
			}
			foreach(int i in todo)
			{
				HexTile tn = t.neighbours[i];

				int nLeft =  (i+2) % 6;  //your i+2 is my i+1
				int nRight = Mathf.Abs((i-2) % 6);  //your i-2 is my i-1
				int nSelf =  (i+3) % 6;  //your i+3 is me
				Debug.Log(string.Format("setting neighbour's neighbours {0} {1} {2}", nLeft, nRight, nSelf));
				     
				tn.neighbours[nLeft] = t.neighbours[(i + 1) % 6];
				tn.neighbours[nRight] = t.neighbours[Mathf.Abs((i - 1) % 6)];
				tn.neighbours[nSelf] = t;
				BuildTileRecursive(tn);
			}
		}
		public bool LegalCoordinates(HexCoordCubic n)
		{
			return n.Ring <= radius;
		}
		//end recursive cunstructor






		//creates or updates map whenever radius has been changed!
		public void BuildMap()
		{
			ringList = ringList ?? new List<List<HexTile>>(radius+1);

			//Building map from rings
			for (int i = 0; i <= radius; i++)
			{
				if (ringList.Count <= i)
					ringList.Add(BuildRing(i));
			}

			HexTile[] HexList = gameObject.GetComponentsInChildren<HexTile>();
			//setting up QrList of the tiles.
			int length = radius*2 + 1;
			QrList = new List<List<HexTile>>(length);
			//QrList.Add

			for (int i=-radius; i<= radius; i++)
			{
				List<HexTile> qList = HexList.Where((HexTile arg1) => arg1.HexCoord.q == i).OrderBy((HexTile arg1) => arg1.HexCoord.r).ToList();
				QrList.Add(qList);
			}

		}
		public List<HexTile> BuildRing(int r)
		{
			List<HexTile> ring = new List<HexTile>(r * 6);
			if (r > 0)
			{
				HexCoordCubic tc = HexCoordCubic.FLAT_DOWN_RIGHT * r;
				HexTile t = Instantiate(tilePrefab, transform);
				t.setup(tc, this);
				ring.Add(t);

				//for(HexDirectionFlat dir = HexDirectionFlat.UP; dir <= HexDirectionFlat.UP_RIGHT; dir++)
				for (int i = 0; i < 6; i++)
				{
					for (int j = 0; j < r; j++)
					{
						HexDirectionFlat dir = (HexDirectionFlat)i;
						tc = HexCoordCubic.Neighbour(tc, dir);
						t = Instantiate(tilePrefab, transform);
						t.setup(tc, this);
						ring.Add(t);
					}
				}
			}
			else
				ring.Add(center);
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
			//BuildMap();
			BuildMapRecursive();
		}

		// Update is called once per frame
		void Update( ) {

		}
	}
}