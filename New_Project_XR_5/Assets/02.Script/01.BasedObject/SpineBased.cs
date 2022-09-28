using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


[System.Serializable]
public struct SpineInfo
{
    public int iTrack;
    [SpineAnimation]
    public string sAnim;
    public bool bLoop;
}

public class SpineBased : MonoBehaviour
{
    public List<SpineInfo> CSpine;

    public Dictionary<int, string> dictAnis;

    protected SkeletonAnimation SkeletonAnimation;

    void Start()
    {
        SkeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public virtual void SetAnimation(int _i)
    {
        SkeletonAnimation.state.SetAnimation(CSpine[_i].iTrack, CSpine[_i].sAnim, CSpine[_i].bLoop);
    }
}