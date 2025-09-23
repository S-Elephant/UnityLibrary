#nullable enable

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Elephant.UnityLibrary.Extensions;
using UnityEngine;

namespace Elephant.UnityLibrary.Audio
{
	/// <summary>
	/// <para>Manages audio playback for sound effects and music within a 2D game.
	/// This includes playing, stopping,and toggling sound effects and music tracks.
	/// Uses the singleton pattern.</para>
	///
	/// <para>Usage:
	/// Create a new GameObject in your first scene, attach this script to it and
	/// drag and drop your SFX and music clips into this. Assign the AudioSources and
	/// configure them as needed.
	/// Then later to play something: AudioManager2D.Instance.PlaySfx("Jump")</para>
	/// </summary>
	public class AudioMgr2D : MonoBehaviour
	{
		/// <summary>
		/// Singleton instance of AudioManager2D to ensure only one instance is active.
		/// </summary>
		public static AudioMgr2D Instance { get; private set; } = null!;

		/// <summary>
		/// Component for playing sound effects.
		/// </summary>
		[SerializeField] protected AudioSource _sfxSource = null!;

		/// <summary>
		/// Component for playing music.
		/// </summary>
		[SerializeField] protected AudioSource _musicSource = null!;

		/// <summary>
		/// List of sound effect AudioClips available to play.
		/// </summary>
		[SerializeField] protected List<AudioClip> _sfxClips = new();

		/// <summary>
		/// List of music AudioClips available to play.
		/// </summary>
		[SerializeField] protected List<AudioClip> _musicClips = new();

		/// <summary>
		/// Library mapping sound effect names to their respective AudioClip.
		/// </summary>
		protected Dictionary<string, AudioClip> _sfxLibrary = new();

		/// <summary>
		/// Library mapping music track names to their respective AudioClip.
		/// </summary>
		protected Dictionary<string, AudioClip> _musicLibrary = new();

		/// <summary>
		/// Flag to enable or disable sound effects.
		/// </summary>
		public bool IsSfxEnabled { get; protected set; } = true;

		/// <summary>
		/// Flag to enable or disable music playback.
		/// </summary>
		public bool IsMusicEnabled { get; protected set; } = true;

		#region Volume Control

		/// <inheritdoc cref="MasterVolume"/>
		private float _masterVolume = 1.0f;

		/// <summary>
		/// Master volume, between 0.0f and 1.0f.
		/// </summary>
		public float MasterVolume
	{
			get => _masterVolume;
			set
			{
			_masterVolume = value;
			RefreshSfxVolume();
			RefreshMusicVolume();
			}
		}

		/// <inheritdoc cref="SfxVolume"/>
		private float _sfxVolume = 1.0f;

		/// <summary>
		/// Sfx volume, between 0.0f and 1.0f.
		/// </summary>
		public float SfxVolume
		{
			get => _sfxVolume;
			set
			{
				_sfxVolume = value;
				RefreshSfxVolume();
			}
		}

		/// <inheritdoc cref="MusicVolume"/>
		private float _musicVolume = 1.0f;

		/// <summary>
		/// Music volume, between 0.0f and 1.0f.
		/// </summary>
		public float MusicVolume
		{
			get => _musicVolume;
			set
			{
				_musicVolume = value;
				RefreshMusicVolume();
			}
		}

		#endregion

		#region Random Music

		/// <summary>
		/// Collection of all random musics played. Prevents playing the same music again and again.
		/// </summary>
		protected readonly List<string> AvailableRandomMusicKeys = new();

		/// <summary>
		/// Indicates if currently random music is being played.
		/// </summary>
		public bool IsPlayingRandomMusic { get; protected set; } = false;

		/// <summary>
		/// Previous random music <see cref="AudioClip"/> key.
		/// </summary>
		protected string? previousRndMusicKey = null;

		/// <summary>
		/// Random music loop Coroutine.
		/// </summary>
		protected Coroutine? RandomMusicLoopCoroutine = null;

		#endregion

