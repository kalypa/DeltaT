using System.Collections.Generic;
using UnityEngine;

namespace SimplePool
{
    public class PoolManager : SingleMonobehaviour<PoolManager>
    {
        // ObjectPool의 Dictionary를 저장하는 변수
        private Dictionary<string, ObjectPool> poolDictionary = new Dictionary<string, ObjectPool>();

        // 해당 타입의 오브젝트를 풀링하는 함수
        public void CreatePool<T>(T prefab, int count) where T : Component
        {
            // 오브젝트 풀링의 이름을 지정
            string poolName = prefab.name + " Pool";

            // 오브젝트 풀링이 존재하지 않을 때 새로 생성
            if (!poolDictionary.ContainsKey(poolName))
            {
                // 오브젝트 풀링을 생성하여 Dictionary에 추가
                ObjectPool newPool = new ObjectPool();
                newPool.Init(prefab.gameObject, count, transform);
                poolDictionary.Add(poolName, newPool);
            }
        }

        // 해당 타입의 오브젝트를 반환하는 함수
        public T GetPoolObject<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
        {
            // 오브젝트 풀링의 이름을 지정
            string poolName = prefab.name + " Pool";

            // 해당 이름의 오브젝트 풀링이 존재하지 않으면 오류 메시지 출력 후 함수 종료
            if (!poolDictionary.ContainsKey(poolName))
            {
                Debug.LogError($"Pool with name {poolName} doesn't exist.");
                return null;
            }

            // 해당 오브젝트 풀링에서 오브젝트를 가져옴
            GameObject poolObject = poolDictionary[poolName].GetObject(position, rotation);

            // 오브젝트 풀링에서 가져온 오브젝트에 PoolObject 컴포넌트를 추가함
            T component = poolObject.GetComponent<T>();
            PoolObject poolObjectComponent = poolObject.GetComponent<PoolObject>();

            if (component != null && poolObjectComponent == null)
            {
                poolObjectComponent = poolObject.AddComponent<PoolObject>();
            }

            // 오브젝트 풀링에서 가져온 오브젝트의 PoolObject 컴포넌트에 ObjectPool의 참조를 설정함
            if (poolObjectComponent != null)
            {
                poolObjectComponent.SetPool(poolDictionary[poolName]);
            }

            return component;
        }
    }
}
