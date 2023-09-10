
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

// USING THIS SCRIPT TO TEST PERSISTENCE FOR ITEMS AND PLAYER ACROSS SCENES
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    public string sceneToLoad;

    public float loadTimer = 3f;

    public GameObject playerPrefab;

    private GameObject _playerInstance;

    [SerializeField] private Transform spawnLocation;

    private void Awake()
    {
        // Ensure only one instance of the GameManager exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // if (_playerInstance == null)
        // {
        //     _playerInstance = Instantiate(playerPrefab);
        //     
        //     _playerInstance.transform.position = spawnLocation.position;
        // }
        
        StartCoroutine(SceneDelay());
    }

    IEnumerator SceneDelay()
    {
        yield return new WaitForSeconds(loadTimer);
        LoadNewScene(sceneToLoad);

    }

    public void LoadNewScene(string sceneName)
    {
        // Load new scene
        SceneManager.LoadScene(1);
        
    }
}
