using UnityEngine;
using UnityEngine.UI;

using UniRx;

public class ControllerMain : MonoBehaviour
{
    [SerializeField]
    private Animator mainAnimator;

    [SerializeField]
    private Button pressToStartButton;

    [SerializeField]
    private Button tutorialButton;

    [SerializeField]
    private GameObject tutorialPanel;

    private void Start()
    {
        AudioManager.Inst.PlayBGM(AudioManager.Inst.GetClipFromPlaylist(SoundName.title.ToString()));

        pressToStartButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                mainAnimator.SetTrigger("TouchScreen");
            })
            .AddTo(gameObject);

        tutorialButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                tutorialPanel.SetActive(true);
            })
            .AddTo(gameObject);
    }
}