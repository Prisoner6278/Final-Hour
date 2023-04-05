using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Temp_cutscene_transition : MonoBehaviour
{
    public float waitTime;
    public string sceneNametoLoad;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedLood());
    }

    private IEnumerator DelayedLood()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(sceneNametoLoad);
    }
}
