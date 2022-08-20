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
        AudioManager.Inst.PlaySFX(SoundName.SFX_Jing);
        AudioManager.Inst.PlayBGM(AudioManager.Inst.GetClipFromPlaylist(SoundName.BGM_Title.ToString()));

        pressToStartButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                AudioManager.Inst.PlaySFX(SoundName.SFX_UI_OnClick);
                mainAnimator.SetTrigger("TouchScreen");
            })
            .AddTo(gameObject);

        tutorialButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                AudioManager.Inst.PlaySFX(SoundName.SFX_UI_OnClick);
                tutorialPanel.SetActive(true);
            })
            .AddTo(gameObject);
    }
}