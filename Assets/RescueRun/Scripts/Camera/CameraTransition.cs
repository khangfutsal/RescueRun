using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Watermelon;
namespace RescueRun
{
    public class CameraTransition : MonoBehaviour
    {
        [SerializeField] private List<GameObject> cameras;
        private void Update()
        {
            if(GameController.Instance.GetCurrentState() == GameState.Setup)
            {

            }
        }

        public void Show(string nameCamera)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                if(cameras[i].name == nameCamera)
                {
                    cameras[i].gameObject.SetActive(true);
                }
                cameras[i].SetActive(false);
            }

        }
        public IEnumerator TransitionSetUpGame()
        {
            float timeChangeCamera = 1f;
            string cameraIntro1 = "CameraIntro1";
            string cameraIntro2 = "CameraIntro2";
            string cameraJoyStick = "CameraJoyStick";

            Show(cameraIntro1);
            yield return new WaitForSeconds(timeChangeCamera);
            Show(cameraIntro2);
            yield return new WaitForSeconds(timeChangeCamera / 3);
            Show(cameraJoyStick);
        }
    }
}

