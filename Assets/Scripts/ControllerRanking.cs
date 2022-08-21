using System.Collections;
using System.Collections.Generic;
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
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private LevelLoader levelLoader;

    private void Start()
    {
        homeButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                levelLoader.LoadScene(SceneName.Main);
            })
            .AddTo(gameObject);

        jangseungButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                Debug.Log($"[KHW] asdfasdf");
            })
            .AddTo(gameObject);

        scoreText.text = InGameMgr.Instance.score.ToString();
    }
}
