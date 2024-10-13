using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameend : MonoBehaviour
{
    [SerializeField] private TutorialEffect effect;
    // Start is called before the first frame update
    void Start()
    {
        
        Screen.SetResolution(640, 480, FullScreenMode.Windowed);
        effect.LaunchTutorialEffect();
        StartCoroutine(TimeLine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator TimeLine()
    {
        yield return new WaitForSeconds(5f);
        int test=0;
        List<DisplayInfo> displayLayout = new List<DisplayInfo>();
        Screen.GetDisplayLayout(displayLayout);
        while (test<20)
        {
            Screen.MoveMainWindowTo(displayLayout[0], new Vector2Int(Random.Range(0,1000), Random.Range(0, 1000)));
            Screen.SetResolution(Random.Range(0, 640), Random.Range(0, 480),FullScreenMode.Windowed);
            yield return new WaitForSeconds(.2f);
            test++;
        }
       
        yield return new WaitForSeconds(1f);
        Screen.MoveMainWindowTo(displayLayout[0], new Vector2Int(0, 0));
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        effect.LaunchTutorialEffect();
        yield return new WaitForSeconds(5f);
        Screen.SetResolution(640, 480, FullScreenMode.Windowed);
        yield return new WaitForSeconds(.2f);
        Screen.SetResolution(320, 240, FullScreenMode.Windowed);
        yield return new WaitForSeconds(.2f);
        Screen.SetResolution(120, 140, FullScreenMode.Windowed);
        yield return new WaitForSeconds(.2f);
        Application.Quit();
    }
}
