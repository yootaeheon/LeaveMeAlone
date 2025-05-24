using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// <summary>
/// MonsterController가 최상위에 있어야하는데 Animator는 자식에 있어야함
/// Attack 애니메이션 이벤트를 호출해야하는데 스크립트 위치와 Animator 위치가 달라서 참조하여 호출할 수 있도록 임시방편 조치함
/// 추후 근본적인 문제는 StateBehaviour 고려해볼것
/// </summary>
public class TempAttack : MonoBehaviour
{
    [SerializeField] MonsterController MonsterController;

    public void Attack()
    {
        MonsterController.character.GetComponent<IDamageable>().TakeDamage(MonsterController.Base.AttackDamage);
    }
}
