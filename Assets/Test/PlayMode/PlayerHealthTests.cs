using System.Collections;
using NUnit.Framework;
using Platformer;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerHealthTests
{
    [UnityTest]
    public IEnumerator TakeDamage_ReduceHealth()
    {
        //Arrange
        var playerGO = new GameObject();
        var playerController = playerGO.AddComponent<PlayerController>();
        var healthPercentVariable = ScriptableObject.CreateInstance<FloatVariable>();
        healthPercentVariable.Value = 1f;
        playerController.CurrentHealthPercent = healthPercentVariable;

        //Act
        playerController.TakeDamage(30f);
        yield return null;

        //Assert
        Assert.AreEqual(0.7f, healthPercentVariable.Value);

        //Cleanup
        Object.Destroy(playerGO);
    }
}
