using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OknaaEXTENSIONS.CustomWrappers;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PerfectHole.V2.Systems {
    public class LevelSpawnSystem : Singleton<LevelSpawnSystem> {
        public static Action<int> OnLevelGenerated;

        public Transform PlayerBoxInitialPosition;
        public Transform LevelContainer;

        [Title("Prefabs", TitleAlignment = TitleAlignments.Centered)]
        public LevelSegment _levelSegmentPrefab;
        public PlayerBox PlayerBoxPrefab;
        public GameObject CorrectPositionPrefab;

        [Title("Generation Params", TitleAlignment = TitleAlignments.Centered)]
        public Vector2Int Dimensions;
        public Vector2Int MinCorrectPosition;
        public Vector2Int MaxCorrectPosition;
        public float CellSize;

        private List<LevelSegment> _cellSegments = new List<LevelSegment>();
        private Coroutine _coroutine;
        private PlayerBox _playerBoxInstance;
        private GameObject _correctPositionInstance;
        private int _levelID;
        [SerializeField] private bool _isInitialLevel;

        public void Init() {
            _levelID = 0;
            _isInitialLevel = true;
            GenerateNextLevel();
            _playerBoxInstance.Init();
        }

        public void GenerateNextLevel() => GenerateLevel();
        public void ReloadLevel() => GenerateLevel(true);


        [Button("Generate Level", ButtonSizes.Large), PropertySpace(20, 0)]
        private void GenerateLevel(bool reload = false) {
            if (_cellSegments.Count > 0) {
                foreach (var segment in _cellSegments) {
                    segment.SetActive(true);
                }
            }

            if (_playerBoxInstance != null) {
                Destroy(_playerBoxInstance.gameObject);
                print("PlayerBox Destroyed");
            }
            if (_coroutine != null) StopCoroutine(_coroutine);

            if (reload == false) {
                if (_correctPositionInstance != null) Destroy(_correctPositionInstance);
                _correctPositionInstance = Instantiate(CorrectPositionPrefab, LevelContainer);
                SetupInstance(_correctPositionInstance.transform, GetRandomCorrectPosition(), "CorrectPosition");
                _correctPositionInstance.transform.SetSiblingIndex(1);
            }

            if (_isInitialLevel) _coroutine = StartCoroutine(GenerateSegments());

            _playerBoxInstance = Instantiate(PlayerBoxPrefab, PlayerBoxInitialPosition.position, PlayerBoxPrefab.transform.rotation, LevelContainer);
            _playerBoxInstance.transform.SetSiblingIndex(0);
            _playerBoxInstance.name = "PlayerBox";
            _playerBoxInstance.Init();

            _levelID++;
            if (reload == false) OnLevelGenerated?.Invoke(_levelID);
        }

        private IEnumerator GenerateSegments() {
            _isInitialLevel = false;

            // GenerateTriangles();
            GenerateCubes();

            yield return null;


            void GenerateTriangles() {
                for (int i = 0; i < Dimensions.x; i++) {
                    for (int j = 0; j < Dimensions.y; j++) {
                        var TriangleInstance = LevelSegmentPool.Get();
                        SetupInstance(TriangleInstance.transform, new Vector2(i * CellSize, j * CellSize), $"Triangle_{i}_{j}", CellSize);
                        _cellSegments.Add(TriangleInstance);

                        TriangleInstance = LevelSegmentPool.Get();
                        SetupInstance(TriangleInstance.transform, new Vector2((i + 1) * CellSize, (j + 1) * CellSize), $"Triangle_{i}_{j}_inverted", CellSize, true);
                        _cellSegments.Add(TriangleInstance);
                    }
                }
            }
            void GenerateCubes() {
                for (int i = 0; i < Dimensions.x; i++) {
                    for (int j = 0; j < Dimensions.y; j++) {
                        var cubeInstance = LevelSegmentPool.Get();
                        SetupInstance(cubeInstance.transform, new Vector2(i * CellSize, j * CellSize), $"Cube_{i}_{j}", CellSize);
                        _cellSegments.Add(cubeInstance);
                    }
                }
            }
        }

        private void SetupInstance(Transform instance, Vector2 position, string name, float scale = 1, bool rotate = false) {
            instance.parent = LevelContainer;
            instance.localPosition = position;
            instance.localScale = scale * Vector3.one;
            if (rotate) instance.rotation = Quaternion.Euler(0, 0, 180);
            instance.name = name;
        }

        [Button("Clear Level", ButtonSizes.Large)]
        private void ClearLevel() {

            foreach (var segment in _cellSegments.Where(segment => segment != null)) {
                DestroyImmediate(segment);
            }
            _cellSegments.Clear();

            if (_playerBoxInstance != null) DestroyImmediate(_playerBoxInstance);
            if (_correctPositionInstance != null) DestroyImmediate(_correctPositionInstance);
            // if (_levelContainer != null) DestroyImmediate(_levelContainer.gameObject);
            if (_coroutine != null) StopCoroutine(_coroutine);
        }


        [Button("SpawnTest Correct Positions", ButtonSizes.Large)]
        private void SpawnTestCorrectPositions(int I = 100) {
            for (int i = 0; i < I; i++) {
                _correctPositionInstance = Instantiate(CorrectPositionPrefab, LevelContainer);
                SetupInstance(_correctPositionInstance.transform, GetRandomCorrectPosition(), "CorrectPosition");
            }
        }


        private Vector2 GetRandomCorrectPosition() {
            float x = Random.Range(MinCorrectPosition.x, MaxCorrectPosition.x + 1) * CellSize;
            float y = Random.Range(MinCorrectPosition.y, MaxCorrectPosition.y + 1) * CellSize;

            return new Vector2(x, y);
        }

        // public static void RemoveTriangle(GameObject triangle) => LevelSegmentPool.Release(triangle.GetComponentInParent<LevelSegment>());
        public static void RemoveTriangle(GameObject triangle) => triangle.GetComponent<LevelSubSegment>().SetActive(false);
    }
}