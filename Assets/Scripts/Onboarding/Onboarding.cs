using UnityEngine;
using UnityEngine.SceneManagement;

public class Onboarding : MonoBehaviour
{
    [SerializeField] private OnboardingView _view;
    [SerializeField] private ScreenVisabilityHandler _firstWindow;
    [SerializeField] private ScreenVisabilityHandler _secondWindow;
    [SerializeField] private ScreenVisabilityHandler _thirdWindow;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("Onboarding"))
            SceneManager.LoadScene("MainScene");
    }

    private void Start()
    {
        _view.EnableWindow(_firstWindow);
        _view.DisableWindow(_secondWindow);
        _view.DisableWindow(_thirdWindow);
    }

    private void OnEnable()
    {
        _view.FirstButtonClicked += ProcessFirstScreenButtonClicked;
        _view.SecondButtonClicked += ProcessSecondScreenButtonClicked;
        _view.ThridButtonClicked += ProcessThirdScreenButtonClicked;
    }

    private void OnDisable()
    {
        _view.FirstButtonClicked -= ProcessFirstScreenButtonClicked;
        _view.SecondButtonClicked -= ProcessSecondScreenButtonClicked;
        _view.ThridButtonClicked -= ProcessThirdScreenButtonClicked;
    }

    public void ProcessFirstScreenButtonClicked()
    {
        _view.DisableWindow(_firstWindow);
        _view.EnableWindow(_secondWindow);
        _view.DisableWindow(_thirdWindow);
    }

    private void ProcessSecondScreenButtonClicked()
    {
        _view.DisableWindow(_firstWindow);
        _view.DisableWindow(_secondWindow);
        _view.EnableWindow(_thirdWindow);
        
        PlayerPrefs.SetInt("Onboarding", 1);
    }

    private void ProcessThirdScreenButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
}
