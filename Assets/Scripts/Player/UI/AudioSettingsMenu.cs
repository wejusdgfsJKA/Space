using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
namespace Player
{
    public class AudioSettingsMenu : MonoBehaviour
    {
        [SerializeField] Slider masterSlider;
        [SerializeField] Slider musicSlider;
        [SerializeField] Slider sfxSlider;

        private const string MixerMaster = "MasterVolume";
        private const string MixerMusic = "MusicVolume";
        private const string MixerSFX = "SFXVolume";

        private const string KeyMaster = "Audio_Master";
        private const string KeyMusic = "Audio_Music";
        private const string KeySFX = "Audio_SFX";

        [SerializeField] AudioMixer audioMixer;

        protected void OnEnable()
        {
            float master = PlayerPrefs.GetFloat(KeyMaster, 1f);
            float music = PlayerPrefs.GetFloat(KeyMusic, 1f);
            float sfx = PlayerPrefs.GetFloat(KeySFX, 1f);

            masterSlider.SetValueWithoutNotify(master);
            musicSlider.SetValueWithoutNotify(music);
            sfxSlider.SetValueWithoutNotify(sfx);

            ApplyVolume(MixerMaster, master);
            ApplyVolume(MixerMusic, music);
            ApplyVolume(MixerSFX, sfx);
        }

        protected void OnDisable()
        {
            SaveAudioSettings();
        }

        public void SetMasterVolume(float value) => ApplyVolume(MixerMaster, value);

        public void SetMusicVolume(float value) => ApplyVolume(MixerMusic, value);

        public void SetSFXVolume(float value) => ApplyVolume(MixerSFX, value);

        protected void ApplyVolume(string parameter, float linearValue)
        {
            float db = Mathf.Log10(Mathf.Max(linearValue, 0.0001f)) * 20f;
            audioMixer.SetFloat(parameter, db);
        }

        public void SaveAudioSettings()
        {
            PlayerPrefs.SetFloat(KeyMaster, masterSlider.value);
            PlayerPrefs.SetFloat(KeyMusic, musicSlider.value);
            PlayerPrefs.SetFloat(KeySFX, sfxSlider.value);
            PlayerPrefs.Save();
        }

        public static void LoadAudioSettings(AudioMixer mixer)
        {
            SetMixerVolume(mixer, MixerMaster, PlayerPrefs.GetFloat(KeyMaster, 1f));
            SetMixerVolume(mixer, MixerMusic, PlayerPrefs.GetFloat(KeyMusic, 1f));
            SetMixerVolume(mixer, MixerSFX, PlayerPrefs.GetFloat(KeySFX, 1f));
        }

        static void SetMixerVolume(AudioMixer mixer, string parameter, float linearValue)
        {
            float db = Mathf.Log10(Mathf.Max(linearValue, 0.0001f)) * 20f;
            mixer.SetFloat(parameter, db);
        }
    }
}