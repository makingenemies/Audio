using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class PersistingSound : MonoBehaviour
    {
        private static List<PersistingSound> _persistingSounds;
        
        [Tooltip("Unique id for this persisting sound to avoid duplicates")]
        [SerializeField]
        private string _id;
        
        [Tooltip("Scenes where it should be active")]
        [SerializeField]
        private string[] _scenes;
        
        private AudioSource _audioSource;

        public string Id => _id;

        public void Awake()
        {
            var existingSound = GetExistingPersistingSound();
            if (existingSound != null && existingSound != this)
            {
                Destroy(gameObject);
                return;
            }

            RegisterSound();
            DontDestroyOnLoad(this);
            
            _audioSource = GetComponent<AudioSource>();
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private PersistingSound GetExistingPersistingSound()
        {
            if (_persistingSounds == null) return null;
            return _persistingSounds.SingleOrDefault(s => s.Id == _id);
        }

        private void RegisterSound()
        {
            if (_persistingSounds == null)
            {
                _persistingSounds = new List<PersistingSound>();
            }
            
            _persistingSounds.Add(this);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            UpdateAudioState();
        }

        private void UpdateAudioState()
        {
            if (_scenes.Contains(SceneManager.GetActiveScene().name))
            {
                if (!_audioSource.isPlaying) _audioSource.Play();
            }
            else
            {
                _audioSource.Stop();
            }
        }
    }
}