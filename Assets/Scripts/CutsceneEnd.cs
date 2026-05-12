using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneEnd : MonoBehaviour
{
    public PlayableDirector director;
    public string sceneAfterCutscene = "MainMenu";

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.stopped += OnCutsceneEnd;
    }

    void OnCutsceneEnd(PlayableDirector pd)
    {
        SceneManager.LoadScene(sceneAfterCutscene);
    }

    void OnDestroy()
    {
        if (director != null)
            director.stopped -= OnCutsceneEnd;
    }
}