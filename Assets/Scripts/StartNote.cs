using UnityEngine;

public class StartNote : MonoBehaviour
{
    public void OnClick()
    {
        NoteSpawner.Instance.StartGameplay();
        gameObject.SetActive(false);
    }
}
