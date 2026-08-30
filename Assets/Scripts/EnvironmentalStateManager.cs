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
    [SerializeField]
    NarrativeCardManager narrativeManager;
    SceneFader SF;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GamePhase = 0;
        if (FridgeDoorController == null)
        {
            FridgeDoorController = GameObject.Find("SM_Fridge/Cube");
        }

        if (compScreenManager == null)
        {
            compScreenManager = GameObject.Find("ComputerScreenManager");
        }

        if (narrativeManager == null)
        {
            narrativeManager = GameObject.Find("NarrativeManager").GetComponent<NarrativeCardManager>();
        }
        narrativeManager.StartNewSequence();

        SF = GameObject.Find("FaderImage").GetComponent<SceneFader>();
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

    public void AdvanceTime()
    {
        FadeToBlack();
        GamePhase++;
        compScreenManager.SendMessage("ProgressToNextSequence",GamePhase);
        if (GamePhase == 4)
        {
            narrativeManager.StartNewSequence(CalculateEnding());
        }
        else
        {
            narrativeManager.StartNewSequence();
        }
    }

    void FadeToBlack()
    {
        Debug.Log("Fade In");
        SF.fadeDirection = SceneFader.FadeDirection.In;
        SF.RunFade();
    }

    public void FadeFromBlack()
    {
        Debug.Log("Fade Out");
        SF.fadeDirection = SceneFader.FadeDirection.Out;
        SF.RunFade();
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
