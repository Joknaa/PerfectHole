using System;
using System.Collections.Generic;
using System.Linq;
using OknaaEXTENSIONS.CustomWrappers;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.V2 {
    public class LevelSpawnSystem : Singleton<LevelSpawnSystem> {
        public static Action<int> OnLevelGenerated;

        public Transform PlayerBoxInitialPosition;

        [Title("Prefabs", TitleAlignment = TitleAlignments.Centered)]
        public GameObject TrianglePrefab;

        public GameObject PlayerBoxPrefab;
        public GameObject CorrectPositionPrefab;

        [Title("Generation Params", TitleAlignment = TitleAlignments.Centered)]
        public Vector2Int Dimensions;

        public Vector2Int MinCorrectPosition;
        public Vector2Int MaxCorrectPosition;

        public float CellSize;


        private List<GameObject> _triangles = new List<GameObject>();
        private GameObject _playerBoxInstance;
        private GameObject _correctPositionInstance;
        private Transform _levelContainer;
        private int _levelID;

        private void Awake() {
            _levelID = 0;
            GenerateNextLevel();
        }

        public void GenerateNextLevel() => GenerateLevel();
        public void ReloadLevel() => GenerateLevel(true);

        [Button("Generate Level", ButtonSizes.Large), PropertySpace(20, 0)]
        private void GenerateLevel(bool reload = false) {
            ClearLevel();
            _levelContainer = new GameObject("LevelContainer") {
                transform = {
                    parent = transform,
                    localPosition = Vector3.zero
                }
            }.transform;

            if (!reload) {
                if (_correctPositionInstance != null) Destroy(_correctPositionInstance);
                _correctPositionInstance = Instantiate(CorrectPositionPrefab, GetRandomCorrectPosition(), "CorrectPosition");
                _correctPositionInstance.transform.SetSiblingIndex(1);
            }

            for (int i = 0; i < Dimensions.x; i++) {
                for (int j = 0; j < Dimensions.y; j++) {
                    var position = new Vector3(i * CellSize, j * CellSize, 0);
                    ;
                    var TriangleInstance = Instantiate(TrianglePrefab, position, $"Triangle_{i}_{j}", CellSize);
                    _triangles.Add(TriangleInstance);

                    position = new Vector2((i + 1) * CellSize, (j + 1) * CellSize);
                    TriangleInstance = Instantiate(TrianglePrefab, position, $"Triangle_{i}_{j}_inverted", CellSize, true);
                    _triangles.Add(TriangleInstance);
                }
            }

            if (_playerBoxInstance != null) Destroy(_playerBoxInstance);
            _playerBoxInstance = Instantiate(PlayerBoxPrefab, PlayerBoxInitialPosition.position, PlayerBoxPrefab.transform.rotation, _levelContainer);
            _playerBoxInstance.transform.SetSiblingIndex(0);
            _playerBoxInstance.name = "PlayerBox";

            _levelID++;
            OnLevelGenerated?.Invoke(_levelID);
        }

        private GameObject Instantiate(GameObject trianglePrefab, Vector2 position, string name, float scale = 1, bool rotate = false) {
            var instance = Instantiate(trianglePrefab, _levelContainer);
            instance.transform.localPosition = position;
            instance.transform.localScale = scale * Vector3.one;
            if (rotate) instance.transform.rotation = Quaternion.Euler(0, 0, 180);
            instance.name = name;
            return instance;
        }

        [Button("Clear Level", ButtonSizes.Large)]
        private void ClearLevel() {
            if (_levelContainer != null) DestroyImmediate(_levelContainer.gameObject);
            _triangles.Clear();
        }


        [Button("SpawnTest Correct Positions", ButtonSizes.Large)]
        private void SpawnTestCorrectPositions(int I = 100) {
            for (int i = 0; i < I; i++) {
                _correctPositionInstance = Instantiate(CorrectPositionPrefab, GetRandomCorrectPosition(), "CorrectPosition");
            }
        }


        private Vector2 GetRandomCorrectPosition() {
            float x = Random.Range(MinCorrectPosition.x, MaxCorrectPosition.x + 1) * CellSize;
            float y = Random.Range(MinCorrectPosition.y, MaxCorrectPosition.y + 1) * CellSize;

            return new Vector2(x, y);
        }
    }
}