using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MenuManagement;

public class SetBossName : MonoBehaviour
{
    [SerializeField] private int defaultMediumHealth = 250;

    private void Start()
    {
        ClearName();
        ClearHealthBar();
    }
    public void SetName()
    {
        GameMenu gm = FindObjectOfType<GameMenu>(true);
        gm.BossName.text = name;
    }
    public void ClearName()
    {
        GameMenu gm = FindObjectOfType<GameMenu>(true);
        gm.BossName.text = "";
    }
    public void SetHealthBar()
    {
        GameMenu gm = FindObjectOfType<GameMenu>(true);
        gm.BossHealth1.DefaultMediumHealth = defaultMediumHealth;
        gm.BossHealth1.SetTarget(gameObject);
    }
    public void ClearHealthBar()
    {
        GameMenu gm = FindObjectOfType<GameMenu>(true);
        gm.BossHealth1.SetTargetOff();
    }
}
