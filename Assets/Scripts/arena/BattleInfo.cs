using UnityEngine;
using TMPro;

namespace PAPIOnline
{

    public class BattleInfo : MonoBehaviour
    {

        public TextMeshProUGUI rewardText;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI manaText;
        public TextMeshProUGUI healthPotText;
        public TextMeshProUGUI manaPotText;
        public TextMeshProUGUI speedText;
        public TextMeshProUGUI defenseText;
        public TextMeshProUGUI damageText;
        public TextMeshProUGUI attackRangeText;
        public TextMeshProUGUI canAttack;
        public TextMeshProUGUI distance;

        public void UpdateInfo(IPlayer player, IPlayer enemy, float reward)
        {
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

    }

}
