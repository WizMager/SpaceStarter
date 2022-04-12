using System.Collections.Generic;

namespace Pool
{
    public abstract class AbstractPool<TTemplate, TStorageType>
    {
        protected TTemplate _template;
        protected Stack<TStorageType> _storage;

        protected AbstractPool(TTemplate template)
        {
            _template = template;
            _storage = new Stack<TStorageType>();
        }

        public virtual void Push(TStorageType storageType)
        {
            _storage.Push(storageType);
        }

        public virtual TStorageType Pop()
        {
            if (_storage.Count > 0)
            {
                return _storage.Pop();
            }

            return Create();
        }

        protected abstract TStorageType Create();

        public virtual void Fill(int storageAmount)
        {
            for (int i = 0; i < storageAmount; i++)
            {
                Push(Create()); 
            }
        }

    }
}