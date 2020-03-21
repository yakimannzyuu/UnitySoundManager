using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YakSE
{
    [CreateAssetMenu(menuName = "Sound/MusicData", fileName = "MusicData")]
    public class MusicData : ScriptableObject
    {
        [SerializeField]
        MusicSetting[] Data = { };

        public MusicSetting this[int _num]
        {
            get
            {
                if(_num < Data.Length && _num >= 0)
                {
                    return Data[_num];
                }
                Debug.Log("範囲外を選曲しました。");
                return null;
            }
        }

        public int Length
        {
            get { return Data.Length; }
        }
    }

    [System.Serializable]
    public class MusicSetting
    {
        public string Name = "start";
        public AudioClip Clip = null;
        public string[] NextMusicNames = { "start" };
        public float StartTime = 0;
        public float EndTime = 0;
        public float CrossFadeTime = 0;

        public float MusicLength
        { get { return EndTime - StartTime; } }

        public string NextMusicName
        {
            get
            {
                int num = Random.Range(0, NextMusicNames.Length - 1);
                return NextMusicNames[num];
            }
        }
    }
}
