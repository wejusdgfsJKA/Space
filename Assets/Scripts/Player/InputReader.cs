using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerControls;
namespace Player
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "ScriptableObjects/InputReader")]
    public class InputReader : ScriptableObject, IBasicControlsActions
    {
        public PlayerControls inputActions;
        public Vector2 LookDelta => inputActions.BasicControls.MouseLook.ReadValue<Vector2>();
        public event UnityAction<Vector3> MoveEvent = delegate { };
        public event UnityAction<float> RollEvent = delegate { }, FreeLookToggleEvent = delegate { }, ZoomEvent = delegate { };
        public event UnityAction ResetEvent = delegate { };
        public void EnablePlayerActions()
        {
            inputActions ??= new PlayerControls();
            inputActions.BasicControls.SetCallbacks(this);
            inputActions.Enable();
        }

        public void DisablePlayerActions()
        {
            if (inputActions != null)
            {
                inputActions.BasicControls.SetCallbacks(null);
                inputActions.Disable();
                MoveEvent = null;
                ResetEvent = null;
                RollEvent = FreeLookToggleEvent = ZoomEvent = null;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector3>());
        }

        public void OnRoll(InputAction.CallbackContext context)
        {
            RollEvent?.Invoke(context.ReadValue<float>());
        }

        /// <summary>
        /// This method only exists to satisfy the interface implementation. The actual mouse
        /// input is read directly from the LookDelta property.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void OnMouseLook(InputAction.CallbackContext context)
        {
            //nothing
        }

        public void OnFreeLookToggle(InputAction.CallbackContext context)
        {
            FreeLookToggleEvent?.Invoke(context.ReadValue<float>());
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            ZoomEvent?.Invoke(context.ReadValue<float>());
        }
        public void OnReset(InputAction.CallbackContext context)
        {
            ResetEvent?.Invoke();
        }
    }
}
