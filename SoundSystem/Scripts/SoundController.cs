using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YakSE;
using UnityEditor;

public class SoundController : MonoBehaviour
{
    public string SoundName = "";
    public float Volume = 1.0f;
    public float Time = 0;
    float time = 0;

    void Update()
    {
        SoundPlayer.Instance.Volume = Volume;

        if(Time != time)
        {
            SoundPlayer.Instance.MusicTime = Time;
            time = Time;
        }
        else
        {
            time = SoundPlayer.Instance.MusicTime;
            Time = SoundPlayer.Instance.MusicTime;
        }
    }
}

[CustomEditor(typeof(SoundController))]
public class SoundControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Change"))
        {
            SoundPlayer.Instance.PlayMusic("ikusa");
        }
    }
}
