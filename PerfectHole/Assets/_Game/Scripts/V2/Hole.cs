using UnityEngine;

namespace _Game.Scripts.V2 {
    public class Hole : MonoBehaviour {
        
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Triangle")) {
                Level.RemoveTriangle(other.gameObject);
            }
        }
    }
}