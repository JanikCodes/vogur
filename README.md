## Frameworks
We're using [AI Tree](https://renownedgames.gitbook.io/ai-tree/) for our AI logic. It's a behavior tree framework.

Currently our **Factions** and **Armies** are using that logic.

## Unit Test
We want to use  [Unit testing](https://docs.unity3d.com/Packages/com.unity.test-framework@1.3/manual/index.html) to write **better and clean** code.

## Component Based Development
This project is working with a Component Based Architecture. Therefore you'll see a lot of components, some generic and some non generic ones. Components allow us to easily share functionality across different GameObjects and encourages modularity.

## Quests
Our quest system is flexible and completly modable. We're storing all quests inside the `Quests.json`. We're using C# reflection to create `QuestObjectiveSO` at runtime based on a given `type` and `properties`.

### How to create a new Quest Objective Type?
To create a new quest objective type you simply create a new script with the naming scheme `Quest[YOUR_OBJECTIVE_TYPE]TypeSO.cs`.
The class needs to inherite from `QuestObjectiveSO.cs`.

Additionally most `ObjectiveTypes` needs extra variables to determine the `IsCompleted()`. We add all required variables for calculation in `QuestObjective.cs` ( Examples: `VisitedTarget` or `CurrencyRemaining`)

To use this new quest objective simply use `[YOUR_OBJECTIVE_TYPE]` in the `Quests.json`. 

Example:
```cs
public class QuestAquireCurrencyTypeSO : QuestObjectiveSO
{
    public int Amount;

    private QuestObjective data;

    public override void Initialize(QuestObjective questObjective)
    {
        data = questObjective;

        data.CurrencyRemaining = Amount;
        data.Self.GetComponent<CurrencyStorage>().OnCurrencyChanged += HandleCurrenyChange;
    }

    public override void UpdateObjective(QuestObjective questObjective)
    {
        // not needed
    }

    public override bool IsComplete(QuestObjective questObjective)
    {
        return data.CurrencyRemaining == 0;
    }

    private void HandleCurrenyChange(int changedAmount)
    {
        if (changedAmount > 0)
        {
            int remaining = Mathf.Max(0, data.CurrencyRemaining - changedAmount);
            data.CurrencyRemaining = remaining;
        }
    }
}

```
This is how it would look like to use this newly added quest objective type
```json
{
    "Text": "Earn some cash along the way",
    "Type": "AquireCurrency",
    "Properties": {
        "Amount": 5000
    }
}
```

### How to create a new condition?
To create a new dialogue requirement you simply create a new script with the naming scheme `Dialogue[YOUR_REQUIREMENT_NAME]RequirementSO.cs`.
The class needs to inherite from `DialogueRequirementSO.cs`.

To use this new dialogue requirement simply use `[YOUR_REQUIREMENT_NAME]` in the `DialogueChoices.json`. 

Example:
```cs
public class DialogueCurrencyRequirementSO : DialogueRequirementSO
{
    public int RequiredCurrency;

    public override bool CheckRequirements(Transform self, Transform other)
    {
        Transform selectedTransform = Self ? self : other;

        CurrencyStorage currencyStorage = selectedTransform.GetComponent<CurrencyStorage>();
        return currencyStorage.HasEnoughCurrency(RequiredCurrency);
    }
}
```
This is how it would look like to use this newly added dialogue requirement
```json
{
    "Type": "Currency",
    "Properties": {
        "Self": true,
        "RequiredCurrency": 6969
    }
}
```
