2020年3月21日 Soundについて。

※MusicPlayer
ScriptableObjectを利用して簡単に設定可能。
クロスフェードができる。

--使い方--
適当な場所にSound/MusicDataからファイル作成。
DataのSizeを1以上にする。

Nameに設定名を入れる。

Clipには対象の音楽。
　同じクリップが複数の設定に入っていてもOK。

NextMusicNameは次に再生する曲の設定名。
　自身の名前を割り当てるとループ再生になる。

StartTimeは再生を始める時間。デフォルトで0。
　マイナスも指定可能。

EndTimeは再生が終わる/切り替わる時間。
　デフォルトで0。その場合ループもしない。

CrossFadeTimeはクロスフェードする時間。
　デフォルトで0。その場合フェードしない。
　EndTimeからフェードアウトが始まり、
　次の曲のStartTimeから流れ始める。

プレファブのSoundManagerを適当に配置。
　スクリプトにMusicDataを設定。
　MusicDataのゼロ番目が自動再生される。
　　（スクリプトをenabledにするとされない）


--スクリプト--
using YakSE
SoundPlayer.Instance.PlayMusic("SoundName")
SoundPlayer.Instance.PlayMusic(番号)
SoundPlayer.Instance.MusicTime
SoundPlayer.Instance.Volume

※Defineに
　Sound
を入れるとまいフレーム終了時間確認とDebugログを出すようになる。



メモ
使った機能
　三項演算子 = a?b:c

　配列ファイル。
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
　
　Editor拡張。(SoundController内)
　　base.OnInspectorGUI();