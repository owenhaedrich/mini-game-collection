using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniGameCollection
{
    public class SceneTransitioner : MonoBehaviour
    {
        [field: SerializeField] public string SceneName { get; private set; } = string.Empty;

        /// <summary>
        ///     Load scene.
        /// </summary>
        public void LoadScene()
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}