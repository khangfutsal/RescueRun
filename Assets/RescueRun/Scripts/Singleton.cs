using UnityEngine;

namespace Lib{
    public class Singleton<T> : MonoBehaviour where T : Component{
        protected static T instance;

        public static T Instance {
            get {
                if (instance == null) {
                    instance = FindObjectOfType<T>();
                    if (instance == null) {
                        GameObject singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake() {
            if (instance == null) {
                instance = this as T;
                //DontDestroyOnLoad(this.gameObject);
            }
            else {
                Destroy(gameObject);
            }

            OnAwake();
        }

        protected virtual void OnAwake() { }
    }
}