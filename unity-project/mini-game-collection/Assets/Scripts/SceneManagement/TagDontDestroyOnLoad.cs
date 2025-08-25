using UnityEngine;

namespace MiniGameCollection.SceneManagement
{
    public class TagDontDestroyOnLoad : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this);
        }

    }
}
