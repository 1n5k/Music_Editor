using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScanLoadScript : MonoBehaviour
{

    public void SceneLoad()
    {
        SceneManager.LoadScene("MusicEditor");
    }
}