using battle.define;
using event_name;
using TigerForge;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UI_PlayerAvatar : MonoBehaviour
{
    [SerializeField] private Team team;
    [SerializeField] private HttpImage avatar;
    
    private void OnEnable()
    {
        EventManager.StartListening(EventName.Gameplay.StartGame, UpdateView);
        UpdateView();
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.Gameplay.StartGame, UpdateView);
    }

    private void UpdateView()
    {
        // var key = Client_DueManager.GetPlayer(team).AvatarKey;
        // if (key == "avatar_0") return; //not exist, default value
        
        var info = Networks.Network.Query.Fighting.GetTeam(team);
        avatar.SetData(info.avatarImage);
        // Addressables.LoadAssetAsync<Sprite>(key).Completed += handle => { avatar.sprite = handle.Result; };
    }
}