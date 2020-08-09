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
        public TextMeshPro meleeAttack;
        public TextMeshPro skill1;
        public TextMeshPro skill2;
        public TextMeshPro skill3;
        public TextMeshPro skill4;
        public TextMeshPro skill5;

        private int playCount = 0;
        private int winCount = 0;
        private int meleeAttackCount = 0;
        private int skill1Count = 0;
        private int skill2Count = 0;
        private int skill3Count = 0;
        private int skill4Count = 0;
        private int skill5Count = 0;

        public void UpdateInfo(IPlayer player, IPlayer enemy, float reward)
        {
            playCountText.text = "PlayCount: " + playCount;
            winCountText.text = "WinCount: " + winCount;
            rewardText.text = "Reward: " + reward.ToString("0.0000");
            healthText.text = "Health: " + player.GetHealth();
            manaText.text = "Mana: " + player.GetMana();
            healthPotText.text = "HealthPot: " + player.GetHealthPotionCount();
            manaPotText.text = "ManaPot: " + player.GetManaPotionCount();
            meleeAttack.text = "MeleeAttack: " + meleeAttackCount;
            skill1.text = "Skill1: " + skill1Count;
            skill2.text = "Skill2: " + skill2Count;
            skill3.text = "Skill3: " + skill3Count;
            skill4.text = "Skill4: " + skill4Count;
            skill5.text = "Skill5: " + skill5Count;
        }

        public void IncreasePlayCount()
		{
            this.playCount++;
		}

        public void IncreaseWinCount()
		{
            this.winCount++;
		}

        public void IncreaseMeleeAttackCount()
        {
            this.meleeAttackCount++;
        }

        public void IncreaseSkillCount(int skill)
        {
            switch(skill)
            {
                case 1:
                    skill1Count++;
                    break;
                case 2:
                    skill2Count++;
                    break;
                case 3:
                    skill3Count++;
                    break;
                case 4:
                    skill4Count++;
                    break;
                case 5:
                    skill5Count++;
                    break;
                default:
                break;
            }
        }

        public void ResetAttackAndSkillCounts()
        {
            this.meleeAttackCount = 0;
            this.skill1Count = 0;
            this.skill2Count = 0;
            this.skill3Count = 0;
            this.skill4Count = 0;
            this.skill5Count = 0;
        }

    }

}
