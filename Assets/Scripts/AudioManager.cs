using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] splatterSounds;
    [SerializeField] private AudioClip[] sliceSounds;
    [SerializeField] private AudioClip fruitThrowSound;
    [SerializeField] private AudioClip bombThrowSound;
    [SerializeField] private AudioClip bombExplosionSound;
    [SerializeField] private AudioClip yoo;
    [SerializeField] private AudioClip gameStartSound;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip comboSound;
    [SerializeField] private float volume = 1f;

    private AudioSource audioPlayer;
    private AudioSource metaAudioPlayer;
    private float pitchLowerRange = 0.8f;
    private float pitchUpperRange = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = gameObject.GetComponent<AudioSource>();
        metaAudioPlayer = gameObject.transform.GetChild(0).GetComponent<AudioSource>();
        audioPlayer.volume = volume;
        metaAudioPlayer.volume = volume;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayYooSound()
    {
        metaAudioPlayer.PlayOneShot(yoo, 0.4f);
    }

    public void PlayGameStartSound()
    {
        metaAudioPlayer.PlayOneShot(gameStartSound, 0.5f);
    }

    public void PlayGameOverSound()
    {
        metaAudioPlayer.PlayOneShot(gameOverSound, 0.5f);
    }

    public void PlayComboSound()
    {
        metaAudioPlayer.PlayOneShot(comboSound, 0.8f);
    }

    public void PlaySliceSound()
    {
        audioPlayer.pitch = Random.Range(pitchLowerRange, pitchUpperRange);
        audioPlayer.PlayOneShot(splatterSounds[Random.Range(0, splatterSounds.Length)]);
        audioPlayer.PlayOneShot(sliceSounds[Random.Range(0, sliceSounds.Length)]);
    }

    public void PlayFruitThrowSound()
    {
        audioPlayer.pitch = Random.Range(pitchLowerRange, pitchUpperRange);
        audioPlayer.PlayOneShot(fruitThrowSound, 0.3f);
    }

    public void PlayBombThrowSound()
    {
        audioPlayer.PlayOneShot(bombThrowSound, 1f);
    }

    public void PlayExplosionSound()
    {
        audioPlayer.PlayOneShot(bombExplosionSound);
    }
}