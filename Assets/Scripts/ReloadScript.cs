using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadScript : MonoBehaviour
{
    public void ReloadComplete()
    {
        PlayerAttack.instance.FinishReload();
    }
}
