using System.Collections.Generic;
using PerfectHole.V2.Systems;
using UnityEngine;

namespace PerfectHole.V2 {
    public class PlayerBox : MonoBehaviour {
        public int CorrectPositionsCount;
        private Dictionary<PlayerBoxCorner, Transform> CornerLocks = new Dictionary<PlayerBoxCorner, Transform>();

        public void Init() {
            var corners = GetComponentsInChildren<PlayerBoxCorner>();
            foreach (var corner in corners) {
                corner.Init(this);
            }
        }

        public void AddCorrectPosition(PlayerBoxCorner corner, Transform lockTransform) {
            CornerLocks.Add(corner, lockTransform);
            CorrectPositionsCount++;

            if (CorrectPositionsCount == 4) {
                GameStateSystem.Instance.GameState = GameState.LevelFinished;
                
            }
        }

        public void RemoveCorrectPosition(PlayerBoxCorner corner) {
            if (CorrectPositionsCount == 0) return;
            if (CornerLocks.ContainsKey(corner)) CornerLocks.Remove(corner);
            
            CorrectPositionsCount--;
        }
    }
}