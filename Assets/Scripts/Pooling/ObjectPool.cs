using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Pooling
{
    public class ObjectPool<T> where T : class
    {
        private readonly Stack<T> PoolStack;
        private readonly List<T> TakenObjects;
        private readonly Func<T> OnCreateFunc;
        private readonly Action<T> ActionOnGet;
        private readonly Action<T> ActionOnReturn;
        private readonly Action<T> ActionOnDestroy;
        private readonly int MaxSize;

        private int CountInactive => PoolStack.Count;

        public ObjectPool(
          Func<T> createFunc,
          Action<T> actionOnGet = null,
          Action<T> actionOnReturn = null,
          Action<T> actionOnDestroy = null,
          int defaultCapacity = 10,
          int maxSize = 10000)
        {
            if (createFunc == null)
            {
                throw new ArgumentNullException(nameof(createFunc));
            }
            if (maxSize <= 0)
            {
                throw new ArgumentException("Max Size must be greater than 0", nameof(maxSize));
            }

            PoolStack = new Stack<T>(defaultCapacity);
            TakenObjects = new List<T>();
            OnCreateFunc = createFunc;
            MaxSize = maxSize;
            ActionOnGet = actionOnGet;
            ActionOnReturn = actionOnReturn;
            ActionOnDestroy = actionOnDestroy;
        }

        public T GetFromPool()
        {
            T poolObj;
            if (PoolStack.Count == 0)
            {
                poolObj = OnCreateFunc();
            }
            else
            {
                poolObj = PoolStack.Pop();
            }

            TakenObjects.Add(poolObj);

            if (ActionOnGet != null)
                ActionOnGet(poolObj);
            return poolObj;
        }

        public void ReturnToPool(T poolObject)
        {
            if (ActionOnReturn != null)
                ActionOnReturn(poolObject);

            TakenObjects.Remove(poolObject);

            if (CountInactive < MaxSize)
            {
                PoolStack.Push(poolObject);
            }
            else
            {
                if (ActionOnDestroy != null)
                {
                    ActionOnDestroy(poolObject);
                }
            }
        }

        public void ReturnAll()
        {
            var TakenObjectsCopy = TakenObjects.ToList();

            foreach (var takenObject in TakenObjectsCopy)
            {
                ReturnToPool(takenObject);
            }
        }
    }
}
