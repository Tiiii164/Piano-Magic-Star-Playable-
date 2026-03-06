using UnityEngine;
using UnityEngine.UI;

public class BackGroundSetUp : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private Sprite _landscapeBg;
    [SerializeField] private Sprite _portraitBg;

    void Start()
    {
        SetupLayout();
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
}
