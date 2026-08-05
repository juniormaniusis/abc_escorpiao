using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEngine;
namespace Assets.Scripts.GameManager
{
    public class UIManager : MonoBehaviour
    {
        public Texture2D[] cursors;
        public static UIManager Instance { get; private set; }
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void SetCursor(ObjectType objectType)
        {
            if (Instance == null) return;
            Cursor.SetCursor(Instance.cursors[(int)objectType], Vector2.zero, CursorMode.Auto);
        }
    }
}