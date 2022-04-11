using UnityEngine;
using View;

namespace MissilePool
{
    public class MissilePool : AbstractPool<GameObject, MissileView>
    {
        private readonly Transform _root;
        private readonly Transform _spawnTransform;
        
        public MissilePool(GameObject template, Transform root, int storageAmount, Transform spawnTransform) : base(template)
        {
            _root = root;
            _spawnTransform = spawnTransform;
            Fill(storageAmount);
        }

        public override void Push(MissileView view)
        {
            base.Push(view);
            view.transform.position = _root.position;
            view.transform.SetParent(_root);
            view.gameObject.SetActive(false);
        }

        public override MissileView Pop()
        {
            var instance = base.Pop();
            instance.transform.SetParent(null);
            instance.transform.SetPositionAndRotation(_spawnTransform.position, _spawnTransform.rotation);
            instance.gameObject.SetActive(true);
            return instance;
        }

        protected override MissileView Create()
        {
            var clone = Object.Instantiate(_template);
            var view = clone.GetComponent<MissileView>();

            return view;
        }
    }
}