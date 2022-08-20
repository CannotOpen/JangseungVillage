using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UniRx;

public class ControllerTutorial : MonoBehaviour
{
    [SerializeField]
    private GameObject firstPage;
    [SerializeField]
    private GameObject normalModePage;
    [SerializeField]
    private GameObject speedModePage;

    [SerializeField]
    private Button explainNormalModeButton;
    [SerializeField]
    private Button explainSpeedModeButton;

    // [SerializeField]
    // private Button previousButton;

    // [SerializeField]
    // private Button nextButton;

    [SerializeField]
    private List<GameObject> normalModePages;

    private int nowIndex = 0;

    private void Start()
    {
        // DeactiveButton(previousButton);
        // ActiveButton(nextButton);

        explainNormalModeButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                firstPage.SetActive(false);
                normalModePage.SetActive(true);
            })
            .AddTo(gameObject);

        explainSpeedModeButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                firstPage.SetActive(false);
                speedModePage.SetActive(true);
            })
            .AddTo(gameObject);

        // previousButton.OnClickAsObservable()
        //     .Subscribe(_ =>
        //     {
        //         if (nowIndex <= 0)
        //         {
        //             return;
        //         }

        //         normalModePages[nowIndex--].SetActive(false);
        //         normalModePages[nowIndex].SetActive(true);

        //         if (nowIndex <= 0)
        //         {
        //             DeactiveButton(previousButton);
        //         }
        //         else
        //         {
        //             ActiveButton(nextButton);
        //         }
        //     })
        //     .AddTo(gameObject);

        // nextButton.OnClickAsObservable()
        //     .Subscribe(_ =>
        //     {
        //         if (nowIndex >= normalModePages.Count - 1)
        //         {
        //             return;
        //         }

        //         normalModePages[nowIndex++].SetActive(false);
        //         normalModePages[nowIndex].SetActive(true);

        //         if (nowIndex >= normalModePages.Count - 1)
        //         {
        //             DeactiveButton(nextButton);
        //         }
        //         else
        //         {
        //             ActiveButton(nextButton);
        //         }
        //     })
        //     .AddTo(gameObject);
    }

    private void ActiveButton(Button button)
    {
        var imageColor = button.GetComponent<Image>().color;
        
        imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, 0.5f);

        button.enabled = true;
    }

    private void DeactiveButton(Button button)
    {
        var imageColor = button.GetComponent<Image>().color;
        
        imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, 0.5f);

        button.enabled = false;
    }
}