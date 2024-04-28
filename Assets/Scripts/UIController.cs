using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.CameraUI;

public class UIController : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;

    VisualElement mainMenuPanel;
    VisualElement settingsPanel;
    VisualElement gamePanel;
    VisualElement endGamePanel;
    Button mainSettingsBtn;
    Button mainStartBtn;
    Button settingsGridSelect4Btn;
    Button settingsGridSelect6Btn;
    Button settingsRoundTimeDecreaseBtn;
    Button settingsRoundTimeIncreaseBtn;
    Label settingsRoundTimeInput;
    Button settingsRoundDecreaseBtn;
    Button settingsRoundIncreaseBtn;
    Label settingsRoundInput;
    Button settingsCloseBtn;
    Button settingsCancelBtn;
    Button settingsStartBtn;

    Label gameRoundLabel;
    Label endGamePanelRoundLabel;
    VisualElement gamePlayer1PointBox;
    VisualElement gamePlayer2PointBox;
    Label gamePlayer1Point;
    Label gamePlayer2Point;
    VisualElement gamePlayer1ScoreBox;
    VisualElement gamePlayer2ScoreBox;
    Label gamePlayer1Score;
    Label gamePlayer2Score;
    VisualElement gamePlayer1Avatar;
    VisualElement gamePlayer1AvatarBack;
    VisualElement gamePlayer2Avatar;
    VisualElement gamePlayer2AvatarBack;
    Label gamePlayer1Label;
    Label gamePlayer2Label;

    Label endGamePlayer1ScoreOutput;
    Label endGamePlayer2ScoreOutput;
    Label endGameGameOverLabel;
    Label endGameWinnerInfoLabel;
    Button endGameExitBtn;
    Button endGameNextRoundBtn;

    VisualElement endGamePlayer1Avatar;
    VisualElement endGamePlayer1AvatarBack;
    VisualElement endGamePlayer2Avatar;
    VisualElement endGamePlayer2AvatarBack;
    VisualElement endGamePlayer1ScoreBox;
    VisualElement endGamePlayer2ScoreBox;
    Label endGamePlayer1Label;
    Label endGamePlayer2Label;

    VisualElement settingsGrid4Selected;
    VisualElement settingsGrid6Selected;

    void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        //Panels
        mainMenuPanel = root.Q<VisualElement>("MainMenu-Panel");
        settingsPanel = root.Q<VisualElement>("Settings-Panel");
        gamePanel = root.Q<VisualElement>("Game-Panel");
        endGamePanel = root.Q<VisualElement>("EndGame-Panel");

        //MainMenu-Panel
        mainSettingsBtn = mainMenuPanel.Q<Button>("Settings-Btn");
        mainStartBtn = mainMenuPanel.Q<Button>("Start-Btn");

        //Settings-Panel
        settingsGridSelect4Btn = settingsPanel.Q<Button>("4x4-Grid-Select-Btn");
        settingsGridSelect6Btn = settingsPanel.Q<Button>("6x6-Grid-Select-Btn");
        settingsGrid4Selected = settingsPanel.Q<VisualElement>("Grid-4x4-Select");
        settingsGrid6Selected = settingsPanel.Q<VisualElement>("Grid-6x6-Select");
        settingsRoundTimeDecreaseBtn = settingsPanel.Q<Button>("Round-Time-Decrease-Btn");
        settingsRoundTimeIncreaseBtn = settingsPanel.Q<Button>("Round-Time-Increase-Btn");
        settingsRoundTimeInput = settingsPanel.Q<Label>("Round-Time-Input");
        settingsRoundDecreaseBtn = settingsPanel.Q<Button>("Round-Decrease-Btn");
        settingsRoundIncreaseBtn = settingsPanel.Q<Button>("Round-Increase-Btn");
        settingsRoundInput = settingsPanel.Q<Label>("Round-Input");
        settingsCloseBtn = settingsPanel.Q<Button>("Settings-Close-Btn");
        settingsCancelBtn = settingsPanel.Q<Button>("Settings-Cancel-Btn");
        settingsStartBtn = settingsPanel.Q<Button>("Settings-Start-Btn");

        //Game-Panel
        gameRoundLabel = gamePanel.Q<Label>("Game-Panel-Round-Label");
        gamePlayer1PointBox = gamePanel.Q<VisualElement>("Player1-Point-Box");
        gamePlayer2PointBox = gamePanel.Q<VisualElement>("Player2-Point-Box");
        gamePlayer1Point = gamePanel.Q<Label>("Player1-Point");
        gamePlayer2Point = gamePanel.Q<Label>("Player2-Point");
        gamePlayer1ScoreBox = gamePanel.Q<VisualElement>("Player1-Score-Box");
        gamePlayer2ScoreBox = gamePanel.Q<VisualElement>("Player2-Score-Box");
        gamePlayer1Score = gamePanel.Q<Label>("Player1-Score");
        gamePlayer2Score = gamePanel.Q<Label>("Player2-Score");
        gamePlayer1Avatar = gamePanel.Q<VisualElement>("Player1-Avatar");
        gamePlayer1AvatarBack = gamePanel.Q<VisualElement>("Player1-Avatar-Back");
        gamePlayer2Avatar = gamePanel.Q<VisualElement>("Player2-Avatar");
        gamePlayer2AvatarBack = gamePanel.Q<VisualElement>("Player2-Avatar-Back");
        gamePlayer1Label = gamePanel.Q<Label>("Player1-Label");
        gamePlayer2Label = gamePanel.Q<Label>("Player2-Label");

        //EndGame-Panel
        endGamePanelRoundLabel = endGamePanel.Q<Label>("EndGame-Panel-Round-Label");
        endGameGameOverLabel = endGamePanel.Q<Label>("Game-Over-Label");
        endGameWinnerInfoLabel = endGamePanel.Q<Label>("Winner-Label");
        endGamePlayer1ScoreOutput = endGamePanel.Q<Label>("Player1-Score-Output");
        endGamePlayer2ScoreOutput = endGamePanel.Q<Label>("Player2-Score-Output");
        endGameExitBtn = endGamePanel.Q<Button>("EndGame-Exit-Btn");
        endGameNextRoundBtn = endGamePanel.Q<Button>("EndGame-Next-Round-Btn");
        endGamePlayer1Avatar = endGamePanel.Q<VisualElement>("EndGame-Player1-Avatar");
        endGamePlayer1AvatarBack = endGamePanel.Q<VisualElement>("EndGame-Player1-Avatar-Back");
        endGamePlayer2Avatar = endGamePanel.Q<VisualElement>("EndGame-Player2-Avatar");
        endGamePlayer2AvatarBack = endGamePanel.Q<VisualElement>("EndGame-Player2-Avatar-Back");
        endGamePlayer1ScoreBox = endGamePanel.Q<VisualElement>("EndGame-Player1-ScoreBox");
        endGamePlayer2ScoreBox = endGamePanel.Q<VisualElement>("EndGame-Player2-ScoreBox");
        endGamePlayer1Label = endGamePanel.Q<Label>("EndGame-Player1-Label");
        endGamePlayer2Label = endGamePanel.Q<Label>("EndGame-Player2-Label");
    }

    void OnEnable()
    {
        CardManager.Instance.OnAllCardsOpened += OnAllCardsOpenedEvent;
        CardManager.Instance.OnRoundStarted += OnRoundStartedEvent;
        playerManager.OnAnyPlayersPointIncreased += OnAnyPlayersPointIncreasedEvent;
        playerManager.OnPlayerTurnChangedIsPlayer1Turn += OnPlayerTurnChangedIsPlayer1TurnEvent;

        //MainMenu-Panel
        mainSettingsBtn.RegisterCallback<ClickEvent>(OnSettingsBtnClicked);
        mainStartBtn.RegisterCallback<ClickEvent>(OnStartBtnClicked);

        //Settings-Panel
        settingsGridSelect4Btn.clicked += () => OnGridSizeSelectBtnClicked(4);
        settingsGridSelect6Btn.clicked += () => OnGridSizeSelectBtnClicked(6);
        settingsRoundTimeDecreaseBtn.RegisterCallback<ClickEvent>(OnRoundTimeDecreaseBtnClicked);
        settingsRoundTimeIncreaseBtn.RegisterCallback<ClickEvent>(OnRoundTimeInreaseBtnClicked);
        settingsRoundDecreaseBtn.RegisterCallback<ClickEvent>(OnRoundDecreaseBtnClicked);
        settingsRoundIncreaseBtn.RegisterCallback<ClickEvent>(OnRoundIncreaseBtnClicked);
        settingsCloseBtn.RegisterCallback<ClickEvent>(OnSettingsCloseBtnClicked);
        settingsCancelBtn.RegisterCallback<ClickEvent>(OnSettingsCancelBtnClicked);
        settingsStartBtn.RegisterCallback<ClickEvent>(OnStartBtnClicked);

        //Game-Panel

        //EndGame-Panel
        endGameExitBtn.RegisterCallback<ClickEvent>(OnEndGameExitBtnClicked);
        endGameNextRoundBtn.RegisterCallback<ClickEvent>(OnEndGameNextRoundBtnClicked);
    }

    int _selectedGridSize = 4;
    int _selectedRoundTime = 2;
    int _selectedRoundCount = 3;

    void OnGridSizeSelectBtnClicked(int gridSize)
    {
        if (gridSize == 4)
        {
            _selectedGridSize = 4;
            settingsGrid6Selected.style.display = DisplayStyle.None;
            settingsGrid4Selected.style.display = DisplayStyle.Flex;
        }
        else if (gridSize == 6)
        {
            _selectedGridSize = 6;
            settingsGrid4Selected.style.display = DisplayStyle.None;
            settingsGrid6Selected.style.display = DisplayStyle.Flex;
        }
    }

    void OnSettingsBtnClicked(ClickEvent e)
    {
        HideAllPanels();
        settingsPanel.style.display = DisplayStyle.Flex;
    }

    void OnRoundTimeDecreaseBtnClicked(ClickEvent e)
    {
        _selectedRoundTime--;
        if (_selectedRoundTime < 1)
        {
            _selectedRoundTime = 1;
        }

        UpdateRoundTimeLabel();
    }

    void OnRoundTimeInreaseBtnClicked(ClickEvent e)
    {
        _selectedRoundTime++;
        if (_selectedRoundTime > 5)
        {
            _selectedRoundTime = 5;
        }

        UpdateRoundTimeLabel();
    }

    void UpdateRoundTimeLabel()
    {
        settingsRoundTimeInput.text = _selectedRoundTime.ToString();
    }

    void OnRoundDecreaseBtnClicked(ClickEvent e)
    {
        _selectedRoundCount--;
        if (_selectedRoundCount < 1)
        {
            _selectedRoundCount = 1;
        }

        UpdateRoundInput();
    }

    void OnRoundIncreaseBtnClicked(ClickEvent e)
    {
        _selectedRoundCount++;
        if (_selectedRoundCount > 5)
        {
            _selectedRoundCount = 5;
        }

        UpdateRoundInput();
    }

    void UpdateRoundInput()
    {
        settingsRoundInput.text = _selectedRoundCount.ToString();
    }

    void OnSettingsCloseBtnClicked(ClickEvent e)
    {
        HideAllPanels();
        mainMenuPanel.style.display = DisplayStyle.Flex;
    }

    void OnSettingsCancelBtnClicked(ClickEvent e)
    {
        HideAllPanels();
        mainMenuPanel.style.display = DisplayStyle.Flex;
    }

    void OnStartBtnClicked(ClickEvent e)
    {
        HideAllPanels();
        gamePanel.style.display = DisplayStyle.Flex;

        playerManager.SetRounds(_selectedRoundTime, _selectedRoundCount);
        CardManager.Instance.CreateCards(_selectedGridSize);
    }

    void OnEndGameExitBtnClicked(ClickEvent e)
    {
        HideAllPanels();
        mainMenuPanel.style.display = DisplayStyle.Flex;
    }

    void OnEndGameNextRoundBtnClicked(ClickEvent e)
    {
        HideAllPanels();
        gamePanel.style.display = DisplayStyle.Flex;

        CardManager.Instance.GoNextRound();
    }

    void HideAllPanels()
    {
        mainMenuPanel.style.display = DisplayStyle.None;
        settingsPanel.style.display = DisplayStyle.None;
        gamePanel.style.display = DisplayStyle.None;
        endGamePanel.style.display = DisplayStyle.None;
    }

    void OnRoundStartedEvent()
    {
        UpdateRoundLabel();
        RefreshAllPlayerUIs();
    }

    void UpdateRoundLabel()
    {
        gameRoundLabel.text = $"Round: {playerManager.CurrentRound}/{playerManager.TotalRound}";
        endGamePanelRoundLabel.text = $"Round: {playerManager.CurrentRound}/{playerManager.TotalRound}";
    }

    void OnAllCardsOpenedEvent()
    {
        StartCoroutine(GoToNextPanelInSeconds(endGamePanel, 1f));
    }

    void OnAnyPlayersPointIncreasedEvent(int player1Point, int player2Point)
    {
        UpdatePlayerPoints(player1Point, player2Point);
    }

    void UpdatePlayerPoints(int player1Point, int player2Point)
    {
        gamePlayer1Point.text = $"Point: {player1Point}";
        gamePlayer2Point.text = $"Point: {player2Point}";
    }

    IEnumerator GoToNextPanelInSeconds(VisualElement panel, float duration)
    {
        yield return new WaitForSeconds(duration);
        HideAllPanels();
        panel.style.display = DisplayStyle.Flex;

        RefreshPlayerPointUIs();
        ArrangePanelUI(panel);
    }

    void ArrangePanelUI(VisualElement visualElement)
    {
        switch (visualElement)
        {
            case VisualElement _ when visualElement == endGamePanel:

                DisableEndGamePlayersUI();

                if (playerManager.Player1Score != playerManager.Player2Score)
                {
                    int winnerPlayerNum;

                    if (playerManager.Player1Score > playerManager.Player2Score)
                    {
                        winnerPlayerNum = 1;
                        EnableEndGamePlayerUI(true);
                    }
                    else
                    {
                        winnerPlayerNum = 2;
                        EnableEndGamePlayerUI(false);
                    }

                    endGameWinnerInfoLabel.text = $"WINNER IS PLAYER {winnerPlayerNum}";
                }
                else
                {
                    endGameWinnerInfoLabel.text = $"GAME ENDED IN A DRAW";
                }

                gamePlayer1Score.text = $"Score: {playerManager.Player1Score}";
                gamePlayer2Score.text = $"Score: {playerManager.Player2Score}";

                endGamePlayer1ScoreOutput.text = $"Score: {playerManager.Player1Score}";
                endGamePlayer2ScoreOutput.text = $"Score: {playerManager.Player2Score}";

                var allRoundsEnded = playerManager.CurrentRound >= playerManager.TotalRound;
                endGameGameOverLabel.style.display = allRoundsEnded ? DisplayStyle.Flex : DisplayStyle.None;
                endGameNextRoundBtn.text = allRoundsEnded ? "NEW GAME" : "NEXT ROUND";
                if (allRoundsEnded)
                {
                    playerManager.ResetPlayers();
                    RefreshAllPlayerUIs();
                }
                break;
            default:

                break;
        }
    }

    void RefreshPlayerPointUIs()
    {
        gamePlayer1Point.text = $"Point: {playerManager.Player1Point}";
        gamePlayer2Point.text = $"Point: {playerManager.Player2Point}";
    }

    void RefreshAllPlayerUIs()
    {
        gamePlayer1Point.text = $"Point: {playerManager.Player1Point}";
        gamePlayer2Point.text = $"Point: {playerManager.Player2Point}";
        gamePlayer1Score.text = $"Score: {playerManager.Player1Score}";
        gamePlayer2Score.text = $"Score: {playerManager.Player2Score}";
    }

    void OnPlayerTurnChangedIsPlayer1TurnEvent(bool isPlayer1Turn)
    {
        if (isPlayer1Turn)
        {
            gamePlayer2PointBox.AddToClassList("disabledPlayerUI");
            gamePlayer2ScoreBox.AddToClassList("disabledPlayerUI");
            gamePlayer2Avatar.AddToClassList("disabledPlayerUI");
            gamePlayer2AvatarBack.AddToClassList("disabledPlayerUI");
            gamePlayer2Label.AddToClassList("disabledPlayerText");

            gamePlayer1PointBox.RemoveFromClassList("disabledPlayerUI");
            gamePlayer1ScoreBox.RemoveFromClassList("disabledPlayerUI");
            gamePlayer1Avatar.RemoveFromClassList("disabledPlayerUI");
            gamePlayer1AvatarBack.RemoveFromClassList("disabledPlayerUI");
            gamePlayer1Label.RemoveFromClassList("disabledPlayerText");
        }
        else
        {
            gamePlayer1PointBox.AddToClassList("disabledPlayerUI");
            gamePlayer1ScoreBox.AddToClassList("disabledPlayerUI");
            gamePlayer1Avatar.AddToClassList("disabledPlayerUI");
            gamePlayer1AvatarBack.AddToClassList("disabledPlayerUI");
            gamePlayer1Label.AddToClassList("disabledPlayerText");

            gamePlayer2PointBox.RemoveFromClassList("disabledPlayerUI");
            gamePlayer2ScoreBox.RemoveFromClassList("disabledPlayerUI");
            gamePlayer2Avatar.RemoveFromClassList("disabledPlayerUI");
            gamePlayer2AvatarBack.RemoveFromClassList("disabledPlayerUI");
            gamePlayer2Label.RemoveFromClassList("disabledPlayerText");
        }
    }

    void DisableEndGamePlayersUI()
    {
        endGamePlayer2Avatar.AddToClassList("disabledPlayerUI");
        endGamePlayer2AvatarBack.AddToClassList("disabledPlayerUI");
        endGamePlayer2ScoreBox.AddToClassList("disabledPlayerUI");
        endGamePlayer2Label.AddToClassList("disabledPlayerText");

        endGamePlayer1Avatar.AddToClassList("disabledPlayerUI");
        endGamePlayer1AvatarBack.AddToClassList("disabledPlayerUI");
        endGamePlayer1ScoreBox.AddToClassList("disabledPlayerUI");
        endGamePlayer1Label.AddToClassList("disabledPlayerText");
    }

    void EnableEndGamePlayerUI(bool isPlayer1)
    {
        if (isPlayer1)
        {
            endGamePlayer1Avatar.RemoveFromClassList("disabledPlayerUI");
            endGamePlayer1AvatarBack.RemoveFromClassList("disabledPlayerUI");
            endGamePlayer1ScoreBox.RemoveFromClassList("disabledPlayerUI");
            endGamePlayer1Label.RemoveFromClassList("disabledPlayerText");
        }
        else
        {
            endGamePlayer2Avatar.RemoveFromClassList("disabledPlayerUI");
            endGamePlayer2AvatarBack.RemoveFromClassList("disabledPlayerUI");
            endGamePlayer2ScoreBox.RemoveFromClassList("disabledPlayerUI");
            endGamePlayer2Label.RemoveFromClassList("disabledPlayerText");
        }
    }
}
