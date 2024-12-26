using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watermelon;
namespace RescueRun
{
    public static class Controller
    {

        private static InputType inputType;
        public static InputType InputType => inputType;

        private static IControlBehavior currentControl;
        public static IControlBehavior CurrentControl => currentControl;

        public static void Initialize(InputType inputType)
        {
            Controller.inputType = inputType;
        }

        public static void SetControl(IControlBehavior controlBehavior)
        {
            currentControl = controlBehavior;
        }

        public static void Enable()
        {
#if UNITY_EDITOR
            if (currentControl == null)
            {
                Debug.LogError("[Control]: Control behavior isn't set!");

                return;
            }
#endif

            currentControl.EnableControl();
        }

        public static void Disable()
        {
#if UNITY_EDITOR
            if (currentControl == null)
            {
                Debug.LogError("[Control]: Control behavior isn't set!");

                return;
            }
#endif

            currentControl.DisableControl();
        }
    }
}

