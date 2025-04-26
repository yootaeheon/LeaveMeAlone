using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterModel : MonoBehaviour
{
    public float moveSpeed = 1f;

    public float attackRange = 2f;

    public float attackDamage = 10f;

    public float attackInterval = 1f;

    public float hp = 100;

    public LayerMask enemyLayer;
}
