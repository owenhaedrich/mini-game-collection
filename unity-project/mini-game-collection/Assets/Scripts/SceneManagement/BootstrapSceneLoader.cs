using UnityEngine;
using UnityEngine.SceneManagement;


namespace MiniGameCollection.SceneManagement
{
    public class BootstrapSceneLoader : MonoBehaviour
    {
        [field: SerializeField] public string SceneName { get; private set; } = string.Empty;

        void LateUpdate()
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
