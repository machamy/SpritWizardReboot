using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.World
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Vector2Int coordinates;
        private List<Entity.Entity> entities = new List<Entity.Entity>();
        public Vector2Int Coordinates
        {
            get => coordinates;
        }
        
        public void Initialize(Vector2Int coordinates)
        {
            this.coordinates = coordinates;
        }
        
        public bool IsClear()
        {
            return entities.Count == 0;
        }
        
        public List<Entity.Entity> GetEntities()
        {
            return entities;
        }
        
        public void AddEntity(Entity.Entity entity)
        {
            entities.Add(entity);
        }
        
        #region Debug

        private float colorDuration = -10f;
        [SerializeField] private SpriteRenderer debugRenderer;
        /// <summary>
        /// 해당 시간만큼 해당 타일의 색을 바꿈
        /// </summary>
        /// <param name="color"></param>
        /// <param name="duration"></param>
        public void ShowDebugColor(Color color, float duration = 1.5f)
        {
            debugRenderer.color = color;
            if (colorDuration <= 0)
            {
                StartCoroutine(ColorDebugRoutine());
            }
            colorDuration = duration;
        }
        
        private IEnumerator ColorDebugRoutine()
        {
            yield return null;
            while (colorDuration > 0)
            {
                colorDuration -= Time.deltaTime;
                yield return null;
            }
            debugRenderer.color = Color.clear;
            colorDuration = 0f;
        }

        #endregion

    }
}