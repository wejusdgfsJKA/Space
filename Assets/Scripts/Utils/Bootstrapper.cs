using Player;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
namespace Utilities
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] protected AudioMixer audioMixer;
        protected void Start()
        {
            QualitySettings.shadows = ShadowQuality.Disable;
            VideoSettingsMenu.LoadVideoSettings();
            AudioSettingsMenu.LoadAudioSettings(audioMixer);
            SceneManager.LoadScene(1);
        }
    }
}