using System.Collections.Generic;
using OknaaEXTENSIONS.CustomWrappers;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _Game.Scripts.V2 {
    public class LevelSpawnSystem : Singleton<LevelSpawnSystem> {
        public GameObject TrianglePrefab;
        public GameObject PlayerBoxPrefab;
        public Transform PlayerBoxInitialPosition;
        public Transform LevelContainer;
        
        [Title("Generation Params", TitleAlignment = TitleAlignments.Centered)]
        public int X;
        public int Y;
        public float CellSize;
        
        
        private List<GameObject> _triangles = new List<GameObject>();
        private GameObject _playerBoxInstance;

        [Button("Generate Level", ButtonSizes.Large), PropertySpace(20, 0)]
        public void GenerateLevel() {
            if (_triangles.Count > 0) {
                foreach (var triangle in _triangles) {
                    if (triangle != null) Destroy(triangle);
                }
                _triangles.Clear();
            }

            if (_playerBoxInstance != null) {
                // DestroyImmediate(_playerBoxInstance);
                Destroy(_playerBoxInstance);
            }
            
            for (int i = 0; i < X; i++) {
                for (int j = 0; j < Y; j++) {
                    var straightTriangle = Instantiate(TrianglePrefab, LevelContainer);
                    straightTriangle.transform.position = new Vector3(i * CellSize, j * CellSize, 0);
                    straightTriangle.transform.localScale = Vector3.one * CellSize;
                    straightTriangle.name = $"Triangle_{i}_{j}";
                    _triangles.Add(straightTriangle);

                    
                    var invertedTriangle = Instantiate(TrianglePrefab, LevelContainer);
                    invertedTriangle.transform.position = new Vector3(i * CellSize + CellSize, j * CellSize + CellSize, 0);
                    invertedTriangle.transform.localScale = Vector3.one * CellSize;
                    invertedTriangle.transform.rotation = Quaternion.Euler(0, 0, 180);
                    invertedTriangle.name = $"Triangle_{i}_{j}_inverted";
                    _triangles.Add(invertedTriangle);
                }
                
            }
            
            _playerBoxInstance = Instantiate(PlayerBoxPrefab);
            _playerBoxInstance.transform.position = PlayerBoxInitialPosition.position;
        }
    }
}