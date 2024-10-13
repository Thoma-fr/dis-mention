using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialEffect : MonoBehaviour
{
    [SerializeField] private TextMeshPro _tutoText;
    [SerializeField] private string _firstTuto;
    [SerializeField] private string _secondTuto;
    public float maxScale = 1.3f;
    public float minScale = 0.9f;

    public void LaunchTutorialEffect()
    {
        _tutoText.gameObject.SetActive(false);

        _tutoText.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
        _tutoText.text = _firstTuto;


        Sequence mySequence = DOTween.Sequence();
        mySequence.SetDelay(0.8f).OnComplete(() =>
        {
            if(GamaManager.instance != null )
                GamaManager.instance.PlayTextSFX();

            _tutoText.gameObject.SetActive(true);

            mySequence = DOTween.Sequence();
            mySequence.Append(_tutoText.transform.DOScale(new Vector3(minScale, minScale, minScale), 1.5f));
            mySequence.OnComplete(() =>
            {
                _tutoText.gameObject.SetActive(false);

                _tutoText.transform.localScale = new Vector3(maxScale, maxScale, maxScale);
                _tutoText.text = _secondTuto;

                mySequence = DOTween.Sequence();
                mySequence.SetDelay(2f).OnComplete(() =>
                {
                    _tutoText.gameObject.SetActive(true);

                    if (GamaManager.instance != null)
                        GamaManager.instance.PlayTextSFX();

                    mySequence = DOTween.Sequence();
                    mySequence.Append(_tutoText.transform.DOScale(new Vector3(minScale, minScale, minScale), 1.5f));
                    mySequence.OnComplete(() => _tutoText.gameObject.SetActive(false));
                });
            });
        });
    }
}
