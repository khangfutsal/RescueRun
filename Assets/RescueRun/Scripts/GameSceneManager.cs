using System.Collections;
using System.Collections.Generic;
using Lib;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace RescueRun
{
    public class GameSceneManager : Singleton<GameSceneManager>
    {
        private Coroutine currentLoadSceneCoroutine;
        public void LoadScene(string name)
        {
            UIController.Instance.GetUILoading().ResetLoadingState();
            if (currentLoadSceneCoroutine != null)
            {
                Debug.LogWarning($"Scene '{name}' is already loading!");
                return;
            }

            currentLoadSceneCoroutine = StartCoroutine(LoadSceneAsync(name));
            UIController.Instance.GetUILoading().Show();
        }

        public void UnloadScene(string name)
        {
            UIController.Instance.GetUILoading().ResetLoadingState();
            if (currentLoadSceneCoroutine != null)
            {
                Debug.LogWarning($"Scene '{name}' is already loading!");
                return;
            }
            currentLoadSceneCoroutine = StartCoroutine(UnloadSceneAsync(name));
            UIController.Instance.GetUILoading().Show();
        }

        private IEnumerator UnloadSceneAsync(string name)
        {
            float minLoadingTime = 1.0f; // Thời gian tối thiểu hiển thị loading
            float elapsedTime = 0f;

            if (SceneManager.GetSceneByName(name).isLoaded)
            {
                Debug.Log($"Starting to unload scene: {name}");

                AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(name);

                if (unloadOperation != null)
                {
                    while (!unloadOperation.isDone || elapsedTime < minLoadingTime)
                    {
                        elapsedTime += Time.deltaTime;

                        if (unloadOperation.progress >= 0.9f)
                        {
                            break;
                        }

                        // Tính tiến trình dựa trên thời gian tối thiểu
                        float progress = Mathf.Clamp01(elapsedTime / minLoadingTime);
                        UIController.Instance?.GetUILoading()?.SetSliderLoading((int)(progress * 100));

                        yield return null;
                    }

                }

                // Dọn dẹp tài nguyên không sử dụng
                yield return Resources.UnloadUnusedAssets();
                System.GC.Collect();

            }

            // Ẩn giao diện loading
            UIController.Instance?.GetUILoading()?.Hide();
            Debug.Log($"Scene '{name}' has been unloaded and UI hidden.");
            currentLoadSceneCoroutine = null;
        }


        IEnumerator LoadSceneAsync(string name)
        {
            float minLoadingTime = 1.0f;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
            asyncOperation.allowSceneActivation = false;

            asyncOperation.completed += (AsyncOperation op) =>
            {
                // Xử lý khi load hoàn tất
                Debug.Log("Scene loaded!");

                // Ẩn giao diện loading sau khi load hoàn tất
                UIController.Instance.GetUILoading().Hide();
                currentLoadSceneCoroutine = null;
            };

            float elapsedTime = 0f;
            while (!asyncOperation.isDone)
            {
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                UIController.Instance.GetUILoading().SetSliderLoading((int)(progress * 100));
                elapsedTime += Time.deltaTime;
                Debug.Log($"Progress: {asyncOperation.progress}, ElapsedTime: {elapsedTime}, AllowSceneActivation: {asyncOperation.allowSceneActivation}");

                if (asyncOperation.progress >= 0.9f && elapsedTime >= minLoadingTime)
                {
                    asyncOperation.allowSceneActivation = true;
                    Debug.Log($"Done: {asyncOperation.progress}, ElapsedTime: {elapsedTime}, AllowSceneActivation: {asyncOperation.allowSceneActivation}");

                }

                yield return null;
            }
        }




    }

}
