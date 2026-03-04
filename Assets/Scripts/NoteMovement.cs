using UnityEngine;

public class NoteMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 600f;

    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void Initialize(float duration)
    {
        
    }

    private void Update()
    {
        _rect.anchoredPosition += Vector2.down * _speed * Time.deltaTime;

        if (_rect.anchoredPosition.y < -1200f)
        {
            Destroy(gameObject);
        }
    }
}