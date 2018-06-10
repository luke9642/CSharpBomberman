using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{
    public int walkingBool;
    public int dyingBool;

    private void Awake()
    {
        walkingBool = Animator.StringToHash("Walking");
        dyingBool = Animator.StringToHash("Dying");
    }
}