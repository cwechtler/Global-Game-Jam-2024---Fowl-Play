﻿using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {
	public static SoundManager instance = null;
	[Range(.01f, .5f)] [SerializeField] private float fadeInTime = .05f;

	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private AudioSource MusicAudioSource;
	[SerializeField] private AudioSource SFXAudioSource;
	[SerializeField] private AudioSource ambientAudioSource;
	[Space]
	[SerializeField] private AudioClip[] music;
	[SerializeField] private AudioClip[] ambientClips;
	[SerializeField] private AudioClip[] movementClips;
	[Space]
	[SerializeField] private AudioClip hurtClip;
	[SerializeField] private AudioClip deathClip;
	[SerializeField] private AudioClip buttonClick;

	public int MusicArrayLength { get => music.Length; }

	private float audioVolume = 1f;
	private int clipIndex = 0;

	private void Awake(){
		if (instance != null){
			Destroy(gameObject);
		} else{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start()
	{
		if (PlayerPrefs.HasKey("master_volume")) {
			ChangeMasterVolume(PlayerPrefsManager.GetMasterVolume());
		}
		else {
			ChangeMasterVolume(0f);
		}

		if (PlayerPrefs.HasKey("music_volume")) {
			ChangeMusicVolume(PlayerPrefsManager.GetMusicVolume());
		}

		if (PlayerPrefs.HasKey("sfx_volume")) {
			ChangeSFXVolume(PlayerPrefsManager.GetSFXVolume());
		}

		PlayMusicForScene(0);
	}

	private void Update(){
		PlayRandomAmbient();

		VolumeFadeIn(MusicAudioSource);
		VolumeFadeIn(ambientAudioSource);
	}

	private void VolumeFadeIn(AudioSource audioSource) {
		if (audioVolume <= 1f){
			audioVolume += fadeInTime * Time.deltaTime;
			audioSource.volume = audioVolume;
		} else{
			audioVolume = 1f;
		}

		if (audioSource.clip != null){
			if (!audioSource.isPlaying){
				audioSource.Play();
				audioSource.volume = 0f;
				audioVolume = 0f;
			}
		}
	}

	private void VolumeFadeOut(AudioSource audioSource) {
		if (audioVolume >= 1f){
			audioVolume -= fadeInTime * Time.deltaTime;
			audioSource.volume = audioVolume;
		} else{
			audioVolume = 0f;
		}

		if (audioSource.volume <= 0f){
			audioSource.Stop();
		}
	}

	private void PlayRandomAmbient()
	{
		if (!ambientAudioSource.isPlaying && ambientClips.Length > 0) {
			clipIndex = Random.Range(0, ambientClips.Length);
			ambientAudioSource.PlayOneShot(ambientClips[clipIndex]);
		}
	}

	private void playRandomMusic()
	{
		int clip = Random.Range(1, music.Length);
		MusicAudioSource.clip = music[clip];
	}

	private void playRandomMusic(AudioClip[] audioClips)
	{
		if (!MusicAudioSource.isPlaying && audioClips.Length > 0) {
			int clip = Random.Range(0, audioClips.Length);
			MusicAudioSource.clip = audioClips[clip];
		}
	}

	public void PlayMusicForScene(int index)
	{
		if (music.Length > 0) {
			MusicAudioSource.clip = music[index];
			if (LevelManager.instance.currentScene == "MikeTest") {
				MusicAudioSource.volume = 0;
				audioVolume = 0f;
			}
		}
	}

	public void StartAudio(){
		MusicAudioSource.Play();
	}

	public void SetButtonClip(){
		SFXAudioSource.pitch = 1f;
		SFXAudioSource.PlayOneShot(buttonClick, 2f);
	}

	public void PlayWalkClip() {
		SFXAudioSource.pitch = 1f;
		SFXAudioSource.PlayOneShot(movementClips[1], .2f);
	}

	public void PlayRunClip()
	{
		SFXAudioSource.pitch = 1f;
		SFXAudioSource.PlayOneShot(movementClips[2], .2f);
	}

	public void PlayHurtClip() {
		SFXAudioSource.pitch = Random.Range(.95f, 1.05f);
		SFXAudioSource.PlayOneShot(hurtClip);
	}

	public void PlayDeathClip()
	{
		SFXAudioSource.pitch = 1f;
		SFXAudioSource.PlayOneShot(deathClip);
	}

	public void PlayDestructibleSound(AudioClip clip)
	{
		SFXAudioSource.PlayOneShot(clip);
	}

	public void BabyChickCaughtSound(AudioClip clip, Vector3 point)
	{
		SFXAudioSource.pitch = 1f;
		SFXAudioSource.PlayOneShot(clip);
		AudioSource.PlayClipAtPoint(clip, point);
	}

	public void ChangeMasterVolume(float volume) {
		audioMixer.SetFloat("Master", volume);
		if (volume == -40f){
			audioMixer.SetFloat("Master", -80f);
		}
	}

	public void ChangeMusicVolume(float volume){
		audioMixer.SetFloat("Music", volume);
		if (volume == -40f){
			audioMixer.SetFloat("Music", -80f);
		}
	}

	public void ChangeSFXVolume(float volume){
		audioMixer.SetFloat("SFX", volume);
		if (volume == -40f){
			audioMixer.SetFloat("SFX", -80f);
		}
	}
}
