using UnityEngine;
using UnityEngine.UI;

public class LaneLayoutController : MonoBehaviour
{
    [Header("Layout")]
    [SerializeField] private Image _background;
    [SerializeField] private Sprite _landscapeBg;
    [SerializeField] private Sprite _portraitBg;

    [Header("References")]
    [SerializeField] private RectTransform _laneContainer;
    [SerializeField] private RectTransform[] _lanes;

    [Header("Landscape Settings (16:9)")]
    [SerializeField] private float _landscapeLaneWidth = 200f;

    private void Start()
    {
        SetupLayout();
    }

    private void SetupLayout()
    {
        float aspectRatio = (float)Screen.width / Screen.height;

        if (aspectRatio > 1f)
        {
            SetupLandscape();
        }
        else
        {
            SetupPortrait();
        }
    }

    private void SetupLandscape()
    {
        float totalWidth = _landscapeLaneWidth * _lanes.Length;

        //_laneContainer.SetSizeWithCurrentAnchors(
        //    RectTransform.Axis.Horizontal,
        //    totalWidth);
        _background.sprite = _landscapeBg;


        for (int i = 0; i < _lanes.Length; i++)
        {
            _lanes[i].SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                _landscapeLaneWidth);
        }
    }

    private void SetupPortrait()
    {
        float screenWidth = _laneContainer.rect.width;
        float laneWidth = screenWidth / _lanes.Length;

        _background.sprite = _portraitBg;
        for (int i = 0; i < _lanes.Length; i++)
        {
            _lanes[i].SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                laneWidth);
        }
    }
}