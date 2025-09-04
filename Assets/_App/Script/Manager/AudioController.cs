using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI musicName;
    [SerializeField] private TextMeshProUGUI musicStatus;
    [SerializeField] private Image musicStatusImage;

    [SerializeField] private TextMeshProUGUI loopStatus;
    [SerializeField] private Image loopStatusImage;

    [SerializeField] private Text volumeText;


    private void Start()
    {
        if (musicName != null)
            AudioManager.Instance.PlayFirstTrack(musicName);
    }

    //------------------------------------------ Play SFX ------------------------------------------//
    public void PlaySFX(string effectSound)
    {
        if (musicStatus != null)
            AudioManager.Instance.PlaySFX(effectSound);
    }

    public void StopSFX()
    {
        AudioManager.Instance.StopSFX();
    }
    //------------------------------------------ Play BGM ------------------------------------------//

    public void PlayBGM(string bgm)
    {
        AudioManager.Instance.PlayMusic(bgm);
        UpdateNowPlaying(bgm);
    }

    public void StopBGM()
    {
        AudioManager.Instance.StopBGM();
    }

    //------------------------------------------ Update Now Playing ------------------------------------------//
    private void UpdateNowPlaying(string bgm)
    {
        musicName.text = bgm;
    }


    //------------------------------------------ Play Next Track ------------------------------------------//
    public void PlayNextTrack()
    {
        AudioManager.Instance.PlayNextTrack(musicName);
        UpdateNowPlaying(AudioManager.Instance.musicSource.clip.name);
    }

    //------------------------------------------ Play Previous Track ------------------------------------------//
    public void PlayPreviousTrack()
    {
        AudioManager.Instance.PlayPreviousTrack(musicName);
        UpdateNowPlaying(AudioManager.Instance.musicSource.clip.name);
    }

    //------------------------------------------ Toggle Music ------------------------------------------//
    public void ToggleMusic()
    {
        if (musicStatusImage != null)
            AudioManager.Instance.ToggleMusic(musicStatus, musicStatusImage);
    }

    //------------------------------------------ Adjust Volume ------------------------------------------//
    public void AdjustVolume(float volume)
    {
        AudioManager.Instance.musicSource.volume = volume / 100f;
    }

    //------------------------------------------ Rotate-Based Volume Adjustment ------------------------------------------//
    public void AdjustVolumeWithRotation(float hingeAngle)
    {
        if (volumeText != null)
            AudioManager.Instance.AdjustVolumeWithRotation(hingeAngle, volumeText);
    }

    //------------------------------------------ Toggle Music Loop ------------------------------------------//
    public void ToggleMusicLoop()
    {
        AudioManager.Instance.ToggleLoop();
        if (loopStatus != null && loopStatusImage != null)
            AudioManager.Instance.UpdateLoopIcon(loopStatus, loopStatusImage);
    }
}