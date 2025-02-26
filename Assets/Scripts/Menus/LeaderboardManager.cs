using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;

namespace Menus
{
    public class LeaderboardManager : MonoBehaviour
    {
        private UIDocument uiDocument;
        private MultiColumnListView multiColumnListView;
        
        private Button exitButton;
        private void Awake()
        {
            uiDocument = GetComponent<UIDocument>();
            multiColumnListView = uiDocument.rootVisualElement.Q<MultiColumnListView>("LeaderBoardList");
            if (UserAccountManager.Instance.IsUserLoggedIn())  UserAccountManager.Instance.GetLeaderBoard(OnSuccess,OnError);
            
            exitButton = uiDocument.rootVisualElement.Q<Button>("ExitButton");
            RegisterButtonCallbacks();
        }

        private void RegisterButtonCallbacks()
        {
            exitButton?.RegisterCallback<ClickEvent>(OnClickExitButton);
        }
        

        private void OnClickExitButton(ClickEvent evt)
        {
            uiDocument.sortingOrder = -1;
        }

        private void OnError(string obj)
        {
            throw new System.NotImplementedException();
        }

        private void OnSuccess(List<LeaderboardEntry> entries)
        {
            //Set Binding here
            BindDataToMultiColumnListView(entries);
            multiColumnListView.Rebuild();
        }
        private void BindDataToMultiColumnListView(List<LeaderboardEntry> leaderboardEntries)
        {
            multiColumnListView.itemsSource = leaderboardEntries;
            
            multiColumnListView.columns[0].makeCell = () => new Label();
            multiColumnListView.columns[0].bindCell = (cell, index) => (cell as Label).text = leaderboardEntries[index].Position.ToString();

            multiColumnListView.columns[1].makeCell = () => new Label();
            multiColumnListView.columns[1].bindCell = (cell, index) => (cell as Label).text = leaderboardEntries[index].PlayFabId;

            multiColumnListView.columns[2].makeCell = () => new Label();
            multiColumnListView.columns[2].bindCell = (cell, index) => (cell as Label).text = leaderboardEntries[index].StatValue.ToString();
        
        }

        private void OnDisable()
        {
            UnregisterButtonCallbacks();
        }

        private void UnregisterButtonCallbacks()
        {
            exitButton?.UnregisterCallback<ClickEvent>(OnClickExitButton);
        }
    }
}