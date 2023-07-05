using System;
using System.Collections.Generic;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace PerfectHole.V2.Systems {
    public static class LevelSegmentPool {
        private static ObjectPool<LevelSegment> _pool;
        private static List<LevelSegment> _pooledObjects = new List<LevelSegment>();
        private static LevelSegment _originalPrefab;

        private static bool _isInit;
        
        public static void Init() {
            if (_isInit) return;
            _pooledObjects.Clear();
            _originalPrefab = LevelSpawnSystem.Instance.LevelSegmentPrefab;
            _pool = new ObjectPool<LevelSegment>(
                OnCreate,
                OnGet,
                OnRelease
            );
            _isInit = true;
        }
        
        public static void ForEach(Action<LevelSegment> action) {
            _pooledObjects.ForEach(action);
        }

        public static LevelSegment Get() {
            if (!_isInit) Init();
            return _pool.Get();
        }

        public static void Release(LevelSegment levelSegment) => _pool.Release(levelSegment);


        private static LevelSegment OnCreate() => Object.Instantiate(_originalPrefab);

        private static void OnGet(LevelSegment levelSegment) {
            _pooledObjects.Add(levelSegment);
            levelSegment.SetActive(true);
        }

        private static void OnRelease(LevelSegment levelSegment) {
            _pooledObjects.Remove(levelSegment);
            levelSegment.SetActive(false);
        }


        public static void Dispose() {
            if (!_isInit) return;
            _pool?.Dispose();
            _pool = null;
            _originalPrefab = null;
            _isInit = false;
        }
    }
}
