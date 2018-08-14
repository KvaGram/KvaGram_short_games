/* using UnityEngine;
using System.Collections;
using UnityEditor;
namespace Kvavatar
{
     
    [CustomEditor(typeof(LevelScript))]
    public class KvavatarControllerEditor : Editor 
    {
        public override void OnInspectorGUI()
        {
            KvavatarController kvavatar = (LevelScript)target;
            


            myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
            EditorGUILayout.LabelField("Level", myTarget.Level.ToString());
        }
    }
} */