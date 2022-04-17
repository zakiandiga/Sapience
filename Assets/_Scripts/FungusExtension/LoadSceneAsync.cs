using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fungus
{
    [CommandInfo("Flow",
                 "Load Scene Async",
                 "Load Unity Scene asynchronously. This is useful " +
                 "for splitting a large game across multiple scene files to reduce peak memory " +
                 "usage. Previously loaded assets will be released before loading the scene to free up memory." +
                 "The scene to be loaded must be added to the scene list in Build Settings.")]
    [AddComponentMenu("")]
    public class LoadSceneAsync : LoadScene
    {
        private string previousScene;
        public override void OnEnter()
        {
            previousScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);

            SceneManager.sceneLoaded += UnloadPreviousScene;
        }

        private void UnloadPreviousScene(Scene scene, LoadSceneMode loadSceneMode)
        {
            SceneManager.sceneLoaded -= UnloadPreviousScene;

            if(SceneManager.GetActiveScene() != scene)
                SceneManager.SetActiveScene(scene);

            SceneManager.UnloadSceneAsync(previousScene);
        }

        public override Color GetButtonColor()
        {
            return new Color32(235, 191, 217, 255);
        }
    }

}
