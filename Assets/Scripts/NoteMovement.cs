using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 600f;
    [SerializeField] private GameObject afterTabImage;

    private RectTransform _rect;
    private bool _clicked = false;
    private int _scoreValue = 3;
    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void Initialize(float duration)
    {
        
    }

    public void ShowAfterTabImage()
    {
        afterTabImage.SetActive(true);
        _clicked = true;
        NoteSpawner.Instance.AddScore(_scoreValue);
    }


    private void Update()
    {
        if (NoteSpawner.Instance._isPlaying)
        {
            _rect.anchoredPosition += Vector2.down * _speed * Time.deltaTime;
        }

        if (_rect.anchoredPosition.y < -1200f)
        {
            Destroy(gameObject);
        }
        if (_rect.anchoredPosition.y < -700f && !_clicked && NoteSpawner.Instance._isPlaying)
        {
            NoteSpawner.Instance.StopGameplay();
        }

    }
}