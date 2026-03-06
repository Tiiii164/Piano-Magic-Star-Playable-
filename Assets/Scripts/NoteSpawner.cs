using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        StartCoroutine(HandAnimCoroutine(handRect, offset));
    }

    private IEnumerator HandAnimCoroutine(RectTransform handRect, Vector2 offset)
    {
        Vector2 startPos = handRect.anchoredPosition;
        Vector2 endPos = startPos + offset * 1.2f;
        float duration = 1f;

        while (true)
        {
            yield return MoveRect(handRect, startPos, endPos, duration);

            yield return MoveRect(handRect, endPos, startPos, duration);
        }
    }

    private IEnumerator MoveRect(RectTransform rect, Vector2 from, Vector2 to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = Mathf.Sin((t / duration) * Mathf.PI * 0.5f);
            rect.anchoredPosition = Vector2.Lerp(from, to, progress);
            yield return null;
        }
        rect.anchoredPosition = to;
    }


    private void SpawnNote(NoteData data)
    {
        GameObject prefab = data.IsLongNote ? _longNotePrefab : _shortNotePrefab;

        GameObject note = Instantiate(prefab, _noteContainer);
        RectTransform rect = note.GetComponent<RectTransform>();

        Vector2 lanePos = _laneLayout.GetLaneCenterPosition(data.LaneIndex);

        rect.anchoredPosition = new Vector2(lanePos.x, _spawnHeight);

        NoteMovement movement = note.GetComponent<NoteMovement>();
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