using System.Collections.Generic;
using Gameplay.player;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class View_Player : MonoBehaviour
{
    [SerializeField] private Image avatar;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private View_HistoryDue historyDue;
    [SerializeField] private TMP_Text hpText;

    [Button]
    public void SetData(Player player)
    {
        // SetName(player.name);
        // SetHp(player.hp);
        // SetAvatar(player.AvatarKey);
        // SetHistory(player.history);
    }

    [Button]
    public void SetName(string pName)
    {
        nameText.SetText(pName);
    }

    [Button]
    public void SetHp(int hp)
    {
        hpText.SetText($"{hp:N0}");
    }

    [Button]
    public void SetAvatar(string key)
    {
        Addressables.LoadAssetAsync<Sprite>(key).Completed += handle => { avatar.sprite = handle.Result; };
    }

    [Button]
    public void SetHistory(List<RoundResult> history)
    {
        historyDue.SetHistory(history);
    }
}