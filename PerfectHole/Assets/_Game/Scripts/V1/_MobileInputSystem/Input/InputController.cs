using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MobileInputSystem {
    public enum InputDevice {
        Screen,
        Mouse
    }
    public enum InputType {
        CurrentPosition,
        DeltaPosition,
    }
    public class InputController : MonoBehaviour {
        public InputDevice _inputDevice = InputDevice.Screen;
        public InputType _inputType = InputType.CurrentPosition;
        
        public static event Action<Vector3> OnTouchDown;
        public static event Action OnTouchUp;

        private Vector3 startPosition;
        private Vector3 endPosition;
        private Vector3 deltaPosition;


        private void FixedUpdate() {
            switch (_inputDevice) {
                case InputDevice.Screen:
                    HandleTouchInput();
                    break;
                case InputDevice.Mouse:
                    HandleMouseInput();
                    break;
            }
        }

        private void HandleTouchInput() {
            if (Input.touchCount <= 0) OnTouchUp?.Invoke();
            else {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase) {
                    case TouchPhase.Began:
                        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

                        startPosition = touch.position;
                        break;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        if (startPosition == Vector3.zero) return;

                        endPosition = touch.position;
                        deltaPosition = endPosition - startPosition;
                        OnTouchDown?.Invoke(_inputType == InputType.CurrentPosition ? touch.position : deltaPosition);
                        startPosition = endPosition;
                        break;

                    case TouchPhase.Ended:
                        endPosition = touch.position;
                        deltaPosition = endPosition - startPosition;
                        OnTouchDown?.Invoke(_inputType == InputType.CurrentPosition ? touch.position : deltaPosition);
                        startPosition = endPosition;
                        break;
                }
            }
        }

        private void HandleMouseInput() {
            if (Input.GetMouseButtonDown(0)) {
                startPosition = Input.mousePosition;
                OnTouchDown?.Invoke(_inputType == InputType.CurrentPosition ? Input.mousePosition : Vector3.zero);
                return;
            }

            if (Input.GetMouseButton(0)) {
                if (startPosition == Vector3.zero) return;

                endPosition = Input.mousePosition;
                deltaPosition = endPosition - startPosition;
                OnTouchDown?.Invoke(_inputType == InputType.CurrentPosition ? Input.mousePosition : deltaPosition);
                startPosition = endPosition;
                return;
            }

            if (Input.GetMouseButtonUp(0)) {
                endPosition = Input.mousePosition;
                deltaPosition = endPosition - startPosition;
                OnTouchUp?.Invoke();
                startPosition = endPosition;
            }

            return;
        }
    }
}