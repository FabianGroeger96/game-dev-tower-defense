using UnityEngine;

public class SoundController : MonoBehaviour
{


    public AudioClip music;
    public AudioClip baseHit;
    public AudioClip enemyHit;
    
    private static AudioClip _baseHit;
    private static AudioClip _enemyHit;
    private static AudioSource _musicOutput;
    private static AudioSource _hitOutput;


    // Start is called before the first frame update
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

    public static void SpeedUpPaceOfMusic()
    {
        //_musicOutput.pitch += 0.001f;
    }

    public static void PlayBaseHit()
    {
        _hitOutput.PlayOneShot(_baseHit, 1);
    }
    
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlayEnemyHit()
    {
        _hitOutput.PlayOneShot(_enemyHit, 1);
    }

    public static void ToogleMute()
    {
        _musicOutput.mute = !_musicOutput.mute;
        _hitOutput.mute = !_hitOutput.mute;
    }
}
