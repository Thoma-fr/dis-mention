using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueBox : MonoBehaviour
{
    [SerializeField] List<string> _textlist = new List<string>();
    [SerializeField] private TextMeshPro _textMeshPro;
    [SerializeField] private float _textDuration = 2f;
    [SerializeField] private GameObject _playeribjectref;
    [SerializeField] private bool _isbig;
    void Start()
    {
        StartCoroutine(DisplayText());
    }
    IEnumerator DisplayText()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < _textlist.Count; i++)
        {
            _textMeshPro.text = _textlist[i];
            yield return new WaitForSeconds(_textDuration);
            if(_playeribjectref != null)
                _playeribjectref.transform.DOShakeScale(.2f);
            transform.DOShakeScale(.2f);
            if (GamaManager.instance != null)
            {
                GamaManager.instance.PlayTextSFX();
                if (!_isbig)
                {
                    GamaManager.instance.PlayTalkcute();
                }
                else
                    GamaManager.instance.PlayTalkbold();
            }
        }
        gameObject.SetActive(false);
    }
}
