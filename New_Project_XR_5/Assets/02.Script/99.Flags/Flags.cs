using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYEREFFECT
{
    HIT, DEATH, WALK, JUMP, ATTACK
};

public enum INPUT //입력 키 목록
{
    UP, DOWN, LEFT, RIGHT, JUMP,
    ATTACK_LIGHT, DASH,
    ENTER, CANCEL
}

public enum ACTION
{
    IDLE, WALK, JUMP, ATTACK, DASH, HIT, DEATH
}
public enum STATE
{
    STAND, FLOAT
}

namespace SpineEnum
{//SpineBased에 CSpine안에 넣은 애니메이션 순서대로
    public enum PLAYER
    {
        IDLE, JUMP, ATTACK, HIT, DEATH
    }
    public enum AMMUT
    {

    }
}