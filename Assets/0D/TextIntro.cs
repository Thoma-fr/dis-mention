using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextIntro : MonoBehaviour
{
    // Start is called before the first frame update
   [SerializeField] List<string> _textlist = new List<string>();
    [SerializeField] private TextMeshPro _textMeshPro;
    [SerializeField] private float _textDuration=5f;
    [SerializeField] private AudioClip _audioClipText;
    [SerializeField] private AudioSource _audioSource;
    void Start()
    {
        StartCoroutine(DisplayText());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DisplayText()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < _textlist.Count; i++)
        {
            _textMeshPro.text = _textlist[i];
            _audioSource.PlayOneShot(_audioClipText);
            yield return new WaitForSeconds(_textDuration);

        }
        SceneManager.LoadScene("Lobby1D");
    }
}
