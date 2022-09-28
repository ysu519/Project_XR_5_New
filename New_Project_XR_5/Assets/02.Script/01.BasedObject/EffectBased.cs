using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBased : MonoBehaviour
{
    [System.Serializable]
    public struct EffectInfo
    {
        public float fYpos;//캐릭터 등 이펙트 발생 위치에서 y로 얼마나 옮길지
        public float fXpos;//위와 동일 x
        public float fFirstDelay;//선딜
        public float fTime;//이펙트 지속시간 보통 5로 두면 될듯
        public bool bIsRotation;//캐릭터의 회전벡터만큼 이펙트를 돌릴지 유무

        public GameObject EffectObj;//이펙트 프리팹
        
    };
    public List<EffectInfo> CEffect;

    protected List<Queue<GameObject>> C_EffectPool;//딕셔너리 오브젝트풀 싱글톤 스크립트로 따로 빼기

    public virtual void Start()
    {
        C_EffectPool = new List<Queue<GameObject>>();

        for (int i = 0; i < CEffect.Count; i++)
        {
            Queue<GameObject> _queue = new Queue<GameObject>();

            if (CEffect[i].EffectObj.GetComponent<ParticleSystem>().loop)
            {
                CEffect[i].EffectObj.SetActive(false);
            }
            else
            {
                GameObject obj = Instantiate(CEffect[i].EffectObj);

                obj.transform.parent = transform;
                obj.SetActive(false);
                _queue.Enqueue(obj);
            }

            C_EffectPool.Add(_queue);
        }
    }

    public virtual void OnEffect(int i, Transform _transform)//반복되는 루프 이펙트 키는 용
    {
        CEffect[i].EffectObj.SetActive(true);
    }
    public virtual void OfffEffect(int i)//반복되는 루프 이펙트 끄는 용
    {
        CEffect[i].EffectObj.SetActive(false);
    }

    public virtual IEnumerator TargetEffect(int i, Transform _transform)
    {
        if (C_EffectPool[i].Count == 0)
        {
            GameObject obj2 = Instantiate(CEffect[i].EffectObj);
            obj2.transform.parent = _transform;
            C_EffectPool[i].Enqueue(obj2);
        }

        GameObject obj;
        obj = C_EffectPool[i].Dequeue();


        yield return new WaitForSeconds(CEffect[i].fFirstDelay);

        obj.SetActive(true);
        obj.transform.position = new Vector3
            (_transform.position.x + CEffect[i].fXpos,
            _transform.position.y + CEffect[i].fYpos,
            _transform.position.z);

        if (CEffect[i].bIsRotation)
            obj.transform.rotation = _transform.rotation;

        obj.GetComponent<ParticleSystem>().Play();


        yield return new WaitForSeconds(CEffect[i].fTime);

        //yield return new WaitForSeconds(obj.GetComponent<ParticleSystem>().duration);
        obj.SetActive(false);

        C_EffectPool[i].Enqueue(obj);
    }
}
