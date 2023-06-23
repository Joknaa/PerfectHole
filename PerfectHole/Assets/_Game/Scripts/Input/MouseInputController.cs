using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EscapeMasters {
    public class MouseInputController : InputController {
        
        private Vector3 _previousPosition = Vector3.zero;

        protected override void HandleInput() {
            var position = Input.mousePosition;

            if (Input.GetMouseButtonUp(0)) {
                OnInputUp?.Invoke(position);
                _previousPosition = position;
                return;
            }
            
            if (Input.GetMouseButtonDown(0)) {
                // if (EventSystem.current.IsPointerOverGameObject()) return;
                OnInputDown?.Invoke(position);
            }
            
            if (Input.GetMouseButton(0)) {
                // if (EventSystem.current.IsPointerOverGameObject()) return;
                if (position != _previousPosition) OnInputMove?.Invoke(position);
                else OnInputStationary?.Invoke(position);
            }
            
            
            _previousPosition = position;
        }
    }
}