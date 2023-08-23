// TODO: Refactor this into something more generic. This was quite quickly taken from one of my old projects.

////using System;
////using System.Collections.Generic;
////using Elephant.UnityLibrary.Core;
////using UnityEngine;

////namespace Elephant.UnityLibrary.Audio
////{
////	/// <summary>
////	/// Version 1.00
////	/// Unity can't show Dictionaries in the inspector so that's why I use an array and a privately hidden dictionary.
////	/// This manager only supports 2D audio.
////	///
////	/// Instructions:
////	/// Create an empty game object and add this script along with 2x AudioSource to it.
////	/// Remove the PlayOnAwake from those AudioSources and set the SpatialBlend to 0 (2D), if not already.
////	/// Populate the EditorSFX and EditorMusic settings in the Inspector for this script.
////	/// </summary>
////	[RequireComponent(typeof(AudioSource))]
////	[RequireComponent(typeof(AudioSource))]
////	public class AudioMgr2D : Singleton<AudioMgr2D>
////	{
////		private const string MasterVolumePrefs = "MasterVolume";
////		private const string SFXVolumePrefs = "SFXVolume";
////		private const string MusicVolumePrefs = "MusicVolume";
////		private const float DefaultMasterVolume = 0.5f;
////		private const float DefaultSFXVolume = 0.5f;
////		private const float DefaultMusicVolume = 0.5f;

////		[SerializeField] private SFXPair[] _editorSFX = Array.Empty<SFXPair>();
////		private readonly Dictionary<ESFX, AudioClip> _allSFX = new Dictionary<ESFX, AudioClip>();

////		[SerializeField] private MusicPair[] _editorMusic = Array.Empty<MusicPair>();
////		private readonly Dictionary<EMusic, AudioClip> _allMusic = new Dictionary<EMusic, AudioClip>();

////		/// <summary>
////		/// Used for playing all 2D effects.
////		/// </summary>
////		private AudioSource _sfxSource;

////		/// <summary>
////		/// Used for playing all Music.
////		/// </summary>
////		private AudioSource _musicSource;

////		/// <summary>
////		/// Master volume.
////		/// </summary>
////		public float MasterVolume
////		{
////			get => AudioListener.volume;

////			set
////			{
////				if (AudioListener.volume == value)
////					return;

////				AudioListener.volume = value;
////				PlayerPrefs.SetFloat(MasterVolumePrefs, value);
////				PlayerPrefs.Save();
////			}
////		}

////		/// <summary>
////		/// Value must be between 0f and 1f.
////		/// </summary>
////		public float SFXVolume
////		{
////			get => _sfxSource.volume;

////			set
////			{
////				if (_sfxSource.volume == value)
////					return;

////				_sfxSource.volume = value;
////				PlayerPrefs.SetFloat(SFXVolumePrefs, value);
////				PlayerPrefs.Save();
////			}
////		}

////		/// <summary>
////		/// Value must be between 0f and 1f.
////		/// </summary>
////		public float MusicVolume
////		{
////			get => _musicSource.volume;

////			set
////			{
////				if (_musicSource.volume == value) { return; }
////				_musicSource.volume = value;
////				PlayerPrefs.SetFloat(MusicVolumePrefs, value);
////				PlayerPrefs.Save();
////			}
////		}

////		/// <summary>
////		/// Awake.
////		/// </summary>
////		protected void Awake()
////		{
////			// Copy the SFX configuration into a lookup table and perform a safety check.
////			for (int i = 0; i < _editorSFX.Length; i++)
////			{
////				if (_editorSFX[i].Clip == null)
////					throw new NullReferenceException(string.Format("The Clip for SFX {0} is null. Please assign it in the editor.", _editorSFX[i].SFX));
////				_allSFX.Add(_editorSFX[i].SFX, _editorSFX[i].Clip);
////			}
////			// Copy the Music configuration into a lookup table and perform a safety check.
////			for (int i = 0; i < _editorMusic.Length; i++)
////			{
////				if (_editorMusic[i].Clip == null)
////					throw new NullReferenceException(string.Format("The Clip for Music {0} is null. Please assign it in the editor.", _editorMusic[i].Music));
////				_allMusic.Add(_editorMusic[i].Music, _editorMusic[i].Clip);
////			}

////			AudioSource[] audioSources = GetComponents<AudioSource>();
////			if (audioSources.Length < 2)
////				throw new Exception("This class needs AudioSource x2.");

////			_sfxSource = audioSources[0];
////			_musicSource = audioSources[1];

////			LoadVolumes();
////		}

////		private void LoadVolumes()
////		{
////			AudioListener.volume = PlayerPrefs.GetFloat(MasterVolumePrefs, DefaultMasterVolume);
////			_sfxSource.volume = PlayerPrefs.GetFloat(SFXVolumePrefs, DefaultSFXVolume);
////			_musicSource.volume = PlayerPrefs.GetFloat(MusicVolumePrefs, DefaultMusicVolume);
////		}

////		/// <summary>
////		/// Play specified SFX.
////		/// </summary>
////		public void PlaySfx(ESFX sfx)
////		{
////			_sfxSource.PlayOneShot(_allSFX[sfx]);
////		}

////		/// <summary>
////		/// Stop all SFX.
////		/// </summary>
////		public void StopAllSfx()
////		{
////			_sfxSource.Stop();
////		}

////		/// <summary>
////		/// Stop all SFX and all music.
////		/// </summary>
////		public void StopEverything()
////		{
////			StopMusic();
////			StopAllSfx();
////		}

////		/// <summary>
////		/// Play specified music.
////		/// </summary>
////		public void PlayMusic(EMusic music)
////		{
////			_musicSource.clip = _allMusic[music];
////			_musicSource.Play();
////		}

////		/// <summary>
////		/// Stop all music.
////		/// </summary>
////		public void StopMusic()
////		{
////			_musicSource.Stop();
////		}
////	}

////	// Expand/change below 2 enums as you need.
////	public enum ESFX
////	{
////		None = 0,
////		ButtonClick,
////	}

////	public enum EMusic
////	{
////		None = 0,
////	}

////	[System.Serializable]
////	public struct SFXPair
////	{
////		public ESFX SFX;
////		public AudioClip Clip;
////	}

////	[System.Serializable]
////	public struct MusicPair
////	{
////		public EMusic Music;
////		public AudioClip Clip;
////	}
////}