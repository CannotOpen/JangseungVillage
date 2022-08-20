using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using UniRx;

public class ControllerOption : MonoBehaviour
{
    private LevelLoader levelLoader;

    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private Button retryButton;

    [SerializeField]
    private Button continueButton;

    [SerializeField]
    private Button bgmControlButton;
    private Image bgmControlImage;

    [SerializeField]
    private Button sfxControlButton;
    private Image sfxControlImage;

    [SerializeField]
    private Sprite controlOnSprite;
    [SerializeField]
    private Sprite controlOffSprite;

    private void Awake()
    {
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        bgmControlImage = bgmControlButton.GetComponent<Image>();
        sfxControlImage = sfxControlButton.GetComponent<Image>();
    }

    private void Start()
    {
        bool isShowInGameButton = !string.Equals(SceneManager.GetActiveScene().name, SceneName.Main.ToString());

        retryButton.gameObject.SetActive(isShowInGameButton);
        continueButton.gameObject.SetActive(isShowInGameButton);

        bgmControlImage.sprite = AudioManager.Inst.IsMusicOn ? controlOnSprite : controlOffSprite;
        sfxControlImage.sprite = AudioManager.Inst.IsSoundOn ? controlOnSprite : controlOffSprite;

        closeButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                transform.parent.gameObject.SetActive(false);
            })
            .AddTo(gameObject);

        retryButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                levelLoader.LoadScene(SceneManager.GetActiveScene().name);
            })
            .AddTo(gameObject);

        continueButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                transform.parent.gameObject.SetActive(false);
            })
            .AddTo(gameObject);

        bgmControlButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                AudioManager.Inst.IsMusicOn = !AudioManager.Inst.IsMusicOn;
                bgmControlImage.sprite = AudioManager.Inst.IsMusicOn ? controlOnSprite : controlOffSprite;
            })
            .AddTo(gameObject);

        sfxControlButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                AudioManager.Inst.IsSoundOn = !AudioManager.Inst.IsSoundOn;
                sfxControlImage.sprite = AudioManager.Inst.IsSoundOn ? controlOnSprite : controlOffSprite;
            })
            .AddTo(gameObject);
    }
}
