using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PersistantAudioSource : MonoBehaviour
{
    private static PersistantAudioSource ms_instance;
    [SerializeField]
    private List<int> m_allowedLevels;

    // Use this for initialization
    void Start()
    {
        ms_instance = this;
        DontDestroyOnLoad(this.gameObject);
    }


    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }


    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (!m_allowedLevels.Contains(SceneManager.GetActiveScene().buildIndex))
        {
            Destroy(this.gameObject);
        }
    }



    public static bool CheckToFadeOutAudio(int m_nextScene)
    {
        if(ms_instance == null)
        {
            return false;
        }

        if (!ms_instance.m_allowedLevels.Contains(m_nextScene))
        {
            ms_instance.gameObject.GetComponent<AudioSource>().loop = false;
            return true;
        }
        return false;
    }
}
