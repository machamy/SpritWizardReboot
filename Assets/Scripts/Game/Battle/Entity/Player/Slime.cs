using System.Collections;
using System.Collections.Generic;
using Game.Entity;
using Game.World;
using Unity.Mathematics.Geometry;
using UnityEngine;
using Math = System.Math;

namespace Game.Player
{
    [RequireComponent(typeof(Entity.Entity))]
    public class Slime : MonoBehaviour
    {
        [SerializeField] SkillCaster skillCaster;
        public SkillCaster SkillCaster => skillCaster;
        
        private Entity.Entity entity;

        private void Awake()
        {
            entity = GetComponent<Entity.Entity>();
        }
    }
}