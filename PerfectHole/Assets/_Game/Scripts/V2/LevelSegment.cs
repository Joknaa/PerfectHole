using UnityEngine;

namespace PerfectHole.V2 {
    public class LevelSegment : MonoBehaviour {
        public bool IsActive;


        public void Init(Vector3 newPosition) {
            transform.position = newPosition;
        }

        public void SetActive(bool isActive) {
            IsActive = isActive;
            gameObject.SetActive(isActive);
        }
    }
}