using UnityEngine;

namespace _Game.Scripts.V2 {
    public class PlayerBox : MonoBehaviour {
        public int CorrectPositionsCount;

        private void Start() {
            var corners = GetComponentsInChildren<BoxCornerTrigger>();
            foreach (var corner in corners) {
                corner.Init(this);
            }
        }

        public void AddCorrectPosition() {
            CorrectPositionsCount++;

            if (CorrectPositionsCount == 4) LevelSpawnSystem.Instance.GenerateNextLevel();
        }

        public void RemoveCorrectPosition() {
            CorrectPositionsCount--;
        }
    }
}