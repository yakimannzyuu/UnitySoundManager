using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YakSE
{
    public class SoundPlayer : MonoBehaviour
    {
        static SoundPlayer instance;
        public MusicData MusicData;
        public AudioSource[] MusicSource { get; private set; }
        public bool ActiveMusicSource { get; private set; }
        public int MusicNumber { get; private set; }

        float volume = 1.0f;

        public static SoundPlayer Instance {
            get {
                if (!instance) { Debug.LogError("見つかりません。"); return null; }
                return instance;
            }
        }

        public AudioSource MainMusicSource
        {
            get { return MusicSource[ActiveMusicSource ? 0 : 1]; }
        }

        void Awake()
        {
            if (instance) { Debug.LogError("複数あります。", transform); return; }
            instance = this;

            MusicSource = new AudioSource[2];
            MusicSource[0] = gameObject.AddComponent<AudioSource>();
            MusicSource[1] = gameObject.AddComponent<AudioSource>();
            ActiveMusicSource = true;

            MusicNumber = 0;
            if(MusicData == null || MusicData[0] == null)
            {
                Debug.LogError("MusicDataがありません。");
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        void Start()
        {
            PlayMusic(0);
        }

#if Sound
        float time = 0;
        void Update()
        {
            if(MusicData[MusicNumber].EndTime > 0 && 0 > time + MusicData[MusicNumber].MusicLength - Time.realtimeSinceStartup)
            {
                PlayMusic(MusicData[MusicNumber].NextMusicName);
            }
        }
#endif

        public void PlayMusic(int _number)
        {

            if (MusicData[_number] == null) { return; }
            else { MusicNumber = _number; }
            StopAllCoroutines();
            if(MusicData[MusicNumber].CrossFadeTime > 0)
            {
                ActiveMusicSource = !ActiveMusicSource;
                StartCoroutine(CrossFadeUpdate(MusicData[MusicNumber].CrossFadeTime));
            }

            MainMusicSource.clip = MusicData[MusicNumber].Clip;
            if(MusicData[MusicNumber].StartTime < 0)
            {
                StartCoroutine(DelayPlay(-MusicData[MusicNumber].StartTime));
            }
            else
            {
                MainMusicSource.time = MusicData[MusicNumber].StartTime;
                MainMusicSource.Play();
            }

#if Sound
            time = Time.realtimeSinceStartup;
            Debug.Log("Music:" + MusicData[MusicNumber].Name);
#else
            if(MusicData[MusicNumber].EndTime > 0)
            {
                StartCoroutine(NextPlay(MusicData[MusicNumber].MusicLength));
            }
#endif
        }



        public void PlayMusic(string _name)
        {
            for(int i = 0; i < MusicData.Length; i++)
            {
                if(MusicData[i].Name == _name)
                {
                    PlayMusic(i);
                    return;
                }
            }
            Debug.Log("曲が見つかりませんでした。", transform);
        }

        public float Volume
        {
            get { return volume; }
            set
            {
                volume = Mathf.Max(0, Mathf.Min(2.0f, value));
                for(int i = 0; i < MusicSource.Length; i++)
                {
                    MusicSource[i].volume = volume;
                }
            }
        }

        public float MusicTime
        {
            get { return MainMusicSource.time; }
            set
            {
                value = Mathf.Max(0, Mathf.Min(SoundPlayer.Instance.MainMusicSource.clip.length - 0.01f, value));
                MainMusicSource.time = value;
            }
        }

        IEnumerator NextPlay(float _time)
        {
            yield return new WaitForSecondsRealtime(_time);
            PlayMusic(MusicData[MusicNumber].NextMusicName);
        }

        IEnumerator DelayPlay(float _time)
        {
            yield return new WaitForSecondsRealtime(_time);
            MainMusicSource.Play();
        }

        IEnumerator CrossFadeUpdate(float _time)
        {
            for(float _cur = 0; _cur < _time; _cur += Time.unscaledDeltaTime)
            {
                MusicSource[ActiveMusicSource ? 0 : 1].volume = _cur / _time * volume;
                MusicSource[ActiveMusicSource ? 1 : 0].volume = (1.0f - _cur / _time) * volume;
                yield return null;
            }
            MusicSource[ActiveMusicSource ? 0 : 1].volume = volume;
            MusicSource[ActiveMusicSource ? 1 : 0].Stop();
        }
    }
}
