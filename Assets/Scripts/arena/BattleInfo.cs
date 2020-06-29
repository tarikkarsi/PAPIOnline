/*******************************************************************************
 *   Namespace:      PAPIOnline
 *   
 *   Class:          BattleInfo
 *   
 *   Description:    All information about battle stored here
 *   
 *   Author:         Tarik Karsi
 *   Email:          tarikkarsi@hotmail.com
 *   
 *   Revision History:
 *   Name:           Date:        Description:
 *   Tarik Karsi	 28.04.2020	  Initial Release
 *******************************************************************************/
using UnityEngine;
using TMPro;

namespace PAPIOnline
{

    public class BattleInfo : MonoBehaviour
    {
        public TextMeshPro playCountText;
        public TextMeshPro winCountText;
        public TextMeshPro rewardText;
        public TextMeshPro healthText;
        public TextMeshPro manaText;
        public TextMeshPro healthPotText;
        public TextMeshPro manaPotText;
        public TextMeshPro speedText;
        public TextMeshPro defenseText;
        public TextMeshPro damageText;
        public TextMeshPro attackRangeText;
        public TextMeshPro canAttack;
        public TextMeshPro distance;

        private int playCount = 0;
        private int winCount = 0;

        public void UpdateInfo(IPlayer player, IPlayer enemy, float reward)
        {
            playCountText.text = "PlayCount: " + playCount;
            winCountText.text = "WinCount: " + winCount;
            rewardText.text = "Reward: " + reward.ToString("0.0000");
            healthText.text = "Health: " + player.GetHealth();
            manaText.text = "Mana: " + player.GetMana();
            healthPotText.text = "HealthPot: " + player.GetHealthPotionCount();
            manaPotText.text = "ManaPot: " + player.GetManaPotionCount();
            speedText.text = "Speed: " + player.GetSpeed();
            defenseText.text = "Defense: " + player.GetDefense();
            damageText.text = "Damage: " + player.GetDamage();
            attackRangeText.text = "AttackRange: " + player.GetAttackRange();
            canAttack.text = "CanAttack: " + Utils.CanAttack(player, enemy);
            distance.text = "Distance: " + Utils.GetDistance(player, enemy);
        }

        public void IncreasePlayCount()
		{
            this.playCount++;
		}

        public void IncreaseWinCount()
		{
            this.winCount++;
		}

    }

}
