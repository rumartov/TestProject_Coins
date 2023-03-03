using System;
using System.Collections;
using Infrastructure.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private string _lastSceneUniqueId;
        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Load(string name, Action onLoaded = null)
        {
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));
        }

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            InitializeLastLevelId();

            if (SceneManager.GetActiveScene().name == nextScene &&
                !IsLastLevelEqualsCurrentLevel())
            {
                _lastSceneUniqueId = GetActiveSceneId(GetActiveSceneGameObjects());
                onLoaded?.Invoke();
                yield break;
            }

            var waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;

            _lastSceneUniqueId = GetActiveSceneId(GetActiveSceneGameObjects());
            onLoaded?.Invoke();
        }

        private bool IsLastLevelEqualsCurrentLevel() =>
            _lastSceneUniqueId != ""
            && _lastSceneUniqueId == GetActiveSceneId(GetActiveSceneGameObjects());

        private void InitializeLastLevelId()
        {
            if (string.IsNullOrEmpty(_lastSceneUniqueId)) 
                _lastSceneUniqueId = GetActiveSceneId(GetActiveSceneGameObjects());
        }

        private string GetActiveSceneId(GameObject[] levelGameObjects)
        {
            foreach (var gameObject in levelGameObjects)
            {
                if (gameObject.TryGetComponent(out UniqueId levelId))
                {
                    return levelId.Id;
                }
            }

            return "";
        }

        private GameObject[] GetActiveSceneGameObjects()
        {
            return SceneManager.GetActiveScene().GetRootGameObjects();
        }
    }
}