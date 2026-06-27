using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Player
{
    public class EndMissionScreen : MenuPDA
    {
        [SerializeField] protected TextMeshProUGUI playerKills, teamCasualties, enemyCasualties;

        protected override void OnEnable()
        {
            base.OnEnable();
            //PopulateText(GameManager.UnitsDestroyed);
        }

        public void PopulateText(List<ObjectDestroyed> unitsDestroyed)
        {
            unitsDestroyed.Sort((a, b) =>
            {
                return a.KillerName.CompareTo(b.KillerName);
            });

            for (int i = 0; i < unitsDestroyed.Count; i++)
            {
                if (unitsDestroyed[i].VictimTeam == GlobalConfig.PlayerTeam)
                {
                    teamCasualties.text += unitsDestroyed[i] + "\n";
                }
                else if (unitsDestroyed[i].VictimTeam == GlobalConfig.EnemyTeam)
                {
                    enemyCasualties.text += unitsDestroyed[i] + "\n";
                }
            }
        }

        public void ReturnToLobby()
        {
            GameManager.LoadLobby();
        }
    }
}