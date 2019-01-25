using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarrotPack
{
    public class AudioManager : MonoBehaviour, IManager
    {
        public AudioLibrary audioLibrary;

        private GameObject bgmManager;
        private GameObject soundEffectManager;
        private GameObject voiceLineManager;

        private AudioSource bgmAudioSource;
        private List<AudioSource> audioSourcesSE = new List<AudioSource>();
        private List<AudioSource> audioSourcesVoice = new List<AudioSource>();

        private IEnumerator bgmCoroutine;

        public void InitializeManager()
        {
            if (soundEffectManager == null)
            {
                soundEffectManager = new GameObject("SE Manager");
                soundEffectManager.transform.parent = Toolbox.Instance.transform;
            }

            if (voiceLineManager == null)
            {
                voiceLineManager = new GameObject("Voice Line Manager");
                voiceLineManager.transform.parent = Toolbox.Instance.transform;
            }

            if (bgmManager == null)
            {
                bgmManager = new GameObject("BGM Manager");
                bgmAudioSource = bgmManager.AddComponent<AudioSource>();
                bgmManager.transform.parent = Toolbox.Instance.transform;
            }

#if !ODIN_INSPECTOR
            // Populate Dictionary
            audioLibrary.InitLibrary();
#endif
        }

        public void PlaySoundEffect(AudioClip audioClip = null)
        {
            if (audioClip == null)
            {
                Debug.LogError("AudioClip Missing!");
                return;
            }

            for (int i = 0; i < audioSourcesSE.Count; ++i)
            {
                if (audioSourcesSE[i].isPlaying)
                {
                    continue;
                }
                else
                {
                    Debug.Log("Play SE: " + audioClip.name);
                    audioSourcesSE[i].PlayOneShot(audioClip);
                    return;
                }
            }

            Debug.Log("Instantiating new audio source");
            GameObject newAudioSource = new GameObject();
            audioSourcesSE.Add(newAudioSource.AddComponent<AudioSource>());
            newAudioSource.transform.parent = soundEffectManager.transform;

            Debug.Log("Play SE: " + audioClip.name);
        }

        public void PlayVoiceLine(AudioClip audioClip = null)
        {
            for (int i = 0; i < audioSourcesVoice.Count; ++i)
            {
                if (audioSourcesVoice[i].isPlaying)
                {
                    continue;
                }
                else
                {
                    Debug.Log("Play Voice Line");
                    audioSourcesVoice[i].PlayOneShot(audioClip);
                    return;
                }
            }

            Debug.Log("Instantiating new audio source");
            GameObject newAudioSource = new GameObject();
            audioSourcesVoice.Add(newAudioSource.AddComponent<AudioSource>());
            newAudioSource.transform.parent = voiceLineManager.transform;

            Debug.Log("Play Voice Line");
        }

        public void BGMPlayer(AudioClip audioClip = null, PlayBGMType type = PlayBGMType.Repeat, float volume = 1f, float fadeDuration = 1f)
        {
            // BGM Player Functions
            switch (type)
            {
                case PlayBGMType.Repeat:
                    if (bgmAudioSource.isPlaying) { bgmAudioSource.Stop(); }
                    bgmAudioSource.loop = true;
                    break;

                case PlayBGMType.Single:
                    if (bgmAudioSource.isPlaying) { bgmAudioSource.Stop(); }
                    bgmAudioSource.loop = false;
                    break;

                case PlayBGMType.Stop:
                    bgmAudioSource.Stop();
                    return;

                case PlayBGMType.FadeIn:
                    if (bgmCoroutine != null)
                    {
                        StopCoroutine(bgmCoroutine);
                    }

                    bgmCoroutine = BGMFade(fadeDuration, true);
                    StartCoroutine(bgmCoroutine);
                    return;

                case PlayBGMType.FadeOut:
                    if (bgmCoroutine != null)
                    {
                        StopCoroutine(bgmCoroutine);
                    }

                    bgmCoroutine = BGMFade(fadeDuration, false);
                    StartCoroutine(bgmCoroutine);
                    return;
            }

            // Playing Audio Clip
            if (audioClip == null)
            {
                Debug.LogError("No Audio Clip Found in calling PlayBGM");
                return;
            }

            bgmAudioSource.volume = volume;
            bgmAudioSource.clip = audioClip;
            bgmAudioSource.Play();
        }

        /// True for fade in, False for fade out 
        private IEnumerator BGMFade(float duration, bool fadeIn, float startVolume = -1f)
        {
            int dir = 1;
            if (fadeIn)
            {
                dir = 1;
            }
            else
            {
                dir = -1;
            }

            if (startVolume >= 0)
            {
                bgmAudioSource.volume = startVolume;
            }

            float timer = duration;
            while (timer > 0)
            {
                bgmAudioSource.volume += 1f / duration * dir;

                yield return null;
                timer -= Time.deltaTime;
                if (bgmAudioSource.volume == 1f || bgmAudioSource.volume == 0f)
                {
                    Debug.LogWarning("Audio source is already at 0 or 1, stopping fade. Fade In: " + fadeIn);
                    Debug.LogWarning("Fade lasted for (seconds): " + (duration - timer).ToString());
                    yield break;
                }
            }

            Debug.Log("Completed BGM Fade In (true) /Out (false): " + fadeIn + " for duration: " + duration);
        }

        public enum PlayBGMType
        {
            Repeat,
            Single,
            Stop,
            FadeOut,
            FadeIn,
        }
    }
}