using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCam : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject cam1,cam2;
    void Start()
    {
        StartCoroutine(SwitchCam());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator SwitchCam()
    {
        yield return new WaitForSeconds(30f);
        cam1.SetActive(false);
        yield return new WaitForSeconds(15f);
        Debug.Log("change");
        SceneManager.LoadScene("0Dpt2");
    }
}
