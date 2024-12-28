using System.Collections;
using System.Collections.Generic;
using Lib;
using UnityEngine;
using UnityEngine.Events;
namespace RescueRun
{
    public class DangerZone : MonoBehaviour
    {
        [SerializeField] private int time;
        [SerializeField] private int meter;
        [SerializeField] private int idxLine;
        [SerializeField] private int threshold;

        [SerializeField] private List<LineData> lineData;
        public LayerMask layerMask;


        private void Start()
        {
            lineData = GameController.Instance.GetLineDatabase().lines;
            StartCoroutine(Moving());
        }

        private void Update()
        {
            if (GameController.Instance.GetCurrentState() == GameState.Start)
            {
                if (Controller.InputType == Watermelon.InputType.UIJoystick)
                {
                    Debug.Log(GameController.Instance.GetPlayer().transform.position.z - transform.position.z);
                    if (GameController.Instance.GetPlayer().transform.position.z - transform.position.z <= Mathf.Abs(threshold))
                    {
                        Debug.Log("Enemy near");
                        GameController.Instance.GetCameraTransition().ShowOnly("CameraJoystickNear");
                    }
                }
            }

        }

        public void AssignLineData(LineData lineData)
        {
            meter = lineData.meter;
            time = lineData.time;
        }

        public LineData GetLineData(int idxLine)
        {
            if (lineData[idxLine] != null)
            {
                return lineData[idxLine];
            }
            return null;
        }


        public IEnumerator Moving()
        {
            yield return new WaitUntil(() => GameController.Instance.GetCurrentState() == GameState.Start);
            var lineData = GetLineData(idxLine++);
            if (lineData != null)
            {
                AssignLineData(lineData);

                float startZ = transform.position.z;
                float targetZ = startZ + meter;
                float elapsedTime = 0;

                while (elapsedTime < time)
                {
                    elapsedTime += Time.deltaTime;
                    float t = elapsedTime / time;

                    float z = Mathf.Lerp(startZ, targetZ, t);
                    transform.position = new Vector3(transform.position.x, transform.position.y, z);

                    yield return null;
                }

                transform.position = new Vector3(transform.position.x, transform.position.y, targetZ);
                StartCoroutine(Moving());
            }


        }


    }
}

