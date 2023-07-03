using System;
using System.Collections.Generic;
using MobileInputSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Scripts.V2 {
    public class Level : MonoBehaviour {
        public static Action OnCorrectPositionReached;
        
        [Title("CorrectPositions", TitleAlignment = TitleAlignments.Centered)]
        public List<Transform> _correctPositions = new List<Transform>();

        
        private Camera _camera;
        private int _correctPositionsIndex;

        void Start() {
            LevelSpawnSystem.Instance.GenerateLevel();
            
            _camera = Camera.main;
            foreach (var correctPosition in _correctPositions) {
                correctPosition.gameObject.SetActive(false);
            }
            _correctPositions[0].gameObject.SetActive(true);
            
            InputController.OnTouchDown += OnTouchDown;
            OnCorrectPositionReached += OnCorrectPositionReachedHandler;
        }

        private void OnCorrectPositionReachedHandler() {
            print("Win");
            _correctPositions[_correctPositionsIndex].gameObject.SetActive(false);
            _correctPositionsIndex++;
            _correctPositionsIndex %= _correctPositions.Count;
            _correctPositions[_correctPositionsIndex].gameObject.SetActive(true);
            LevelSpawnSystem.Instance.GenerateLevel();
        }

        private void OnTouchDown(Vector3 position) {
            print("Touch down");
            Ray ray = _camera.ScreenPointToRay(position);
            Debug.DrawRay(ray.origin, ray.direction * 3, Color.yellow);
            if (!Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit)) return;

            var hitObject = hit.transform.gameObject;
            Debug.Log("Hit: " + hitObject.name);

            if (hitObject.layer != LayerMask.NameToLayer("Level")) return;
            
            Destroy(hitObject.transform.parent.gameObject);
            print("Hit triangle : " + hitObject.name);
        }

        private void OnDestroy() {
            InputController.OnTouchDown -= OnTouchDown;
            OnCorrectPositionReached -= OnCorrectPositionReachedHandler;
        }
    }
}