using PerfectHole.V2.Systems;
using UnityEngine;

namespace PerfectHole.V2 {
    public class Hole : MonoBehaviour {
        
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Triangle")) {
                LevelSpawnSystem.RemoveTriangle(other.gameObject);
            }
        }
    }
}