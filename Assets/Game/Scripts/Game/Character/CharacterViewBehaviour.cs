using com;
using UnityEngine;
using System;
using System.Collections;

public class CharacterViewBehaviour : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnInfo
    {
        public GameObject prefab;
        public Vector3 offset;
    }

    [SerializeField] Transform _view;
    [SerializeField] SpawnInfo[] _prefabs;
    [SerializeField] string _soundName;
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] GameObject[] _toShows;

    Vector3 _viewOffset;

    private void Awake()
    {
        _viewOffset = _view.localPosition;
    }

    public void SyncView()
    {
        if (_sr.flipX)
        {
            _view.localScale = new Vector3(-1, 1, 1);
            _view.localPosition = new Vector3(-_viewOffset.x, _viewOffset.y, _viewOffset.z);
        }
        else
        {
            _view.localScale = new Vector3(1, 1, 1);
            _view.localPosition = new Vector3(_viewOffset.x, _viewOffset.y, _viewOffset.z);
        }
    }

    public Vector3 viewPos { get { return _view.position; } }

    public void TriggerSpawn(float externalDestroyTime = 5)
    {
        foreach (var p in _prefabs)
        {
            var pos = _view.position;
            if (_sr.flipX)
                pos += p.offset;
            else
                pos += new Vector3(-p.offset.x, p.offset.y, p.offset.z);

            var s = Instantiate(p.prefab, pos, Quaternion.identity);
            if (_sr.flipX)
                s.transform.localScale = new Vector3(-s.transform.localScale.x, s.transform.localScale.y, s.transform.localScale.z);

            if (externalDestroyTime > 0)
                Destroy(s.gameObject, externalDestroyTime);
        }

        SoundSystem.instance.Play(_soundName);
    }

    public void OnFire()
    {
        SyncView();
        foreach (var ts in _toShows)
            ts.SetActive(true);

        TriggerSpawn();
        StartCoroutine(DelayAction(0.1f, TurnOff));
    }

    void TurnOff()
    {
        foreach (var ts in _toShows)
            ts.SetActive(false);
    }

    IEnumerator DelayAction(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}