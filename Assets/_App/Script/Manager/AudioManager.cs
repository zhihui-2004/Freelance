using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private Sound[] musicSounds;
    [SerializeField] private Sound[] sfxSounds;
    [SerializeField] private Sound[] narrativeSounds;

    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;
    [SerializeField] private Sprite sfxOnSprite;
    [SerializeField] private Sprite sfxOffSprite;
    [SerializeField] private Sprite loopOnSprite;
    [SerializeField] private Sprite loopOffSprite;

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource narrativeSource;

    private int currentMusicIndex = 0;
    private bool isLooping = false;
    private Coroutine autoPlayCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Just play sound
    public void PlayFirstTrack(TextMeshProUGUI musicNameText)
    {
        currentMusicIndex = 0;
        PlayMusicByIndex(currentMusicIndex);
        UpdateMusicName(musicNameText);

        if (autoPlayCoroutine != null)
        {
            StopCoroutine(autoPlayCoroutine);
        }
        autoPlayCoroutine = StartCoroutine(CheckForNextTrack(musicNameText));
    }

    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound not found.");
        }
        else
        {
            musicSource.clip = sound.audioClip;
            musicSource.loop = isLooping;
            musicSource.Play();
        }
    }

    private void PlayMusicByIndex(int index)
    {
        if (index >= 0 && index < musicSounds.Length)
        {
            currentMusicIndex = index;
            musicSource.clip = musicSounds[currentMusicIndex].audioClip;
            musicSource.loop = isLooping;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Index out of range.");
        }
    }

    public void PlayNextTrack(TextMeshProUGUI musicNameText)
    {
        currentMusicIndex++;
        if (currentMusicIndex >= musicSounds.Length)
        {
            currentMusicIndex = 0;
        }
        PlayMusicByIndex(currentMusicIndex);
        UpdateMusicName(musicNameText);
    }

    public void PlayPreviousTrack(TextMeshProUGUI musicNameText)
    {
        currentMusicIndex--;
        if (currentMusicIndex < 0)
        {
            currentMusicIndex = musicSounds.Length - 1;
        }
        PlayMusicByIndex(currentMusicIndex);
        UpdateMusicName(musicNameText);
    }

    public void StopBGM()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        if (autoPlayCoroutine != null)
        {
            StopCoroutine(autoPlayCoroutine);
            autoPlayCoroutine = null;
        }
    }

    public void PlaySFX(string name, bool loop = false)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound not found.");
        }
        else
        {
            sfxSource.clip = sound.audioClip;
            sfxSource.loop = loop;
            sfxSource.Play();
        }
    }

    public void StopSFX()
    {
        if (sfxSource.isPlaying)
        {
            sfxSource.Stop();
            sfxSource.loop = false;
        }
    }

    public void PlayNarrative(string name, bool loop = false)
    {
        Sound narrative = Array.Find(narrativeSounds, x => x.name == name);

        if (narrative == null)
        {
            Debug.Log("Sound not found.");
        }
        else
        {
            narrativeSource.clip = narrative.audioClip;
            narrativeSource.Play();
        }
    }

    public void StopNarrative()
    {
        if (narrativeSource.isPlaying)
        {
            narrativeSource.Stop();
        }
    }

    #endregion

    #region  Toggle sound
    public void ToggleMusic(TextMeshProUGUI musicStatusText, Image musicStatusImage)
    {
        UpdateMusicValue();
        UpdateMusicIcon(musicStatusText, musicStatusImage);
    }

    public void ToggleSFX(UnityEngine.UI.Button sfxButton)
    {
        UpdateSFXValue();
        UpdateSFXIcon(sfxButton);
    }

    #endregion

    #region  Toggle value
    private void UpdateMusicValue()
    {
        bool isMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        PlayerPrefs.SetInt("MusicMuted", isMuted ? 0 : 1);
        musicSource.mute = !isMuted;
    }

    private void UpdateSFXValue()
    {
        bool isMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;
        PlayerPrefs.SetInt("SFXMuted", isMuted ? 0 : 1);
        sfxSource.mute = !isMuted;
    }
    #endregion

    #region Update Icon
    public void UpdateMusicIcon(TextMeshProUGUI musicStatusText, Image musicStatusImage)
    {
        if (PlayerPrefs.GetInt("MusicMuted", 0) == 0)
        {
            musicSource.mute = false;
            musicStatusText.text = "Play";
            musicStatusImage.sprite = musicOnSprite;
        }
        else
        {
            musicSource.mute = true;
            musicStatusText.text = "Pause";
            musicStatusImage.sprite = musicOffSprite;
        }
    }

    public void UpdateSFXIcon(UnityEngine.UI.Button sfxButton)
    {
        if (PlayerPrefs.GetInt("SFXMuted", 0) == 0)
        {
            sfxSource.mute = false;
            sfxButton.GetComponent<Image>().sprite = sfxOnSprite;
        }
        else
        {
            sfxSource.mute = true;
            sfxButton.GetComponent<Image>().sprite = sfxOffSprite;
        }
    }

    public void UpdateLoopIcon(TextMeshProUGUI loopStatusText, Image loopStatusImage)
    {
        if (isLooping)
        {
            loopStatusText.text = "Loop On";
            loopStatusImage.sprite = loopOnSprite;
        }
        else
        {
            loopStatusText.text = "Loop Off";
            loopStatusImage.sprite = loopOffSprite;
        }
    }
    #endregion

    #region Update Music Name
    private void UpdateMusicName(TextMeshProUGUI musicNameText)
    {
        if (musicSource.clip != null)
        {
            musicNameText.text = musicSource.clip.name;
        }
        else
        {
            musicNameText.text = "No Music Playing";
        }
    }
    #endregion

    #region Change slider value
    public void MusicVolume(float value)
    {
        PlayerPrefs.SetFloat("Music", value);
        musicSource.volume = value / 100f;
    }

    public void SFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFX", value);
        sfxSource.volume = value / 100f;
    }

    #endregion

    #region Togggle loop mode
    public void ToggleLoop()
    {
        isLooping = !isLooping;
        musicSource.loop = isLooping;

        if (isLooping)
        {
            Debug.Log("Looping enabled for current music.");
        }
        else
        {
            Debug.Log("Looping disabled. Moving to next track on completion.");
        }
    }

    #endregion

    #region Auto-Play Next Track
    private IEnumerator CheckForNextTrack(TextMeshProUGUI musicNameText)
    {
        while (true)
        {
            if (!musicSource.isPlaying && !isLooping && musicSource.clip != null)
            {
                PlayNextTrack(musicNameText);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion

    #region Volume Control
    public void AdjustVolumeWithRotation(float hingeAngle, Text volumeText)
    {
        float clampedAngle = Mathf.Clamp(hingeAngle, 0f, 350f);
        float volume = clampedAngle / 350f;
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        volumeText.text = $"{Mathf.RoundToInt(volume * 100f)}%";
    }
    #endregion

    public float GetNarrativeClipLength()
    {
        return narrativeSource.clip != null ? narrativeSource.clip.length : 0f;
    }
}
