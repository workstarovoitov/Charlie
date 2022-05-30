using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBossAggroStages : MonoBehaviour
{
    [SerializeField] UnitMain uMain;
    [SerializeField] EnemyActions eActions;

    [SerializeField] AggroStage[] stages;

    private int currentStage = 0;
    private int previousStage = 0;

    // Update is called once per frame

    private void Start()
    {
        UpdateStats();
    }
    void Update()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            if (uMain.uHealth.CurrentHealth > stages[i].HealthMin * uMain.uHealth.CurrentMaxHealth && uMain.uHealth.CurrentHealth <= stages[i].HealthMax * uMain.uHealth.CurrentMaxHealth)
            {
                currentStage = i;
                if (currentStage != previousStage)
                {
                    previousStage = currentStage;
                    UpdateStats();
                }
            }
        }
    }

    private void UpdateStats()
    {
        eActions.attackInterval = stages[currentStage].AttackInterval;
        eActions.defendChance = stages[currentStage].DefendChance;
        eActions.specialAttackChance = stages[currentStage].SpecialAttackChance;
        eActions.meleeAttackComboIndexes = new int[stages[currentStage].MeleeAttackComboIndexes.Length];
        stages[currentStage].MeleeAttackComboIndexes.CopyTo(eActions.meleeAttackComboIndexes, 0);
 
        eActions.rangedAttackComboIndexes = new int[stages[currentStage].RangedAttackComboIndexes.Length];
        stages[currentStage].RangedAttackComboIndexes.CopyTo(eActions.rangedAttackComboIndexes, 0);
    }
}



[System.Serializable]
public class AggroStage
{
    [SerializeField] [Range(0f, 1f)] private float healthMin;
    public float HealthMin
    {
        get => healthMin;
    }
    [SerializeField] [Range(0f, 1f)] private float healthMax;
    public float HealthMax
    {
        get => healthMax;
    }

    [SerializeField] private int[] meleeAttackComboIndexes;
    public int[] MeleeAttackComboIndexes
    {
        get => meleeAttackComboIndexes;
    }
    [SerializeField] private int[] rangedAttackComboIndexes;
    public int[] RangedAttackComboIndexes
    {
        get => rangedAttackComboIndexes;
    }
    [SerializeField]  [Range(0f, 1f)] private float defendChance = 0;
    public float DefendChance
    {
        get => defendChance;
    }

    [SerializeField]  [Range(0f, 1f)] private float specialAttackChance = 0;
    public float SpecialAttackChance
    {
        get => specialAttackChance;
    }

    [SerializeField] private float attackInterval = 5f;
    public float AttackInterval
    {
        get => attackInterval;
    }

}