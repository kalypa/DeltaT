using System.Collections.Generic;
using UnityEngine;

namespace SimplePool
{
    public class ObjectPool : MonoBehaviour
    {
        // 오브젝트 풀링에 사용될 프리팹
        public GameObject prefab;
        // 오브젝트 풀링의 초기 크기
        public int poolSize = 10;

        // 오브젝트 풀링에서 사용 가능한 오브젝트를 저장하는 리스트
        private List<GameObject> pool = new List<GameObject>();

        // 오브젝트 풀링을 초기화하는 함수
        public void Init(GameObject Obj, int cnt, Transform t)
        {
            // 초기 크기만큼 오브젝트를 생성하고 풀 리스트에 추가
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                pool.Add(obj);
            }
        }

        // 오브젝트 풀링에서 사용 가능한 오브젝트를 반환하는 함수
        public GameObject GetObject(Vector3 position, Quaternion rotation)
        {
            // 풀 리스트에서 사용 가능한 오브젝트를 찾아서 반환
            foreach (GameObject obj in pool)
            {
                if (!obj.activeInHierarchy)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }
            // 사용 가능한 오브젝트가 없으면 새로운 오브젝트를 생성해서 반환
            GameObject newObj = Instantiate(prefab);
            newObj.transform.SetParent(transform);
            pool.Add(newObj);
            return newObj;
        }

        // 오브젝트를 다시 오브젝트 풀링에 반환하는 함수
        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
        }
    }
}
