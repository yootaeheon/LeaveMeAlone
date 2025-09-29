using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillController : MonoBehaviour
{
    [SerializeField] SkillDataSO _skillData;

    List<GameObject> _targets = new List<GameObject>();

    public virtual void UseSkill(GameObject user, List<GameObject> targets) { }
}
