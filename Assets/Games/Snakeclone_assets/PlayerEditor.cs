﻿using UnityEngine;
using System.Collections;
using UnityEditor;

namespace KvaGames.Snakeclone
{
	[CustomEditor(typeof(Player))]
	class PlayerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			Player player = (Player)target;

			base.OnInspectorGUI();
			EditorGUILayout.LabelField("Next tick in: ", (player.TickLength - player.TickTime).ToString() + " Secunds");
			EditorGUILayout.LabelField("Game Length: ", player.TickTime.ToString() + " Ticks");
			EditorGUILayout.LabelField("Player direction: ", player.Dir.ToString());
			//EditorGUILayout.LabelField("Next tick is: ", player.HitStatus);
			if(GUILayout.Button("Add tail"))
			{
				player.AddTail();
			}
		}
	}
}
