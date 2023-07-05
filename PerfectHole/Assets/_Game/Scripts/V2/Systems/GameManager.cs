using System;
using OknaaEXTENSIONS.CustomWrappers;
using UnityEngine;

namespace PerfectHole.V2.Systems {
    public class GameManager : Singleton<GameManager> {
        private void Awake() => Init();

        private static void Init() {
            Application.targetFrameRate = 60;
            LevelInteractionManager.Instance.Init();
            UIController.Instance.Init();
            LevelSegmentPool.Init();
            LevelSpawnSystem.Instance.Init();
        }
        
        public void Reset() {
            DisposeAll();
            
            LevelInteractionManager.Instance.Init();
            UIController.Instance.Init();
            LevelSpawnSystem.Instance.Init();
        }

        private static void DisposeAll() {
            UIController.Instance.Dispose();
            LevelSpawnSystem.Instance.Dispose();
            LevelInteractionManager.Instance.Dispose();
        }
        
        public override void Dispose() {
            LevelSegmentPool.Dispose();
            DisposeAll();
            
            base.Dispose();
        }
    }
}