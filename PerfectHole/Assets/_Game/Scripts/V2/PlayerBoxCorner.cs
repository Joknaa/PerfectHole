using System;
using UnityEngine;

namespace PerfectHole.V2 {
    public class PlayerBoxCorner : MonoBehaviour {
        private PlayerBox _playerBox;

        public void Init(PlayerBox playerBox) {
            _playerBox = playerBox;
        }
        
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("CorrectPositionTrigger")) {
                _playerBox.AddCorrectPosition(this, other.transform);
            }
        }
        
        private void OnTriggerExit(Collider other) {
            if (other.gameObject.CompareTag("CorrectPositionTrigger")) {
                _playerBox.RemoveCorrectPosition(this);
            }
        }
    }
}