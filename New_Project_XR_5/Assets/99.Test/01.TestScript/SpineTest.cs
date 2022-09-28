using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineTest : MonoBehaviour
{
    [SpineAnimation]
    public string anim;
    [SpineAnimation]
    public string anim2;
    [SpineAnimation]
    public string anim3;

    protected SkeletonAnimation SkeletonAnimation;


    // Start is called before the first frame update
    void Start()
    {
        SkeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SkeletonAnimation.state.SetAnimation(1, anim, false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            SkeletonAnimation.state.SetAnimation(0, anim2, true);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SkeletonAnimation.state.SetAnimation(0, anim3, true);
        }
    }
}