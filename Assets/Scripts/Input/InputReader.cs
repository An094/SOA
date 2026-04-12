using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInputActions;

namespace Platformer
{
    public interface IInputReader
    {
        Vector2 Direction { get; }
        void EnablePlayerActions();
    }

    [CreateAssetMenu(fileName = "InputReader", menuName = "Platformer/Input/InputReader")]
    public class InputReader : ScriptableObject, IInputReader, IPlayerActions
    {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<bool> Jump = delegate { };
        public event UnityAction<Vector2, bool> Look = delegate { };
        public event UnityAction EnableMouseControlCamera = delegate { };
        public event UnityAction DisableMouseControlCamera = delegate { };
        public event UnityAction<bool> Dash = delegate { };
        public event UnityAction<bool> Fire = delegate { };

        public PlayerInputActions inputActions;
        public Vector2 Direction => inputActions.Player.Move.ReadValue<Vector2>();
        public bool IsJumpKeyPressed => inputActions.Player.Jump.IsPressed();

        public void EnablePlayerActions()
        {
            if(inputActions == null)
            {
                inputActions = new PlayerInputActions();
                inputActions.Player.SetCallbacks(this);
            }

            inputActions.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            switch(context.phase)
            {
                case InputActionPhase.Started:
                    Jump.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Jump.Invoke(false);
                    break;
            }
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            Look.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
        }

        private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

        public void OnMouseControlCamera(InputAction.CallbackContext context)
        {
            switch(context.phase)
            {
                case InputActionPhase.Started:
                    EnableMouseControlCamera.Invoke();
                    break;
                case InputActionPhase.Canceled:
                    DisableMouseControlCamera.Invoke();
                    break;
            }
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if(context.phase == InputActionPhase.Canceled)
            {
                Fire?.Invoke(true);
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            switch(context.phase)
            {
                case InputActionPhase.Started:
                    Dash.Invoke(true);
                    break;
                case InputActionPhase.Canceled:
                    Dash.Invoke(false);
                    break;
            }
        }
    }
}
