using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EscapeMasters {
    public class TouchInputController : InputController {

        protected override void HandleInput() {
            if (Input.touchCount <= 0) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;

            Touch touch = Input.GetTouch(0);
            switch (touch.phase) {
                case TouchPhase.Began:
                    OnInputDown?.Invoke(touch.position);
                    break;
                
                case TouchPhase.Moved:
                    OnInputMove?.Invoke(touch.position);
                    break;
                
                case TouchPhase.Stationary:
                    OnInputStationary?.Invoke(touch.position);
                    break;
                 
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    OnInputUp?.Invoke(touch.position);
                    break;
            }
        }
    }
}