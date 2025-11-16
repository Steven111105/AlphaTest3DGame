using UnityEngine;

public class ReloadScript : MonoBehaviour
{
    public void ReloadComplete()
    {
        PlayerAttack.instance.FinishReload();
    }
}
