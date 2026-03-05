using UnityEngine;
using UnityEngine.UI;

public class LaneLayoutController : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] private Image _background;
    [SerializeField] private Sprite _landscapeBg;
    [SerializeField] private Sprite _portraitBg;
    [SerializeField] private RectTransform _laneContainer;
    [SerializeField] private GameObject _linePrefab;

    [Header("Lane Settings")]
    [SerializeField] private int _laneCount = 4;
    [SerializeField] private float _lineWidth = 5f;

    [Header("Landscape Config")]
    [SerializeField] private float _maxLandscapeWidth = 800f;
    [SerializeField] private bool _limitLandscapeWidth = true;

    private RectTransform[] _lanes;

    private void Start()
    {
        SetupLayout();
        GenerateLanes();
    }

    private void SetupLayout()
    {
        float aspectRatio = (float)Screen.width / Screen.height;

        if (aspectRatio > 1f)
        {
            _background.sprite = _landscapeBg;
        }
        else
        {
            _background.sprite = _portraitBg;
        }
    }

    private void GenerateLanes()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        float containerWidth = _laneContainer.rect.width;

        float playAreaWidth = containerWidth;

        if (aspectRatio > 1f && _limitLandscapeWidth)
        {
            playAreaWidth = Mathf.Min(containerWidth, _maxLandscapeWidth);
        }

        float startX = (containerWidth - playAreaWidth) * 0.5f;
        float laneWidth = playAreaWidth / _laneCount;

        for (int i = 0; i <= _laneCount; i++)
        {
            GameObject line = Instantiate(_linePrefab, _laneContainer);
            RectTransform rect = line.GetComponent<RectTransform>();

            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 0.5f);

            rect.sizeDelta = new Vector2(_lineWidth, 0);

            float xPos = startX + laneWidth * i - 2;
            rect.anchoredPosition = new Vector2(xPos, 0);
        }
    }

    public Vector2 GetLaneCenterPosition(int laneIndex)
    {
        float containerWidth = _laneContainer.rect.width;
        float aspectRatio = (float)Screen.width / Screen.height;

        float playAreaWidth = containerWidth;

        if (aspectRatio > 1f && _limitLandscapeWidth)
        {
            playAreaWidth = Mathf.Min(containerWidth, _maxLandscapeWidth);
        }

        float startX = (containerWidth - playAreaWidth) * 0.5f;
        float laneWidth = playAreaWidth / _laneCount;

        float x = startX + laneWidth * laneIndex + laneWidth * 0.5f;
        return new Vector2(x, 0);
    }
}