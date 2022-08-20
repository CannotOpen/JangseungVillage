using UnityEngine;
using UnityEngine.UI;

using UniRx;

[RequireComponent(typeof(Button))]
public class OptionButton : MonoBehaviour
{
    private Button optionButton;

    [SerializeField]
    private GameObject optionPanel;

    private void Awake()
    {
        optionButton = GetComponent<Button>();
    }

    private void Start()
    {
        optionButton.OnClickAsObservable()
            .Subscribe(_ =>
            {
                optionPanel.SetActive(true);
            })
            .AddTo(gameObject);
    }
}