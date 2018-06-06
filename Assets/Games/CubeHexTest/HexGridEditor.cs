using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace KvaGames.Hex
{
	[CustomEditor(typeof(HexGrid))]
	public class HexGridEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			HexGrid grid = (HexGrid)target;

			if(GUILayout.Button("(RE)BUILD MAP"))
			{
				grid.BuildMap();
			}


		}
	}
}
