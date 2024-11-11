namespace Pooling
{
    public class PoolMono<T> where T : MonoBehaviour
    {
        public T Prefab { get; }
        public bool AutoExpand { get; set; }
        public Transform ContentHolder { get; }
        public List<T> pool;
        public PoolMono(T prefab, int count)
        {
            Prefab = prefab;
            ContentHolder = null;
            CreatePool(count);
        }
        public PoolMono(T prefab, int count, Transform ContentHolder)
        {
            Prefab = prefab;
            this.ContentHolder = ContentHolder;
            CreatePool(count);
        }
        private void CreatePool(int count)
        {
            pool = new List<T>();
            for (int i = 0; i < count; i++) CreateObject(); 
        }
        private T CreateObject(bool IsActiveByDefault = false)
        {
            var createdObject = Object.Instantiate(Prefab, ContentHolder);
            createdObject.gameObject.SetActive(IsActiveByDefault);
            pool.Add(createdObject);
            return createdObject;
        }
        public bool HasFreeElement(out T element)
        {
            foreach (var mono in pool)
                if (!mono.gameObject.activeInHierarchy) { element = mono; mono.gameObject.SetActive(true); return true; }
            element = null;
            return false;
        }
        public T GetFreeElement()
        {
            if (HasFreeElement(out var element))
                return element;
            if (AutoExpand)
                return CreateObject(true);
            throw new System.Exception("No elements");
        }
        public void ReleaseElement(T element) => element.gameObject.SetActive(false);
        public void ReleaseElement(int elementIndex) => pool[elementIndex].gameObject.SetActive(false);
    }
}
