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
                case SentenceType.SubmergedPillar:
                    AddArrayToList(ListComposedWithIds, DataManager.SubmergedPillarSentences);
                    break;
                case SentenceType.SciencePillar:
                    AddArrayToList(ListComposedWithIds, DataManager.SciencePillarSentences);
                    break;
                case SentenceType.Plank:
                    AddArrayToList(ListComposedWithIds, DataManager.PlankSentences);
                    break;
                case SentenceType.Ruin:
                    AddArrayToList(ListComposedWithIds, DataManager.RuinSentences);
                    break;
                case SentenceType.Wall:
                    AddArrayToList(ListComposedWithIds, DataManager.WallSentences);
                    break;
                case SentenceType.Slab:
                    AddArrayToList(ListComposedWithIds, DataManager.SlabSentences);
                    break;
                case SentenceType.Rock:
                    AddArrayToList(ListComposedWithIds, DataManager.RockSentences);
                    break;
                case SentenceType.Stone:
                    AddArrayToList(ListComposedWithIds, DataManager.StoneSentences);
                    break;
                case SentenceType.Wonder:
                    AddArrayToList(ListComposedWithIds, DataManager.WonderSentences);
                    break;
                case SentenceType.Snake:
                    AddArrayToList(ListComposedWithIds, DataManager.SnakeSentences);
                    break;
                case SentenceType.Ring:
                    AddArrayToList(ListComposedWithIds, DataManager.RingSentences);
                    break;
                case SentenceType.Orca:
                    AddArrayToList(ListComposedWithIds, DataManager.OrcaSentences);
                    break;
                case SentenceType.WhiteBuilding:
                    AddArrayToList(ListComposedWithIds, DataManager.WhiteBuildingSentences);
                    break;
                case SentenceType.MusicHole:
                    AddArrayToList(ListComposedWithIds, DataManager.MusicHoleSentences);
                    break;
                case SentenceType.windmill:
                    AddArrayToList(ListComposedWithIds, DataManager.WindmillSentences);
                    break;
                case SentenceType.Rune:
                    AddArrayToList(ListComposedWithIds, DataManager.RuneSentences);
                    break;
                case SentenceType.Joy:
                    AddArrayToList(ListComposedWithIds, DataManager.JoySentences);
                    break;
                case SentenceType.Knowledge:
                    AddArrayToList(ListComposedWithIds, DataManager.KnowledgeSentences);
                    break;
                case SentenceType.Sun:
                    AddArrayToList(ListComposedWithIds, DataManager.SunSentences);
                    break;
                case SentenceType.Day:
                    AddArrayToList(ListComposedWithIds, DataManager.DaySentences);
                    break;
                case SentenceType.Beginning:
                    AddArrayToList(ListComposedWithIds, DataManager.BeginningSentences);
                    break;
                case SentenceType.Destiny:
                    AddArrayToList(ListComposedWithIds, DataManager.DestinySentences);
                    break;
                case SentenceType.Wealth:
                    AddArrayToList(ListComposedWithIds, DataManager.WealthSentences);
                    break;
                case SentenceType.Eye:
                    AddArrayToList(ListComposedWithIds, DataManager.EyeSentences);
                    break;
                case SentenceType.Journey:
                    AddArrayToList(ListComposedWithIds, DataManager.JourneySentences);
                    break;
                case SentenceType.Bird:
                    AddArrayToList(ListComposedWithIds, DataManager.BirdSentences);
                    break;
                case SentenceType.Fire:
                    AddArrayToList(ListComposedWithIds, DataManager.FireSentences);
                    break;
                case SentenceType.Ash:
                    AddArrayToList(ListComposedWithIds, DataManager.AshSentences);
                    break;
                case SentenceType.Ice:
                    AddArrayToList(ListComposedWithIds, DataManager.IceSentences);
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
