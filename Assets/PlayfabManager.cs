using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayfabManager : MonoBehaviour
{
    [Header("LeaderBoard")]
    public GameObject rowPrefab;
    public Transform rowsParent;

    [Header("Display Name Window")]
    public TMP_InputField nameInput;

    [Header("LeaderBoard & DisplayName board references")]
    public GameObject displayNameWindow;
    public GameObject leaderBoardWindow;
    void Start()
    {
        Login();
    }

    void Login(){
        var request = new LoginWithCustomIDRequest{
            CustomId = SystemInfo.deviceUniqueIdentifier,
            // CustomId = player_name,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams{
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }
    void OnLoginSuccess(LoginResult result){
        Debug.Log("Successful Login/Account create!");
        string name = null;
        if(result.InfoResultPayload.PlayerProfile != null)
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
            Debug.Log("Player Name: "+name);

        if(name == null){
            displayNameWindow.SetActive(true);
        }
        // else{
        //     leaderBoardWindow.SetActive(true);
        // }
    }
    // submit name button
    public void SubmitName(){
        var request = new UpdateUserTitleDisplayNameRequest {
            DisplayName = nameInput.text
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result){
        Debug.Log("Update display name");
        displayNameWindow.SetActive(false);
        // leaderBoardWindow.SetActive(true);
    } 



    void OnError(PlayFabError error){
        Debug.Log("Error while logging in/Creating account");
        Debug.Log(error.GenerateErrorReport());
    }
    public void SendLeaderBoard(int score){
        var request = new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate>{
                new StatisticUpdate{
                    StatisticName = "Frosty Enchantments", 
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }
    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result){
        Debug.Log("Succesfull leaderboard sent");
    }
    public void GetLeaderBoard(){
        var request =  new GetLeaderboardRequest {
            StatisticName = "Frosty Enchantments",
            StartPosition = 0,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderBoardGet, OnError);
    }
    void OnLeaderBoardGet(GetLeaderboardResult result){
        foreach (Transform item in rowsParent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            TMP_Text[] texts = newGo.GetComponentsInChildren<TMP_Text>();
            texts[0].text = (item.Position+1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();
            print(item.Position + " " + item.PlayFabId + " " + item.StatValue);
        }
    }
}
