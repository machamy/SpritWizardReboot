using Game.Player;
using UnityEngine;

namespace Game
{
    public class BattleManager : Singleton<BattleManager>
    {
        public Board Board => board;
        [Header("Managers")]
        [SerializeField] private TurnManager turnManager;
        [SerializeField] private CardCastManager cardCastManager;
        [Header("References")]
        [SerializeField] private Board board;
        [Header("Slime")]
        [SerializeField] private Slime iceSlime;
        [SerializeField] private Slime grassSlime;
        [SerializeField] private Slime fireSlime;
        [Header("Deck")]
        [SerializeField] private Deck startDeck;
        [SerializeField] private Deck drawDeck;
        [SerializeField] private Deck discardDeck;
        public Deck StartDeck => startDeck;
        public Deck DrawDeck => drawDeck;
        public Deck DiscardDeck => discardDeck;
        [Header("Battle")]
        [SerializeField] private bool isOnBattle = false;
        public bool IsOnBattle => isOnBattle;
        
        private void Awake()
        {
            board = FindAnyObjectByType<Board>();
        }
        
        public void StartBattle()
        {
            isOnBattle = true;
            turnManager.StartGame();
            
        }
        
        
        public Slime GetSlime(SkillCaster caster)
        {
            return caster switch
            {
                SkillCaster.Ice => iceSlime,
                SkillCaster.Grass => grassSlime,
                SkillCaster.Fire => fireSlime,
                _ => null
            };
        }
    }
}