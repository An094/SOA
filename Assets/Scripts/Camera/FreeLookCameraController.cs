using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Platformer
{
    public class FreeLookCameraController : InputAxisControllerBase<FreeLookCameraController.Reader>
    {
        [SerializeField] InputReader input;
        [SerializeField] float speedMultiplier = 500f;

        bool isRMBPressed;
        bool isDeviceMouse;
        bool cameraMovementLock;

        protected override void OnEnable()
        {
            base.OnEnable();
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        protected override void OnDisable()
        {
            input.Look -= OnLook;
            input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
        }

        private void OnDisableMouseControlCamera()
        {
            isRMBPressed = false;

            //Unlock the cursor and make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            //Reset the camera axis to prevent jumping when re-enabling mouse control
            foreach(var controller in Controllers)
            {
                if(controller.Input.InputReader == input)
                {
                    controller.Input.ProcessInput(Vector2.zero);
                }
            }
        }

        private void OnEnableMouseControlCamera()
        {
            isRMBPressed = true;

            //Lock the cursor to the center of the screen and hide it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(DisableMouseForFrame());
        }

        IEnumerator DisableMouseForFrame()
        {
            cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            cameraMovementLock = false;
        }

        private void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
        {
            if (cameraMovementLock) return;

            if (isDeviceMouse && !isRMBPressed) return;

            //If the device is mouse use fixedDeltaTime, otherwise use deltaTime
            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;

            foreach (var controller in Controllers)
            {
                if (controller.Input.InputReader == input)
                {
                    controller.Input.ProcessInput(new Vector2(cameraMovement.x, cameraMovement.y)  * (speedMultiplier * deviceMultiplier));
                }
            }
        }

        void Update()
        {
            if(Application.isPlaying)
            {
                UpdateControllers();
            }
        }

        [Serializable]
        public class Reader : IInputAxisReader
        {
            public InputReader InputReader;
            private Vector2 m_Value;

            public void ProcessInput(Vector2 InInput)
            {
                m_Value = InInput;
            }

            public float GetValue(UnityEngine.Object context, IInputAxisOwner.AxisDescriptor.Hints hint)
            {
                return hint == IInputAxisOwner.AxisDescriptor.Hints.Y ? m_Value.y : m_Value.x;
            }
        }
    }
}
