using UnityEngine;

/// <summary>
/// Controller that handles the sound of the game.
/// </summary>
public class SoundController : MonoBehaviour
{
    // reference to the audio file of the game music
    public AudioClip music;

    // reference to the audio file, which will play when the base gets hit
    public AudioClip baseHit;

    // reference to the audio file, which will play when an enemy is hit
    public AudioClip enemyHit;

    // static reference to the base hit
    private static AudioClip _baseHit;

    // static reference to the enemy hit
    private static AudioClip _enemyHit;

    // static reference to the game music
    private static AudioSource _musicOutput;

    // static reference to the hit output
    private static AudioSource _hitOutput;

    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    void Awake()
    {
        _baseHit = baseHit;
        _enemyHit = enemyHit;
        AudioSource[] sources = GetComponents<AudioSource>();
        _musicOutput = sources[0];
        _hitOutput = sources[1];
        _musicOutput.clip = music;
        _musicOutput.loop = true;
        _musicOutput.Play();
        _musicOutput.volume = 0.5f;
        _hitOutput.volume = 0.4f;
    }

    /// <summary>
    /// Speeds up the pace of the music
    /// </summary>
    public static void SpeedUpPaceOfMusic(float fraction)
    {
        _musicOutput.pitch += (0.25f / fraction);
    }

    /// <summary>
    /// Plays the base hit sound.
    /// </summary>
    public static void PlayBaseHit()
    {
        _hitOutput.PlayOneShot(_baseHit, 1);
    }

    /// <summary>
    /// Plays the enemy hit sound.
    /// </summary>
    public static void PlayEnemyHit()
    {
        _hitOutput.PlayOneShot(_enemyHit, 1);
    }

    /// <summary>
    /// Toggles the mute of the audio.
    /// </summary>
    public static void ToogleMute()
    {
        _musicOutput.mute = !_musicOutput.mute;
        _hitOutput.mute = !_hitOutput.mute;
    }
}
