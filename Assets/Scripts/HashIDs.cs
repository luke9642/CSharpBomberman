using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
    public int WalkingBool;
    public int DyingBool;

    private void Awake()
    {
        WalkingBool = Animator.StringToHash("Walking");
        DyingBool = Animator.StringToHash("Dying");
    }
}