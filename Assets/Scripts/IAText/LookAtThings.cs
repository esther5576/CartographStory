using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using DG.Tweening;
using System.Linq;

public class LookAtThings : MonoBehaviour
{
    Camera cam;
    MachineCall myMachineCall;
    PictureSystem myPictureSystem;

    void Start()
    {
        cam = GetComponent<Camera>();
        myMachineCall = GameObject.Find("GAME_MANAGER").GetComponent<MachineCall>();
        myPictureSystem = GetComponent<PictureSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            Debug.DrawRay(this.transform.position, this.transform.forward, Color.red);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(this.transform.position, hit.transform.position, Color.green);
                print("I'm looking at " + hit.transform.name);
                ObjectsID ScriptOnItem = hit.transform.GetComponent<ObjectsID>();
                if (ScriptOnItem != null)
                {
                    myPictureSystem.IDOfIlsandToAdd = ScriptOnItem.ID;
                    myPictureSystem.TakePic(GetRandomSentence(ScriptOnItem.ObjectDescription));
                }
            }
            else
            {
                print("I'm looking at nothing!");
            }
        }

    }

    public string GetRandomSentence(List<SentenceType> Ids)
    {
        List<string> ListComposedWithIds = new List<string>();
        foreach (var ID in Ids)
        {
            switch (ID)
            {
                case SentenceType.Areas:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Orange:
                    AddArrayToList(ListComposedWithIds, DataManager.OrangeSentences);
                    break;
                case SentenceType.Green:
                    AddArrayToList(ListComposedWithIds, DataManager.GreenSentences);
                    break;
                case SentenceType.Grey:
                    AddArrayToList(ListComposedWithIds, DataManager.GreySentences);
                    break;
                case SentenceType.Snow:
                    AddArrayToList(ListComposedWithIds, DataManager.SnowSentences);
                    break;
                case SentenceType.Submerged:
                    AddArrayToList(ListComposedWithIds, DataManager.SubmergedSentences);
                    break;
                case SentenceType.Remains:
                    AddArrayToList(ListComposedWithIds, DataManager.RemainsSentences);
                    break;
                case SentenceType.Boat:
                    AddArrayToList(ListComposedWithIds, DataManager.BoatSentences);
                    break;
                case SentenceType.Tree:
                    AddArrayToList(ListComposedWithIds, DataManager.TreeSentences);
                    break;
                case SentenceType.SubmergedPillar: // TODO : complete
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.SciencePillar:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Plank:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Ruin:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Wall:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Slab:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Rock:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Stone:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Wonder:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Snake:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Ring:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Orca:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.WhiteBuilding:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.MusicHole:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.windmill:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Rune:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Joy:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Knowledge:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Sun:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Day:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Beginning:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Destiny:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Wealth:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Eye:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                case SentenceType.Journey:
                    AddArrayToList(ListComposedWithIds, DataManager.AreasSentences);
                    break;
                default:
                    break;
            }
        }
        return ListComposedWithIds[Random.Range(0, ListComposedWithIds.Count)];
    }

    void AddArrayToList (List<string> List, string[] Array)
    {
        foreach (string item in Array)
        {
            List.Add(item);
        }
    }
}
