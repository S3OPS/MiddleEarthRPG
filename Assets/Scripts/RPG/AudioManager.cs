using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Centralized audio manager for game sounds and music
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("Audio Sources")]
    private AudioSource _musicSource;
    private AudioSource _sfxSource;
    private List<AudioSource> _sfxPool = new List<AudioSource>();
    
    [Header("Audio Settings")]
    public float masterVolume = 1.0f;
    public float musicVolume = 0.7f;
    public float sfxVolume = 1.0f;
    
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    private const int SFX_POOL_SIZE = 10;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        InitializeAudioSources();
        GenerateProceduralSounds();
    }
    
    private void InitializeAudioSources()
    {
        // Music source
        _musicSource = gameObject.AddComponent<AudioSource>();
        _musicSource.loop = true;
        _musicSource.volume = musicVolume * masterVolume;
        
        // SFX source pool for overlapping sounds
        for (int i = 0; i < SFX_POOL_SIZE; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.volume = sfxVolume * masterVolume;
            source.loop = false;
            _sfxPool.Add(source);
        }
        
        _sfxSource = _sfxPool[0];
    }
    
    /// <summary>
    /// Generate simple procedural sound effects using Unity's AudioClip.Create
    /// This allows us to have sounds without external audio files
    /// </summary>
    private void GenerateProceduralSounds()
    {
        // Attack swing sound - whoosh
        _audioClips["swing"] = CreateSimpleTone(0.2f, 200f, 100f, 0.3f);
        
        // Hit impact sound - thud
        _audioClips["hit"] = CreateSimpleTone(0.15f, 150f, 50f, 0.5f);
        
        // Critical hit - higher pitched impact
        _audioClips["critical"] = CreateSimpleTone(0.2f, 400f, 200f, 0.7f);
        
        // Special ability - magical whoosh
        _audioClips["special"] = CreateSimpleTone(0.4f, 500f, 800f, 0.5f);
        
        // Level up - ascending tones
        _audioClips["levelup"] = CreateAscendingTone(0.5f, 400f, 800f, 0.6f);
        
        // Quest complete - triumphant
        _audioClips["questcomplete"] = CreateAscendingTone(0.6f, 300f, 600f, 0.5f);
        
        // Treasure open - sparkle
        _audioClips["treasure"] = CreateSimpleTone(0.3f, 800f, 1200f, 0.4f);
        
        // Footstep - short thud
        _audioClips["footstep"] = CreateSimpleTone(0.1f, 80f, 60f, 0.2f);
        
        // Enemy death - descending tone
        _audioClips["enemydeath"] = CreateDescendingTone(0.4f, 300f, 100f, 0.5f);
    }
    
    private AudioClip CreateSimpleTone(float duration, float startFreq, float endFreq, float volume)
    {
        int sampleRate = 44100;
        int sampleCount = (int)(duration * sampleRate);
        AudioClip clip = AudioClip.Create("Tone", sampleCount, 1, sampleRate, false);
        
        float[] samples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleCount;
            float freq = Mathf.Lerp(startFreq, endFreq, t);
            float envelope = Mathf.Sin(t * Mathf.PI); // Smooth fade in/out
            samples[i] = Mathf.Sin(2 * Mathf.PI * freq * i / sampleRate) * envelope * volume;
        }
        
        clip.SetData(samples, 0);
        return clip;
    }
    
    private AudioClip CreateAscendingTone(float duration, float startFreq, float endFreq, float volume)
    {
        int sampleRate = 44100;
        int sampleCount = (int)(duration * sampleRate);
        AudioClip clip = AudioClip.Create("AscendingTone", sampleCount, 1, sampleRate, false);
        
        float[] samples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleCount;
            float freq = Mathf.Lerp(startFreq, endFreq, Mathf.Pow(t, 0.5f));
            float envelope = 1f - (t * 0.5f); // Gradual fade out
            samples[i] = Mathf.Sin(2 * Mathf.PI * freq * i / sampleRate) * envelope * volume;
        }
        
        clip.SetData(samples, 0);
        return clip;
    }
    
    private AudioClip CreateDescendingTone(float duration, float startFreq, float endFreq, float volume)
    {
        int sampleRate = 44100;
        int sampleCount = (int)(duration * sampleRate);
        AudioClip clip = AudioClip.Create("DescendingTone", sampleCount, 1, sampleRate, false);
        
        float[] samples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            float t = (float)i / sampleCount;
            float freq = Mathf.Lerp(startFreq, endFreq, t);
            float envelope = 1f - t; // Fade out
            samples[i] = Mathf.Sin(2 * Mathf.PI * freq * i / sampleRate) * envelope * volume;
        }
        
        clip.SetData(samples, 0);
        return clip;
    }
    
    private AudioSource GetAvailableSFXSource()
    {
        foreach (var source in _sfxPool)
        {
            if (!source.isPlaying)
                return source;
        }
        return _sfxPool[0]; // Fallback to first source
    }
    
    public void PlaySound(string soundName, float volumeMultiplier = 1f)
    {
        if (_audioClips.ContainsKey(soundName))
        {
            AudioSource source = GetAvailableSFXSource();
            source.PlayOneShot(_audioClips[soundName], volumeMultiplier * sfxVolume * masterVolume);
        }
    }
    
    public void PlayAttackSound(bool isCritical)
    {
        PlaySound(isCritical ? "critical" : "hit", isCritical ? 1.2f : 1f);
    }
    
    public void PlaySwingSound()
    {
        PlaySound("swing", 0.5f);
    }
    
    public void PlaySpecialAbilitySound()
    {
        PlaySound("special", 1f);
    }
    
    public void PlayLevelUpSound()
    {
        PlaySound("levelup", 1f);
    }
    
    public void PlayQuestCompleteSound()
    {
        PlaySound("questcomplete", 1f);
    }
    
    public void PlayTreasureSound()
    {
        PlaySound("treasure", 0.8f);
    }
    
    public void PlayFootstepSound()
    {
        PlaySound("footstep", 0.3f);
    }
    
    public void PlayEnemyDeathSound()
    {
        PlaySound("enemydeath", 0.7f);
    }
    
    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        _musicSource.volume = musicVolume * masterVolume;
        foreach (var source in _sfxPool)
        {
            source.volume = sfxVolume * masterVolume;
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        _musicSource.volume = musicVolume * masterVolume;
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }
}
