using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace RescueRun
{
    public class ScanAnimal : MonoBehaviour
    {
        public Animal animalTarget;
        public MeshRenderer mesh;
        public LayerMask layerMask;

        private void Awake()
        {
            mesh = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            animalTarget = LevelController.Instance.GetCurrentAnimalTarget();
            Debug.Log(LevelController.Instance.GetCurrentAnimalTarget());
            HideMesh();
        }

        private void Update()
        {
            //if (!animalTarget.gameObject.activeSelf)
            //{
            //    animalTarget = LevelController.Instance.GetCurrentAnimalTarget();
            //}
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

