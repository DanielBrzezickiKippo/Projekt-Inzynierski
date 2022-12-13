using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
 

namespace inzynierka
{
    public static class SceneManager
    {
        public static void NextScene()
        {
            int sceneId = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneId + 1);
        }

        public static void ResetScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
