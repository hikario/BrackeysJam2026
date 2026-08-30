using UnityEngine;

public enum Ending
{
    NONE,
    CAT,
    ROOMMATE,
    INSUFFICIENT,
    PEPE_SILVA
}

public class EnvironmentalStateManager : MonoBehaviour
{
    [SerializeField]
    GameObject FridgeDoorController;
    int GamePhase;
    GameObject compScreenManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (FridgeDoorController == null)
        {
            FridgeDoorController = GameObject.Find("SM_Fridge/Cube");
        }

        if (compScreenManager == null)
        {
            compScreenManager = GameObject.Find("ComputerScreenManager");
        }
    }

    // Open Fridge Door
    void OpenFridge()
    {
        FridgeDoorController.SendMessage("SetBottomDoorOpen");
    }

    void CloseFridge()
    {
        FridgeDoorController.SendMessage("SetBottomDoorClosed");
    }

    void AdvanceTime()
    {
        GamePhase++;
        compScreenManager.SendMessage("ProgressToNextSequence",GamePhase);
    }

    int GetTime()
    {
        return GamePhase;
    }


    public static Ending CalculateEnding()
    {
        int correct_evidence = 0;
        int incorrect_evidence = 0;
        int total_correct_evidence = 0;
        int total_incorrect_evidence = 0;

        foreach (Evidence evidence in ComputerScreenManager.instance.evidenceDefinitionReference.evidence)
        {
            if(evidence.catEvidence)
            {
                total_correct_evidence++;
                if(evidence.evidenceIsCollected && evidence.flaggedAsRelevant)
                {
                    correct_evidence++;
                }
            }
            else
            {
                total_incorrect_evidence++;
                if(evidence.evidenceIsCollected && evidence.flaggedAsRelevant)
                {
                    incorrect_evidence++;
                }
            }
        }

        if((correct_evidence == total_correct_evidence) && (incorrect_evidence == total_incorrect_evidence))
        {
            return Ending.PEPE_SILVA;
        }
        else if(correct_evidence >= total_correct_evidence/2)
        {
            if(correct_evidence > incorrect_evidence)
            {
                return Ending.CAT;
            }
            else if(correct_evidence == incorrect_evidence)
            {
                return Ending.INSUFFICIENT;
            }
            else
            {
                return Ending.ROOMMATE;
            }
        }
        else if(incorrect_evidence >= total_incorrect_evidence/2)
        {
            return Ending.ROOMMATE;
        }
        else
        {
            return Ending.INSUFFICIENT;
        }
    }
}
