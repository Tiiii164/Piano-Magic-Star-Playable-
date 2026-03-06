using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField] private TextAsset _midiData;
    [Header("References")]
    [SerializeField] private LaneLayoutController _laneLayout;
    [SerializeField] private RectTransform _noteContainer;
    [SerializeField] private GameObject _shortNotePrefab;
    [SerializeField] private GameObject _longNotePrefab;
    [SerializeField] private GameObject _startNotePrefab;
    [SerializeField] private GameObject _handPrefab;
    [SerializeField] private GameObject _highScorePrefab;
    [SerializeField] private GameObject _tapToStartPrefab;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _endAudioSource;
    [SerializeField] private GameObject _blackBackground;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [Header("Config")]
    [SerializeField] private float _spawnOffsetTime = 1f;
    [SerializeField] private float _spawnHeight = 1000f;

    public static NoteSpawner Instance { get; private set; }

    private List<NoteData> _notes;
    private int _nextNoteIndex = 0;
    private float _gameplayTime = 0f;
    //private bool _audioStarted = false;
    public bool _isPlaying = false;
    private int _score = 0;

    private void Start()
    {
        Initialize(_midiData.text);
        SpawnStartNote();
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Update()
    {
        if (!_isPlaying) 
            return;

        _gameplayTime += Time.deltaTime;

        while (_nextNoteIndex < _notes.Count &&
               _notes[_nextNoteIndex].TimeAppear <= _gameplayTime + _spawnOffsetTime)
        {
            SpawnNote(_notes[_nextNoteIndex]);
            _nextNoteIndex++;
        }
    }
    public void StartGameplay()
    {
        _isPlaying = true;
        StartCoroutine(DelayAudioStart());
        _gameplayTime = 0f;
        _nextNoteIndex = 0;
        _handPrefab.SetActive(false);
        _highScorePrefab.SetActive(false);
        _tapToStartPrefab.SetActive(false);
    }

    public void StopGameplay()
    {
        _isPlaying = false;
        _audioSource.Stop();
        _endAudioSource.Play();
        _blackBackground.SetActive(true);
        StartCoroutine(DelayEndGame());
        Debug.Log("Game Over! Final Score: " + _score);
    }

    private IEnumerator DelayEndGame()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("EndMenu");
    }

    private IEnumerator DelayAudioStart()
    {
        yield return new WaitForSeconds(1);
        _audioSource.Play();
    }

    private void Initialize(string midiRaw)
    {
        _notes = MidiParser.Parse(midiRaw);
        _nextNoteIndex = 0;
        _gameplayTime = 0f;
    }

    private void SpawnStartNote()
    {
        GameObject note = Instantiate(_startNotePrefab, _noteContainer);
        RectTransform rect = note.GetComponent<RectTransform>();
        Vector2 lanePos = _laneLayout.GetLaneCenterPosition(2);
        rect.anchoredPosition = new Vector2(lanePos.x, -200);
        HandAnim(rect.anchoredPosition);
    }

    private void HandAnim(Vector2 startNotePos)
    {
        _handPrefab.SetActive(true);
        Vector2 offset = new Vector2(150, -190);

        RectTransform handRect = _handPrefab.GetComponent<RectTransform>();
        handRect.anchoredPosition = startNotePos + offset;
        handRect.DOAnchorPos(handRect.anchoredPosition + offset * 1.2f, 1)
                 .SetEase(Ease.OutSine)
                 .SetLoops(-1, LoopType.Yoyo);
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
    public void AddScore(int amount)
    {
        if(!_scoreText.IsActive()) 
            _scoreText.gameObject.SetActive(true);

        _score += amount;
        _scoreText.text = _score.ToString();
        Debug.Log(_score);
    }
}