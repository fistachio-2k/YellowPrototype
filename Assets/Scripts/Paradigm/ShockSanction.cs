using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[CreateAssetMenu(menuName = "Paradigm/Sanctions/Shock")]
public class ShockSanction : SanctionSO
{
    public override void Apply(EnemyManager enemy)
    {
        enemy.StartCoroutine(ShockRoutine(enemy));
        
    }

    public IEnumerator ShockRoutine(EnemyManager enemy)
    {
         // Navigate to a go-to position if exist in the wraping ParadigmSO
        enemy.Ai.MoveToPoint(GameManager.Instance.PlayerTransform.position);
        
        yield return null;
        while (enemy.Ai.IsNavigating())
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // Initiate transition and reset the scene
        GameManager.Instance.InvokeShock();
    }
}
