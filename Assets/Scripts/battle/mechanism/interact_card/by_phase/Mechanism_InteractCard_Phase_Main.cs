using System;
using battle.define;
using battle.query;
using battle.view;

namespace battle.mechanism.interact_card.by_phase
{
    public class Mechanism_InteractCard_Phase_Main : IMechanism_InteractCard
    {
        public bool InEnable(string cardGuid)
        {
            // if (DueQuery.Is_TheirTurn || DueQuery_Card.IsTheir(cardGuid)) return false;
            //
            //
            // var cardLocation = DueQuery_Card.Locate(cardGuid);
            // var cardType = DueQuery_Card.TypeOf(cardGuid);
            //
            // switch (cardLocation.zone)
            // {
            //     case BoardZoneType.InHand:
            //     {
            //         switch (cardType)
            //         {
            //             case CardType.Monster:
            //                 return Can_InHand_Monster(cardGuid, out _);
            //             case CardType.Spell:
            //                 throw new NotImplementedException();
            //             case CardType.Trap:
            //                 throw new NotImplementedException();
            //             case CardType.Field:
            //                 throw new NotImplementedException();
            //         }
            //
            //         break;
            //     }
            //     case BoardZoneType.MainMonster:
            //     {
            //         switch (cardType)
            //         {
            //             case CardType.Monster:
            //                 return Can_InBoard_Monster(cardGuid, out _);
            //             case CardType.Spell:
            //                 throw new NotImplementedException();
            //             case CardType.Trap:
            //                 throw new NotImplementedException();
            //         }
            //
            //         break;
            //     }
            // }

            return false;
        }


        public void OnSelect_Any(string cardGuid)
        {
            // if (DueQuery.Is_TheirTurn || DueQuery_Card.IsTheir(cardGuid)) return;
            //
            // var cardLocation = DueQuery_Card.Locate(cardGuid);
            // var cardType = DueQuery_Card.TypeOf(cardGuid);
            //
            // switch (cardLocation.zone)
            // {
            //     case BoardZoneType.InHand:
            //     {
            //         switch (cardType)
            //         {
            //             case CardType.Monster:
            //                 if (Can_InHand_Monster(cardGuid, out var impossibleReason))
            //                 {
            //                     Panel_CardOptions.ShowAt(cardGuid).UseOptions_NormalSummon();
            //                 }
            //                 else
            //                 {
            //                     Panel_CardOptions.ShowAt(cardGuid).UseText(impossibleReason);
            //                 }
            //
            //                 break;
            //             case CardType.Spell:
            //                 throw new NotImplementedException();
            //             case CardType.Trap:
            //                 throw new NotImplementedException();
            //             case CardType.Field:
            //                 throw new NotImplementedException();
            //         }
            //
            //         break;
            //     }
            //     case BoardZoneType.MainMonster:
            //     {
            //         switch (cardType)
            //         {
            //             case CardType.Monster:
            //                 if (Can_InBoard_Monster(cardGuid, out var impossibleReason))
            //                 {
            //                     Panel_CardOptions.ShowAt(cardGuid).UseOptions_ChangePosition();
            //                 }
            //                 else
            //                 {
            //                     Panel_CardOptions.ShowAt(cardGuid).UseText(impossibleReason);
            //                 }
            //
            //                 break;
            //             case CardType.Spell:
            //                 throw new NotImplementedException();
            //             case CardType.Trap:
            //                 throw new NotImplementedException();
            //         }
            //
            //         break;
            //     }
            // }
        }

        public void OnSelect_Enable(string cardGuid)
        {
        }


        private bool Can_InHand_Monster(string cardGuid, out string impossibleReason)
        {
            // Các lý do không thể:
            // BÀI THƯỜNG
            // - lượt này đã triệu hồi thường -> Event Summon
            // - số lượng quái đầy -> Event Summon
            // BÀI HIẾN TẾ
            // - lượt này đã triệu hồi thường -> Event Summon
            // - không đủ hiến tế

            throw new NotImplementedException();
        }

        private bool Can_InBoard_Monster(string cardGuid, out string impossibleReason)
        {
            // Các lý do không thể:
            // - vừa triệu hồi trong lượt này -> Event Summon
            // - đã tấn công trong lượt này -> Event Attack
            // - đã thay đổi trong lượt này -< Event Chane Position

            throw new NotImplementedException();
        }
    }
}