using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public string debugPrefix = "SCENE LOADER --- ";

    public void LoadSceneByName(string sceneName){
        Debug.Log(debugPrefix + "Loading scene " + sceneName);
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    public void LoadSceneByIndex(int sceneIndex){
        Debug.Log(debugPrefix + "Loading scene " + sceneIndex);
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
    }

    public void QuitGame(){
        Debug.Log(debugPrefix + "Quitting game");
        Application.Quit();
    }
}
