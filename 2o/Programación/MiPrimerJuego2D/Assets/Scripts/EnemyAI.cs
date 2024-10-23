using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnermyAI : MonoBehaviour
{

    public enum EnemyState { Rise, Walk, Bury, Die };
    public EnemyState enemyState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StateMachine();

    }

    void StateMachine()
    {
        switch (enemyState)
        {
            case EnemyState.Rise:
                Rise();
                break;
            case EnemyState.Walk:
                Walk();
                break;
            case EnemyState.Bury:
                Bury();
                break;
            case EnemyState.Die:
                Die();
                break;
            default:
                break;
        }
    }

    void Rise()
    {

    }

    void Walk()
    {

    }

    void Bury()
    {

    }

    void Die()
    {

    }

    private void SetState()
    {
        
    }
}
