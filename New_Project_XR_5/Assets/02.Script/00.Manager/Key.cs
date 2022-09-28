using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Key : MonoBehaviour
    {
        static public Key Inst;

        public Dictionary<INPUT, KeyCode> dictKeys;

        public void Init()
        {
            Inst = this;
            dictKeys = new Dictionary<INPUT, KeyCode>();

            SettingKeyboardInput();
        }
        private void Start()
        {
            Init();
        }
        void SettingKeyboardInput()
        {
            AddKey(INPUT.UP, KeyCode.UpArrow);//up은 메뉴창에서만 사용함
            AddKey(INPUT.DOWN, KeyCode.DownArrow);
            AddKey(INPUT.LEFT, KeyCode.LeftArrow);
            AddKey(INPUT.RIGHT, KeyCode.RightArrow);
            AddKey(INPUT.JUMP, KeyCode.Space);
            AddKey(INPUT.ATTACK_LIGHT, KeyCode.A);
            AddKey(INPUT.DASH, KeyCode.S);
            AddKey(INPUT.ENTER, KeyCode.Return);
            AddKey(INPUT.CANCEL, KeyCode.Escape);
        }

        void AddKey(INPUT InputEnum, KeyCode AddKey)
        {
            dictKeys.Add(InputEnum, AddKey);
        }

        public bool GetAction(INPUT input)
        {
            return Input.GetKey(dictKeys[input]);
        }
        public bool GetActionDown(INPUT input)
        {
            return Input.GetKeyDown(dictKeys[input]);
        }
        public bool GetActionUp(INPUT input)
        {
            return Input.GetKeyUp(dictKeys[input]);
        }
    }
}
