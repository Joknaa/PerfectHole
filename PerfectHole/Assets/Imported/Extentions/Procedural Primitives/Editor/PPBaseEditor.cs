using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace ProceduralPrimitivesUtil
{
    [CustomEditor(typeof(PPBase), true)]
    [CanEditMultipleObjects]
    public class PPBaseEditor : Editor
    {
        bool fold = true;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            foreach (PPBase pp in targets)
            {
                if (pp.gameObject.scene.name == null) continue;
                if (pp.isDirty && pp.GetType() != typeof(PPCombiner)) pp.Apply();
            }

            if (targets.Length == 1)
            {
                PPBase pp = (PPBase)target;
                if (pp.GetType() == typeof(PPCombiner))
                {
                    EditorGUILayout.HelpBox("Runtime combining is not recommended if elements are very complex", MessageType.None);
                    if (GUILayout.Button("Apply")) pp.Apply();
                }
                EditorGUILayout.LabelField("");
                fold = EditorGUILayout.Foldout(fold, "Editor Functions");
                if (fold)
                {
                    EditorGUILayout.HelpBox("Mesh asset will be saved under \"Assets/Procedural Primitives/Temp\" folder", MessageType.None);
                    if (GUILayout.Button("Quick Save"))
                    {
                        QuickSave(pp);
                    }
                }
            }
        }

        public void QuickSave(PPBase pp)
        {
            string folderPath = "Assets/Procedural Primitives";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                string guid = AssetDatabase.CreateFolder("Assets", "Procedural Primitives");
                folderPath = AssetDatabase.GUIDToAssetPath(guid);
            }
            folderPath += "/Temp";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                string guid = AssetDatabase.CreateFolder("Assets/Procedural Primitives", "Temp");
                folderPath = AssetDatabase.GUIDToAssetPath(guid);
            }
            string assetName = pp.mesh.name.Trim();
            string assetPath = folderPath + "/" + assetName + ".asset";
            int counter = 1;
            while (File.Exists(assetPath))
            {
                counter++;
                assetPath = folderPath + "/" + assetName + "_" + counter.ToString() + ".asset";
            }
            AssetDatabase.CreateAsset(pp.mesh, assetPath);
            pp.ForceRecreateMesh();
        }
    }
}
