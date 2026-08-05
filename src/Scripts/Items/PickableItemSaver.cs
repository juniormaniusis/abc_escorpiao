using UnityEngine;
using PixelCrushers;
using System;
using UnityEngine.SceneManagement;
using Assets.Scripts.Quest;

namespace Assets.Scripts.Items
{
    [AddComponentMenu("")] // Use wrapper instead.
    public class PickableItemSaver : PositionSaver
    {
        [Serializable]
        public class PickableItemData : PositionData
        {
            public bool isCollected;
            public ObjectType objectType;
            public string slotId;
        }

        private PickableItem pickableItem;

        public override void Awake()
        {
            base.Awake();
            pickableItem = GetComponent<PickableItem>();
            if (pickableItem == null)
            {
                Debug.LogError("PickableItem component not found on the GameObject.");
            }
        }

        public override string RecordData()
        {
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            var slotId = pickableItem.transform.parent?.name; // Assuming the slot's name is unique
            var data = new PickableItemData
            {
                scene = currentScene,
                position = target.transform.position,
                rotation = target.transform.rotation,
                isCollected = pickableItem.IsCollected,
                objectType = pickableItem.ObjectType,
                slotId = slotId
            };
            return SaveSystem.Serialize(data);
        }

        public override void ApplyData(string s)
        {
            if (usePlayerSpawnpoint && SaveSystem.playerSpawnpoint != null)
            {
                SetPosition(SaveSystem.playerSpawnpoint.transform.position, SaveSystem.playerSpawnpoint.transform.rotation);
            }
            else if (!string.IsNullOrEmpty(s))
            {
                var data = SaveSystem.Deserialize(s, new PickableItemData());
                if (data == null) return;

                if (data.scene == SceneManager.GetActiveScene().buildIndex || data.scene == -1)
                {

                    pickableItem.IsCollected = data.isCollected;
                    pickableItem.ObjectType = data.objectType;

                    if (!string.IsNullOrEmpty(data.slotId))
                    {
                        var cabinet = FindObjectOfType<ToysCarbinetInteractions>();
                        var slot = cabinet.GetSlotById(data.slotId);
                        if (slot != null)
                        {
                            cabinet.ColocarItemNoEspaco(pickableItem.gameObject, slot);
                        }
                    }
                    else { SetPosition(data.position, data.rotation); }
                }
            }
        }
    }
}