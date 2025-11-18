using UnityEngine;

public class LevelStateTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colidiu");

        if (other.gameObject.CompareTag("Player"))
        {
            LevelManager levelManager = LevelManager.instance;
            LevelStateMachine levelStateMachine = levelManager.GetLevelStateMachine;

            levelStateMachine.SetState(new LevelFlashbackTwoState(levelStateMachine, levelManager));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {


        
    }
}
