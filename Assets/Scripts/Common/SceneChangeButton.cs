using UnityEngine;
using UnityEngine.UI;

using UniRx;

[RequireComponent(typeof(Button))]
public class SceneChangeButton : MonoBehaviour
{
    private Button uiButton;
    private LevelLoader levelLoader;

    [SerializeField]
    private SceneName destinationSceneName;

    private void Awake()
    {
        uiButton = GetComponent<Button>();
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
    }

    private void Start()
    {
        uiButton.OnClickAsObservable()
            .Subscribe(_ => 
            {
                // PlaySFX가 작동하지 않아 PlayOnShot을 사용 - Hyeonwoo, 2022.08.20.
                AudioManager.Inst.PlayOneShot(SoundName.SFX_UI_OnClick);
                levelLoader.LoadScene(destinationSceneName);
            })
            .AddTo(gameObject);
    }
}