using UnityEngine;

public class EnemyHpbarController : MonoBehaviour
{
    public SpriteRenderer fillBar;
    public void UpdateHealthBar(int curHp, int maxHp)
    {
        float hpRatio = (float)curHp / (float)maxHp;
        fillBar.transform.localScale = new Vector3(hpRatio, 1, 1);
    }
}
