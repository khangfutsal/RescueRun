using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
namespace RescueRun
{
    public class ScanAnimal : MonoBehaviour
    {
        public Animal animalTarget;
        public MeshRenderer mesh;
        public LayerMask layerMask;
        public bool isCalled;

        private void Awake()
        {
            mesh = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            
            HideMesh();
        }

        private void Update()
        {
            if (GameController.Instance.GetCurrentState() == GameState.Start)
            {
                if(animalTarget == null && !isCalled)
                {
                    isCalled = true;
                    Controller.SetControl(Watermelon.InputType.Keyboard);
                    GameController.Instance.GetCameraTransition().ShowOnly("CameraKeyboard");
                    LevelController.Instance.DestroyLevel();
                }
                if (!animalTarget.gameObject.activeSelf)
                {
                    animalTarget = LevelController.Instance.GetCurrentAnimalTarget();
                }
            }
        }

        public void GetAnimalTarget()
        {
            animalTarget = LevelController.Instance.GetCurrentAnimalTarget();
        }

        public void HideMesh()
        {
            mesh.enabled = false;
        }

        public void ShowMesh()
        {
            mesh.enabled = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((layerMask.value & (1 << other.transform.gameObject.layer)) != 0)
            {
                if (animalTarget == other.GetComponent<Animal>() && !animalTarget.isCollected)
                {
                    ShowMesh();
                    animalTarget.IsOverlapped = true;
                }
            }
            
        }

        private void OnTriggerStay(Collider other)
        {
            if ((layerMask.value & (1 << other.transform.gameObject.layer)) != 0)
            {
                if (animalTarget == other.GetComponent<Animal>() && !animalTarget.isCollected)
                {
                    ShowMesh();
                    animalTarget.IsOverlapped = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ((layerMask.value & (1 << other.transform.gameObject.layer)) != 0)
            {
                if (animalTarget == other.GetComponent<Animal>() && !animalTarget.isCollected)
                {
                    HideMesh();
                    animalTarget.IsOverlapped = false;
                }
            }
        }

    }
}

