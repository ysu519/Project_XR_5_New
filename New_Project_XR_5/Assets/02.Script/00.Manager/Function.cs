using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class Function : MonoBehaviour
    {
        public static Function Inst;
        public void Init()
        {
            Inst = this;
        }
        void Start()
        {
            Init();
        }

        //쓸일은 잘 없을거 같은데 특정 컴포넌트 내용 싹다 복사 하는거임
        // a = b 했을때 레퍼런스로 가져오게되는 컴포넌트들에 씀
        public Component CopyComponent(Component _original, GameObject _destination)
        {
            System.Type type = _original.GetType();
            Component copy = _destination.AddComponent(type);
            // Copied fields can be restricted with BindingFlags
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(_original));
            }
            return copy;
        }

        

        public float SmoothMove(float _currentval, float _targetval, float lastval, float smoothTime)
        {
            float smooth = Mathf.SmoothDamp(_currentval, _targetval,
                                               ref lastval, smoothTime);
            return smooth;
        }
    }
}