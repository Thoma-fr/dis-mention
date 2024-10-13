using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Character25Dmove : MonoBehaviour
{
    private float _horizontal;
    [SerializeField]private Rigidbody _rb;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxZ,_minbz;

    [SerializeField] private LifeCounter _lifeCounter;
    // Start is called before the first frame update
    [SerializeField] private bool _aiRight,_aiLeft;
    private bool _lookAtPlayer;
    [SerializeField] private GameObject _camref;
    [SerializeField] private Collider _col;
    [SerializeField] private Volume _volumeref;
    void Start()
    {
        StartCoroutine(looseControl());
    }
    private void FixedUpdate()
    {
        Vector3 direction=Vector3.zero;
        if(Input.GetKey(KeyCode.LeftArrow)||_aiRight)
                direction = ((new Vector3(0,0, 1).normalized * _speed) * Time.deltaTime);
        else if(Input.GetKey(KeyCode.RightArrow) ||_aiLeft)
                    direction = ((new Vector3(0, 0, -1).normalized * _speed) * Time.deltaTime);
        else
             direction = Vector3.zero;
        _rb.velocity = direction;
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, _minbz, _maxZ));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Collectable"))
            _lifeCounter.removeOneLife();
        else
        {
            if (GamaManager.instance != null)
                GamaManager.instance.Collectible++;
            Destroy(other.gameObject);
        }
    }
    IEnumerator looseControl()
    {
        yield return new WaitForSeconds(20f);
        DOTween.To(() => _camref.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView, x => _camref.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = x, 20, 60);
        yield return new WaitForSeconds(20f);
        
        _col.enabled = false;
        StartCoroutine(lookatplayerTimer());
        //DOTween.To(() => _camref.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView, x => _camref.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView=x, 10, 8);
        while (!_lookAtPlayer)
        {
            _aiRight = true;
            _aiLeft=false;
            yield return new WaitForSeconds(Random.Range(0.5f,3f));
            _aiRight = false;
            _aiLeft = true;
            yield return new WaitForSeconds(Random.Range(0.5f, 3f));
        }
        _camref.SetActive(false);
        yield return new WaitForSeconds(5f);
        if (GamaManager.instance!= null)
            GamaManager.instance.StopAudio();
        SceneManager.LoadScene("Lobby3D");
    }
    IEnumerator lookatplayerTimer()
    {
        DOTween.To(() => _volumeref.weight, x => _volumeref.weight = x, 1, 10f);

        yield return new WaitForSeconds(5f);

        _lookAtPlayer = true;
        //transform.LookAt(_camref.transform);
        //transform.Rotate(0, 180, 0);
    }
}
