﻿using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.World
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Vector2Int coordinates;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer focusRenderer;
        private List<Entity.Entity> entities = new List<Entity.Entity>();
        public Vector2Int Coordinates
        {
            get => coordinates;
        }
        private Color defaultColor;

        private void Start()
        {
            defaultColor = spriteRenderer.color;
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
        
        public void RemoveEntity(Entity.Entity entity)
        {
            entities.Remove(entity);
        }

        public void Focus() => Focus(new Color(0.7f, 1f, 0f, 0.5f));
        public void Focus(Color focusColor)
        {
            spriteRenderer.color = focusColor;
        }
        
        public void Unfocus()
        {
            spriteRenderer.color = defaultColor;
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