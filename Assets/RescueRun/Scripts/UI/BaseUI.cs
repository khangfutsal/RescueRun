using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RescueRun
{
    public abstract class BaseUI : MonoBehaviour
    {
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }

        public void Show()
        {
            this.gameObject.SetActive(true);
        }
    }
}

