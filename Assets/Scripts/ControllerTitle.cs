using UnityEngine;

public class ControllerTitle : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Inst.PlayBGM(AudioManager.Inst.GetClipFromPlaylist(SoundName.title.ToString()));
    }
}