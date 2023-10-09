using System;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public class MusicController : MonoBehaviour
    {
        private static MusicController instance;
        
        public static MusicController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<MusicController>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("MusicController");
                        instance = go.AddComponent<MusicController>();
                    }
                }
                return instance;
            }
        }

        [SerializeField] private GameObject songPlayer;
        private AudioSource radioSource;
        [SerializeField] private AudioClip[] songs;

        public bool radioStart = false;
        private int songIndex = 0;
        private List<AudioClip> playlist;
        private Random _random;
        private int finishedCount;
        
        private void Start()
        {
            playlist = new List<AudioClip>();
            _random = new Random();
            radioSource = songPlayer.GetComponent<AudioSource>();
            StartupRadio();
        }

        public void PlayRadio()
        {
            if (!radioStart)
            {
                radioStart = true;
                PlaySongs();
            }
            else
            {
                NextSong();
            }
        }

        private void StartupRadio()
        {
            ShuffleSongs();
            
        }
        private void Update()
        {
            bool playing = GameLogic.Instance.isLoaded;
            if (!radioSource.isPlaying && playing)
            {
                finishedCount++;
                if (finishedCount > 1)
                {
                    NextSong();
                }
            }
            else
            {
                finishedCount = 0;
            }
        }

        private void ShuffleSongs()
        {
            List<int> usedSongs = new List<int>();
            int songLength = songs.Length;
            for (int i = 0; i < songs.Length; i++)
            {
                int songNum = _random.Next(0, songLength);
                while (usedSongs.Contains(songNum))
                {
                    songNum = _random.Next(0, songLength);
                }
                
                
                playlist.Add(songs[songNum]);
                usedSongs.Add(songNum);
            }
        }
        private void NextSong()
        {
            songIndex++;
            
            if (songIndex >= playlist.Count) songIndex=0;
            radioSource.clip = playlist[songIndex];
            radioSource.Play();
        }

        private void PlaySongs()
        {
            if (songIndex >= playlist.Count) return;
            radioSource.clip = playlist[songIndex];
            radioSource.Play();
        }
    }
}