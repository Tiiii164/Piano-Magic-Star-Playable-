using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LaneLayoutController _laneLayout;
    [SerializeField] private RectTransform _noteContainer;
    [SerializeField] private GameObject _shortNotePrefab;
    [SerializeField] private GameObject _longNotePrefab;
    [SerializeField] private AudioSource _audioSource;

    [Header("Config")]
    [SerializeField] private float _spawnOffsetTime = 2f;
    [SerializeField] private float _spawnHeight = 1000f;

    private List<NoteData> _notes;
    private int _nextNoteIndex = 0;

    public void Initialize(string midiRaw)
    {
        _notes = MidiParser.Parse(midiRaw);
        _nextNoteIndex = 0;
    }

    private void Update()
    {
        if (_notes == null || !_audioSource.isPlaying)
            return;

        float currentTime = _audioSource.time;

        while (_nextNoteIndex < _notes.Count &&
               _notes[_nextNoteIndex].TimeAppear <= currentTime + _spawnOffsetTime)
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

        rect.anchoredPosition = new Vector2(lanePos.x, _spawnHeight);

        NoteMovement movement = note.GetComponent<NoteMovement>();
        movement.Initialize(data.Duration);
    }
}