		/// <summary>
		/// Initializes this AudioManager2D instance and prepares the sound effect and music libraries.
		/// Ensures that this object persists between scene loads.
		/// </summary>
		protected virtual void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				InitializeSelf();
			}
			else
			{
				// Allow no more than 1 instance.
				Destroy(gameObject);
			}
		}

		/// <summary>On Destroy.</summary>
		protected virtual void OnDestroy()
		{
			StopMusicLoopCoroutine();
		}

		/// <summary>
		/// Refresh the <see cref="_sfxSource"/> volume.
		/// </summary>
		protected virtual void RefreshSfxVolume() => _sfxSource.volume = _sfxVolume * MasterVolume;

		/// <summary>
		/// Refresh the <see cref="_musicSource"/> volume.
		/// </summary>
		protected virtual void RefreshMusicVolume() => _musicSource.volume = _musicVolume * MasterVolume;

		/// <summary>
		/// Initializes this AudioManager2D instance and prepares the sound effect and music libraries.
		/// </summary>
		private void InitializeSelf()
		{
			// Initialize the sound effects library.
			foreach (AudioClip clip in _sfxClips)
				_sfxLibrary[clip.name] = clip;

			// Initialize the music library.
			foreach (AudioClip clip in _musicClips)
				_musicLibrary[clip.name] = clip;
		}

		/// <summary>
		/// Optional initialization helper.
		/// </summary>
		public void InitializeSettings(bool isSfxEnabled, float sfxVolume, bool isMusicEnabled, float musicVolume)
		{
			IsSfxEnabled = isSfxEnabled;
			SfxVolume = sfxVolume;
			IsMusicEnabled = isMusicEnabled;
			SfxVolume = sfxVolume;
		}

		/// <summary>
		/// Plays a sound effect identified by name if sound effects are enabled.
		/// </summary>
		/// <param name="sfxKey">Name of the sound effect to play.</param>
		public void PlaySfx(string sfxKey)
		{
			// Note: PlayOneShotallows us to play a clip once, and it can overlap with other
			// clips played by PlayOneShot on the same AudioSource.
			if (IsSfxEnabled)
			{
				if (_sfxLibrary.TryGetValue(sfxKey, out AudioClip clip))
				{
					_sfxSource.PlayOneShot(clip);
				}
				else
				{
					Debug.LogWarning($"Attempted to play an unknown SFX clip, key: {sfxKey}. Aborting.");
				}
			}
		}

		/// <summary>
		/// Plays a music track identified by name if music is enabled. If the music track is already playing, it will not restart.
		/// </summary>
		/// <param name="musicKey">Name of the music track to play.</param>
		public void PlayMusic(string musicKey)
		{
			if (IsMusicEnabled)
			{
				if (_musicLibrary.TryGetValue(musicKey, out AudioClip clip))
				{
					if (_musicSource.clip != clip)
					{
						_musicSource.clip = clip;
						_musicSource.Play();
					}
				}
				else
				{
					Debug.LogWarning($"Attempted to play an unknown music clip, key: {musicKey}. Aborting");
				}
			}
		}

		/// <summary>
		/// Stop all music.
		/// </summary>
		public void StopAllMusic()
		{
			StopMusicLoopCoroutine();
			_musicSource.Stop();
		}

		#region Play Random Music

		/// <summary>
		/// Reset the available random music clip keys.
		/// </summary>
		public void ResetAvailableRandomMusic()
		{
			AvailableRandomMusicKeys.Clear();
		}

		/// <summary>
		/// Resets and then fills the <see cref="AvailableRandomMusicKeys"/>.
		/// </summary>
		/// <param name="match">Will only fill it with clips that have this string (case-insensitive) in their name somewhere.</param>
		protected void FillAvailableRandomMusic(string? match = null)
		{
			ResetAvailableRandomMusic();

			if (match == null)
			{
				AvailableRandomMusicKeys.AddRange(_musicLibrary.Keys);
			}
			else
			{
				match = match.ToLowerInvariant();
				AvailableRandomMusicKeys.AddRange(_musicLibrary.Keys.Where(m => m.ToLowerInvariant().Contains(match)));
			}
		}

		/// <summary>
		/// Play random music.
		/// Does nothing if there's no music configured.
		/// </summary>
		/// <param name="match">Partial <see cref="_musicLibrary"/> key to match for filtering the music.</param>
		/// <param name="playbackLoopModeType">Determines how and if music will be looped.</param>
		public void PlayRandomMusic(string? match = null, PlaybackLoopModeType playbackLoopModeType = PlaybackLoopModeType.LoopAllRandom)
		{
			if (!IsMusicEnabled || _musicLibrary.Count == 0)
				return;

			if (AvailableRandomMusicKeys.IsEmpty())
				FillAvailableRandomMusic(match);

			string rndMusicKey;
			do
			{
				rndMusicKey = AvailableRandomMusicKeys.GetRandom();
			}
			while (_musicLibrary.Count > 1 && rndMusicKey == previousRndMusicKey);
			AvailableRandomMusicKeys.Remove(rndMusicKey);

			IsPlayingRandomMusic = true;

			_musicSource.Stop();
			previousRndMusicKey = rndMusicKey;
			AudioClip musicClip = _musicLibrary[rndMusicKey];
			_musicSource.PlayOneShot(musicClip);

			RandomMusicLoopCoroutine = StartCoroutine(WaitAndPlayNextRandomMusic(musicClip.length, match, playbackLoopModeType));
		}

		/// <summary>
		/// Waits for the current music to end and if applicable will then play the next one.
		/// </summary>
		private IEnumerator WaitAndPlayNextRandomMusic(float musicClipLength, string? match, PlaybackLoopModeType playbackLoopModeType)
		{
			yield return new WaitForSeconds(musicClipLength);

			if (IsMusicEnabled && (playbackLoopModeType == PlaybackLoopModeType.LoopSingle || playbackLoopModeType == PlaybackLoopModeType.LoopAllRandom))
			{
				PlayRandomMusic(match, playbackLoopModeType);
			}
			else
			{
				IsPlayingRandomMusic = false;
				RandomMusicLoopCoroutine = null;
			}
		}

		/// <summary>
		/// Stop the <see cref="RandomMusicLoopCoroutine"/> and sets <see cref="IsPlayingRandomMusic"/> to false.
		/// </summary>
		protected void StopMusicLoopCoroutine()
		{
			try
			{
				if (RandomMusicLoopCoroutine != null)
				{
					StopCoroutine(RandomMusicLoopCoroutine);
					RandomMusicLoopCoroutine = null;
					IsPlayingRandomMusic = false;
				}
			}
			catch
			{
				// Do nothing.
			}
		}

		/// <summary>
		/// Play the next random music, aborting the current one (if any).
		/// Does nothing if currently not playing any random music.
		/// </summary>
		public void PlayNextRndMusic()
		{
			if (!IsPlayingRandomMusic)
				return;

			StopMusicLoopCoroutine();
			PlayRandomMusic();
		}

		#endregion

		/// <summary>
		/// Enable or disable SFX. Optionally also stop all running SFX if it's being disabled.
		/// </summary>
		/// <param name="isSfxEnabled">True to enable sound effects, false to disable them.</param>
		/// <param name="stopRunning">True to stop any running SFX if <paramref name="isSfxEnabled"/> is false.</param>
		public void SetSfxEnabled(bool isSfxEnabled, bool stopRunning = true)
		{
			IsSfxEnabled = isSfxEnabled;

			if (isSfxEnabled && stopRunning)
				_sfxSource.Stop();
		}

		/// <summary>
		/// Enable or disable music. Optionally also stop any running music if it's being disabled.
		/// </summary>
		/// <param name="isMusicEnabled">True to music sound effects, false to disable it.</param>
		/// <param name="stopRunning">True to stop the music if <paramref name="isMusicEnabled"/> is false.</param>
		public void SetMusicEnabled(bool isMusicEnabled, bool stopRunning = true)
		{
			IsMusicEnabled = isMusicEnabled;

			if (!isMusicEnabled && stopRunning)
			{
				_musicSource.Stop();
				_musicSource.clip = null;
				StopMusicLoopCoroutine();
			}
		}

		#region Config

		/// <summary>
		/// Returns the audio settings as a <see cref="AudioConfig"/>.
		/// </summary>
		/// <remarks>
		/// If you use Newtonsoft for serialization instead then you
		/// could use an Adapter pattern for the <see cref="AudioConfig"/>.
		/// </remarks>
		public virtual AudioConfig ToConfig()
		{
			return new AudioConfig(MasterVolume, SfxVolume, IsSfxEnabled, MusicVolume, IsMusicEnabled);
		}

		/// <summary>
		/// Apply the <paramref name="audioConfig"/> settings onto this <see cref="AudioMgr2D"/>.
		/// </summary>
		/// <param name="audioConfig">Configuration to apply.</param>
		public virtual void ApplyAudioConfig(AudioConfig audioConfig)
		{
			MasterVolume = audioConfig.MasterVolume;
			SfxVolume = audioConfig.SfxVolume;
			IsSfxEnabled = audioConfig.IsSfxEnabled;
			MusicVolume = audioConfig.MusicVolume;
			IsMusicEnabled = audioConfig.IsMusicEnabled;
		}

		#endregion
	}
}