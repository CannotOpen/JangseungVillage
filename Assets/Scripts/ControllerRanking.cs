using UnityEngine;
using UnityEngine.UI;

using UniRx;
using TMPro;

public class ControllerRanking : MonoBehaviour
{
    [SerializeField]
    private Button homeButton;

    [SerializeField]
    private Button jangseungButton;

    [SerializeField]
    private ScrollScript_2d scroll;

    [SerializeField]
    private TextMeshProUGUI explainText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private ScrollRect scrollRect;

    [SerializeField]
    private LevelLoader levelLoader;

    private void Start()
    {
        scrollRect.onValueChanged.AddListener(HandlOnValueChanged);

        scroll.animate();

        homeButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                levelLoader.LoadScene(SceneName.Main);
            })
            .AddTo(gameObject);

        jangseungButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                levelLoader.LoadScene(SceneName.InGame);
            })
            .AddTo(gameObject);

        var randomNumber = UnityEngine.Random.Range(0, 10);

        switch (randomNumber)
        {
        case 9:
            explainText.text = "생각하고 치셨나요?";
            break;
        case 8:
            explainText.text = "올해의 박치왕";
            break;
        case 7:
            explainText.text = "해줄 말도 없네요";
            break;
        case 6:
            explainText.text = "보는 내가 다 아깝네! 한 번 더?";
            break;
        case 5:
            explainText.text = "제 표정 어때보여요?";
            break;
        case 4:
            explainText.text = "생각보다 『평범』하군요";
            break;
        case 3:
            explainText.text = "우월한 가락유전자";
            break;
        case 2:
            explainText.text = "웃기는 박자왕";
            break;
        case 1:
            explainText.text = "상상도 못한 점수! ㄴㅇㄱ";
            break;
        default:
            explainText.text = "충청의 딸, 허지향 화이팅";
            break;
        }
        
        scoreText.text = PlayerPrefs.GetInt("SCORE_KEY", 0).ToString();
    }

    private void HandlOnValueChanged(Vector2 value)
    {
        if (value.y > 1.3f)
        {
            levelLoader.LoadScene(SceneName.InGame);
        }
    }
}
