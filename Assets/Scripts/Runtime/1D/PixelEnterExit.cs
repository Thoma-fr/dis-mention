using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PixelEnterExit : MonoBehaviour
{
    public void EnterEffect()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, 45), 1f, RotateMode.LocalAxisAdd));
        mySequence.Join(transform.DOScale(0, 1f));
    }

    public void ExitEffect(Vector3 pos)
    {
        transform.position = pos;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, 45), 1f, RotateMode.LocalAxisAdd));
        mySequence.Join(transform.DOScale(80, 1f));
        StartCoroutine(NextScene());
    }

    private IEnumerator NextScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync("Lobby2D");
    }
}
