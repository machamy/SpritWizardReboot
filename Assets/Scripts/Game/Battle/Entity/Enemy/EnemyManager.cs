using UnityEngine;

// 이거 매니저에 박는건지는 모르겠지만 일단 여기서 구현하고 필요시 딴데로 옮김
namespace Game
{
    public class EnemyManager : Singleton<EnemyManager>
    {
        [SerializeField]
        private EnemyDataSO enemyDataSO;
        [SerializeField]
        private EnemyAttackSO enemyAttackSo;
    }
}

