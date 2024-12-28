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

        public void Initialize()
        {
            ShowOnly("CameraKeyboard");
        }

        public void Show(string nameCamera)
        {

            for (int i = 0; i < cameras.Count; i++)
            {
                if (cameras[i].name == nameCamera)
                {
                    Debug.Log(cameras[i].name);
                    cameras[i].gameObject.SetActive(true);
                    break;
                }
            }
        }

        public void ShowOnly(string nameCamera)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                if (cameras[i].name == nameCamera)
                {
                    Debug.Log(cameras[i].name);
                    cameras[i].gameObject.SetActive(true);
                }
                else cameras[i].gameObject.SetActive(false);
            }
        }

        public void Hide(string nameCamera)
        {

            for (int i = 0; i < cameras.Count; i++)
            {
                if (cameras[i].name == nameCamera)
                {
                    cameras[i].gameObject.SetActive(false);
                    break;
                }

            }
        }
        public IEnumerator TransitionSetUpGame()
        {
            Time.timeScale = 1;
            Hide("CameraKeyboard");
            float timeChangeCamera = 3f;
            string cameraIntro1 = "CameraIntro1";
            string cameraIntro2 = "CameraIntro2";
            string cameraJoyStick = "CameraJoystick";

            Show(cameraIntro1);
            yield return new WaitForSeconds(timeChangeCamera);
            Show(cameraIntro2);
            yield return new WaitForSeconds(timeChangeCamera);
            Show(cameraJoyStick);
            yield return new WaitForSeconds(timeChangeCamera);
            StartCoroutine(UIController.Instance.GetUIGameplay().StartCountdown());


        }
    }
}

