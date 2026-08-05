using UnityEngine;

namespace Assets.Scripts.Items
{
    public interface ICollectable
    {
        public bool CanCollect();
        public void Collect();
        public void Drop();
        public void MarkAsCollected();
    }
}