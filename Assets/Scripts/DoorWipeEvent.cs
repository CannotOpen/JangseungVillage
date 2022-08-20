using UnityEngine;

public class DoorWipeEvent : MonoBehaviour
{
    public void OnStartDoorWipeClose()
    {
        AudioManager.Inst.PlayOneShot(SoundName.SFX_DoorClose);
    }

    public void OnCompleteDoorWipeClose()
    {
        AudioManager.Inst.PlayOneShot(SoundName.SFX_DoorShut);
    }

    public void OnStartDoorWipeOpen()
    {
        AudioManager.Inst.PlayOneShot(SoundName.SFX_DoorOpen_Last);
    }
}