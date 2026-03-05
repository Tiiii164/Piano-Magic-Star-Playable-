using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private TextAsset _midiData;
    [Header("References")]
    [SerializeField] private LaneLayoutController _laneLayout;
    [SerializeField] private RectTransform _noteContainer;
    [SerializeField] private GameObject _shortNotePrefab;
    [SerializeField] private GameObject _longNotePrefab;
    [SerializeField] private AudioSource _audioSource;

    [Header("Config")]
    [SerializeField] private float _spawnOffsetTime = 1f;
    [SerializeField] private float _spawnHeight = 1000f;

    private List<NoteData> _notes;
    private int _nextNoteIndex = 0;
    private float _gameplayTime = 0f;
    private bool _audioStarted = false;

    private void Start()
    {
        Initialize(_midiData.text);
        StartCoroutine(DelayAudioStart());
    }

    private IEnumerator DelayAudioStart()
    {
        yield return new WaitForSeconds(1);
        _audioSource.Play();
        _audioStarted = true;
    }

    private void Initialize(string midiRaw)
    {
        _notes = MidiParser.Parse(midiRaw);
        _nextNoteIndex = 0;
        _gameplayTime = 0f;
    }

    private void Update()
    {
        _gameplayTime += Time.deltaTime;

        while (_nextNoteIndex < _notes.Count &&
               _notes[_nextNoteIndex].TimeAppear <= _gameplayTime + _spawnOffsetTime)
        {
            SpawnNote(_notes[_nextNoteIndex]);
            _nextNoteIndex++;
        }
    }

    private void SpawnNote(NoteData data)
    {
        GameObject prefab = data.IsLongNote ? _longNotePrefab : _shortNotePrefab;

        GameObject note = Instantiate(prefab, _noteContainer);
        RectTransform rect = note.GetComponent<RectTransform>();

        Vector2 lanePos = _laneLayout.GetLaneCenterPosition(data.LaneIndex);
        Debug.Log($"Spawning note in lane {data.LaneIndex} at position {lanePos.x}");

        rect.anchoredPosition = new Vector2(lanePos.x, _spawnHeight);

        NoteMovement movement = note.GetComponent<NoteMovement>();
        movement.Initialize(data.Duration);
    }
}