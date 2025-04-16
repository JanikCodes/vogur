using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class FactionTest : MonoBehaviour
{
    private FactionService factionService;

    [SetUp]
    public void Setup()
    {
        // TODO: Fix my issue with dependencies, I think I have to set up my tests in the same structure how my Services are structured, therefore certain tests rely on other tests? [ WorldTest -> FactionTest, StructureTest] ?
        GameObject factionServiceGO = new GameObject("test");
        factionService = factionServiceGO.AddComponent<FactionService>();
    }

    [Test]
    public void HasCreatedScriptableObjects()
    {
        Assert.Greater(factionService.GetAllScriptableObjects().Count, 0);
    }
}
