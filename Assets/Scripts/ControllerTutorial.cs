using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;

public class ControllerTutorial : MonoBehaviour
{
    [Header("Common")]
    [SerializeField]
    private Button closeButton;

    #region First

    [Header("First")]
    [SerializeField]
    private GameObject firstPage;

    [SerializeField]
    private Button explainNormalModeButton;

    [SerializeField]
    private Button explainSpeedModeButton;

    #endregion

    #region Normal

    [Header("Normal")]
    [SerializeField]
    private List<GameObject> normalModePages;

    [SerializeField]
    private GameObject normalModePagePanel;

    [SerializeField]
    private Button previousButton;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Button goToSpeedModeButton;

    #endregion

    #region Speed Mode

    [Header("Speed Mode")]
    [SerializeField]
    private GameObject speedModePagePanel;

    [SerializeField]
    private Button goToNormalModeButton;

    #endregion

    private int nowIndex = 0;

    private void Start()
    {
        DeactiveButton(previousButton);
        ActiveButton(nextButton);

        explainNormalModeButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                firstPage.SetActive(false);
                normalModePagePanel.SetActive(true);
            })
            .AddTo(gameObject);

        explainSpeedModeButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                firstPage.SetActive(false);
                speedModePagePanel.SetActive(true);
            })
            .AddTo(gameObject);

        previousButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                if (nowIndex <= 0)
                {
                    return;
                }

                normalModePages[nowIndex--].SetActive(false);
                normalModePages[nowIndex].SetActive(true);

                if (nowIndex <= 0)
                {
                    DeactiveButton(previousButton);
                }
                else
                {
                    ActiveButton(previousButton);
                    ActiveButton(nextButton);
                }
            })
            .AddTo(gameObject);

        nextButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                if (nowIndex >= normalModePages.Count - 1)
                {
                    return;
                }

                normalModePages[nowIndex++].SetActive(false);
                normalModePages[nowIndex].SetActive(true);

                if (nowIndex >= normalModePages.Count - 1)
                {
                    DeactiveButton(nextButton);
                }
                else
                {
                    ActiveButton(previousButton);
                    ActiveButton(nextButton);
                }
            })
            .AddTo(gameObject);

        closeButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                gameObject.SetActive(false);
            })
            .AddTo(gameObject);

        goToSpeedModeButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                normalModePagePanel.SetActive(false);
                speedModePagePanel.SetActive(true);
            })
            .AddTo(gameObject);

        goToNormalModeButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                normalModePagePanel.SetActive(true);
                speedModePagePanel.SetActive(false);
            })
            .AddTo(gameObject);
    }

    private void OnEnable()
    {
        firstPage.SetActive(true);
        normalModePagePanel.SetActive(false);
        speedModePagePanel.SetActive(false);

        nowIndex = 0;

        DeactiveButton(previousButton);
        ActiveButton(nextButton);
    }

    private void OnDisable()
    {
        normalModePages[0].SetActive(true);

        for (int i = 1; i < normalModePages.Count; i++)
        {
            normalModePages[i].SetActive(false);
        }
    }

    private void ActiveButton(Button button)
    {
        var imageColor = button.GetComponent<Image>().color;
        
        button.GetComponent<Image>().color = new Color(imageColor.r, imageColor.g, imageColor.b, 1f);

        button.enabled = true;
    }

    private void DeactiveButton(Button button)
    {
        var imageColor = button.GetComponent<Image>().color;
        
        button.GetComponent<Image>().color = new Color(imageColor.r, imageColor.g, imageColor.b, 0.5f);

        button.enabled = false;
    }
}