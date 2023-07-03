using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _Game.Scripts.V2 {
    public class LevelSpawnSystem : MonoBehaviour {
        public GameObject TrianglePrefab;
        public Transform LevelContainer;
        
        [Title("Params", TitleAlignment = TitleAlignments.Centered)]
        public int X;
        public int Y;
        public float CellSize;
        
        private List<GameObject> _triangles = new List<GameObject>();

        [Button("Generate Level", ButtonSizes.Large)]
        public void GenerateLevel() {
            if (_triangles.Count > 0) {
                foreach (var triangle in _triangles) {
                    if (triangle != null) DestroyImmediate(triangle);
                }
                _triangles.Clear();
            }
            
            for (int i = 0; i < X; i++) {
                for (int j = 0; j < Y; j++) {
                    var straightTriangle = PrefabUtility.InstantiatePrefab(TrianglePrefab, LevelContainer) as GameObject;
                    straightTriangle.transform.position = new Vector3(i * CellSize, j * CellSize, 0);
                    straightTriangle.transform.localScale = Vector3.one * CellSize;
                    straightTriangle.name = $"Triangle_{i}_{j}";
                    _triangles.Add(straightTriangle);

                    
                    var invertedTriangle = PrefabUtility.InstantiatePrefab(TrianglePrefab, LevelContainer) as GameObject;
                    invertedTriangle.transform.position = new Vector3(i * CellSize + CellSize, j * CellSize + CellSize, 0);
                    invertedTriangle.transform.localScale = Vector3.one * CellSize;
                    invertedTriangle.transform.rotation = Quaternion.Euler(0, 0, 180);
                    invertedTriangle.name = $"Triangle_{i}_{j}_inverted";
                    _triangles.Add(invertedTriangle);
                }
                
            }
            
            
        }
    }
}