using UnityEngine;

namespace SimplePool
{
    public class PoolObject : MonoBehaviour
    {
        // 해당 오브젝트가 속한 오브젝트 풀링의 참조를 저장하는 변수
        private ObjectPool pool;

        // 오브젝트 풀링의 참조를 설정하는 함수
        public void SetPool(ObjectPool pool)
        {
            this.pool = pool;
        }

        // 오브젝트를 다시 오브젝트 풀링에 반환하는 함수
        public void ReturnToPool()
        {
            if (pool != null)
            {
                pool.ReturnObject(gameObject);
            }
            else
            {
                Debug.LogError("Object doesn't belong to a pool.");
                Destroy(gameObject);
            }
        }
    }
}
