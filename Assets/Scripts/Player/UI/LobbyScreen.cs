using System.Collections;
using TMPro;
using UnityEngine;
namespace Player
{
    public class LobbyScreen : MenuPDA
    {
        [SerializeField] protected int missionStartTimerDuration = 5;
        [SerializeField] protected TextMeshProUGUI timerText;
        protected WaitForSeconds waitForOneSecond = new(1);
        protected Coroutine coroutine;

        protected override void OnEnable()
        {
            base.OnEnable();
            timerText.text = missionStartTimerDuration.ToString();
        }

        public void BeginMission()
        {
            Mission.CurrentMission = Mission.GetRandomMission();
            coroutine = StartCoroutine(MissionStartCountdown());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (coroutine != null) StopCoroutine(coroutine);
        }

        protected IEnumerator MissionStartCountdown()
        {
            int count = missionStartTimerDuration;
            while (count > 0)
            {
                yield return waitForOneSecond;
                timerText.text = count.ToString();
                count--;
            }
            GameManager.LoadMission();
        }
    }
}