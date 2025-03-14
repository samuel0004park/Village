using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    static public DialogueManager Instance;

    //matches key to specific data 
    private Dictionary<int, string[]> talkData;
    private Dictionary<int, Sprite> portraitData;

    //Array to store all portraits
    [SerializeField] private Sprite[] portraitArray;

    // Lists for Live time Dialgue
    private List<string> listSentences;
    private List<Sprite> listSprites;
    private List<Sprite> listDialogueWindows;

    public bool talking { get; private set; }
    private int page; 
    private bool keyActivated;


    [SerializeField] private Animator animSprite;
    [SerializeField] private Animator animDialogueWindow;
    [SerializeField] private Image image;
    [SerializeField] private Image dialogueWindow;
    [SerializeField] private TypeEffect theType;


    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        SetUp();

        GenerateData();
    }

    private void SetUp() {
        //initialize list and dictionary
        page = 0;
        listSentences = new List<string>();
        listSprites = new List<Sprite>();
        listDialogueWindows = new List<Sprite>();
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
    }

    public void GenerateData()
    {
        //RULES :
        //1. Only talk text must be multiple of 100, 
        //2. Same quest must have same beginning 10 -> 11 -> 12....
        //3. If talking to item is involved in quest, skip a count 10 -> 12 -> 13
        //4. New quest increments count by 10  ... 12 -> 20 -> 21

        //shrine data
        talkData.Add(1, new string[] { "�Ż��. �ȿ��� ������ �׿� �ִ�" });
        talkData.Add(2, new string[] { "�㸧�� �Ż��." });
        //story data
        talkData.Add(3, new string[] { "���� ������ �� ������ ���� ���� �� ������ �־����ϴ�." });
        talkData.Add(4, new string[] { "������ ���ϰ� ���ø� ���Ͽ� �Ź� ���� ���� ���ִ� ����� ���Ȱ�...", "���� �����ϰ� �ο��� ���Ͽ� ���� �����κ��� ������ ��������ϴ�. " });
        talkData.Add(5, new string[] { "���� � ������ �θ� ���� �ڶ����� ��� ���� ���� ���� ���� ���� ��ָ� ������ �־����ϴ�.", "�׵��� ����� ģ�� ���� ������ ���� �Ǿ� ������ ���㸦 ä�����ϴ�. " });
        talkData.Add(6, new string[] { "��� �� �� �̿� ���󿡼� �� ������ ���� ���� ������� ���� ��ź�Ͽ� �ڽŰ� ���� ��縦 �ϱ� �����߽��ϴ�.", "������ ������ �Ѵٸ� �ο� ���� �� ���� ���� �� ���̶�� �ӻ迴���ϴ�."});
        talkData.Add(7, new string[] { "������ ���� �ο� ������ ������ ���ٰ� �����߽��ϴ�." ,"�ڽ��� ������ ��Ű�� ���� �踦 �ޱ��� �ε���� ������ ���ݵ� ������ ���ٰ� �߽��ϴ�. ." });
        talkData.Add(8, new string[] { "�װ� ���� ������ �ƽ��� ǥ���� ���� �ٽ� �� ���� ������ ������ ���� �Ӹ��ӿ� ���� ������ �⸷�� ������ �����ϴ�."});
        talkData.Add(9, new string[] { "�ο� ���� �ٸ� �ƴ� ������ ��Ű�� ���� ����� �� �ִٰ� �ӻ迴���ϴ�." });
        talkData.Add(10, new string[] { "�׷��� ���� �����̱� �����߽��ϴ�.","�׻� �ڽ��� ���� �翡�� ����� ���ϰ� ����� ������ �������� �̾��������ϴ�.","���� ���� ������ ������ �� �̻� ����� �� �ʿ䰡 ���� �� ���ҽ��ϴ�." });
        talkData.Add(11, new string[] { "���� ���� ����ϴ� ���� ���鼭 �̾����� ������ϴ�.", "���� �ڽ��� ��Ű���� ������ �ǻ��ϴ� ���� �ճ��� �ڰ��� ���� �� �ƴұ� �ϰ� ���������ϴ�." });
        talkData.Add(12, new string[] { "���� ����� �ذ����� ���� ä�� �ῡ ������� ������ �׳� �� ������ ���� �Ծ����ϴ�.", "������ �� �̻� �ڽ��� ���� ����� �ʿ䰡 ���ٰ� �����ϱ�� �߽��ϴ�." });
        talkData.Add(13, new string[] { "������ ������ ��ħ ���� ������ ã�ư����ϴ�." ,"�и� ������ �Ծ����� ���� �����ϱ⿣ �ڽ��� �ʹ��� ���߽��ϴ�." }); 
        talkData.Add(14, new string[] { "������ ������ ������ �ذ�å�� ������ �־����ϴ�.","���� ���� ���������� ������ ��� �������� �ִµ� �� ��ุ ���ø� ������ ���� �� �ִٰ� �߽��ϴ�."});
        talkData.Add(15, new string[] { "�������� �ǳ� ���� ��� ������ ��Ḧ ������ �����߽��ϴ�." });
        talkData.Add(16, new string[] { "ù��° ���� �����̾����ϴ�.","������ ������ ���� ����� �������� ǳ���� �������� ������ ������ �� �ӿ� ��� �����ϱ�� �߽��ϴ�.","�װ� ��ġ ����ó�� �Ƹ��ٿ����ϴ�." });
        talkData.Add(17, new string[] { "�ι�° ���� �¾��̾����ϴ�.", "������ �� �Ŵ��� �¾��� �̹����� ���� ���� ���� ��� ����� �߽��ϴ�.", "��� �� �̻� ���� �� �� �������� �� �ȿ� �������� ������ ���п� �Ƚ��� �� �־����ϴ�." });
        talkData.Add(18, new string[] { "��ᰡ �ϳ� �� �������� ������ �� ��° ���� ���� ã���� �� �ʿ䰡 ���ٰ� �߽��ϴ�.", "�װ� �̹� ����ϴٰ� ������ ���߽��ϴ�." });
        talkData.Add(19, new string[] { "������ ���ο��Լ� ���� ���� ������ �ٷ� ���� ã�� �����ϴ�." });
        talkData.Add(20, new string[] { "���� ������ ���� ������ ������ ������ ���� ��¦ ������� �� �������ٵ� ���� ���� ������ ���� ������ ���������ϴ�." });
        talkData.Add(21, new string[] { "������ �ڽ��� �̰� ���ø� �� �̻� ���� �ɸ����� ���� �������� ������ �ڻ��� ���� ������ ���ؼ� �׷� ������ �� �� ���� �غ� ���� �����ϴ�.","������ �ڽ��� ���� �̷��� �Ǿ���� ������ ���� ���� ū ��å���� ��� �����߽��ϴ�." });
        talkData.Add(22, new string[] { "���� ���� �������� ������ ���� ����� �������ٴ� �� �� �� �־����ϴ�.","�̴�� ���ٰ� ���� ���� ��ȹ�� ����ǰ�� �� ���� ������ ������ ������ �Ŵ޷� �ְɺ��� �ֿ��߽��ϴ�." });
        talkData.Add(23, new string[] { "������ ������ �� ���� �� �̻� ���� �տ� ���� �ڽ��� �������ϴ�." });
        talkData.Add(24, new string[] { "���� �� ���ķ� ���ΰ� �Բ� ���� ���� ��縦 �����߽��ϴ�.","�� ���� �������� ���� �� �Ǿ� �θ� ��ô�� �� �־��� ���� �Ⱓ�� �ȿ� ������ �ִ� ��ó�� ������ ���� �� �־����ϴ�." });
        talkData.Add(25, new string[] { "������ ������� ���� ���� �ҹ��� ������ �ſ� ��ڰ� �ڶ������� �߽��ϴ�.","��� ������ �� �� �������� ������ ���� ã�ƿ� ���� ����ϸ� ���� �ູ�ϰ� ���� �� �־����ϴ�." });
        talkData.Add(26, new string[] { "�׿� ���� ���� ���� ���ſ����� ��å�� ������ ������ ����������� �ұ��ϰ� ������ �ﴩ���� �ָ��� ���Ѻ� ���̾����ϴ�." });
        talkData.Add(27, new string[] { "�׷� �Ϸ��Ϸ簡 �ݺ��Ǵ� �� �� �� ��ó�� ����� ��� ��, ���� ������ ���� ���� �ҽ��� ���޹޾ҽ��ϴ�.","�װ� ���� ������ ���� �۷� ���� ���� ���� �����ٴ� �ҽ��̾����ϴ�." });
        talkData.Add(28, new string[] { "���� �ϴ� ���� ���߰� ��� ������ ���� �޷������ϴ�.","������ �׸� �ݱ�� �� �ǽ� ���� ������ ������ �� ���̾����ϴ�." });
        talkData.Add(29, new string[] { "�˰� ���� ������ ���� �ֹε�κ��� �ܸ��� �ް� �־����� �ڱⰡ �ٿ��� ������ ���� ��� ����ģ�� ���������ϴ�.","�ڽ��� �������� �����̴� ������ ������ �ٵ� ������ ��� ���� �ߵ������� �߽��ϴ�." });
        talkData.Add(30, new string[] { "���� ����������� ſ�� ���� �������ϴ�.","�������� ���� �ʿ��ߴ� ������ �״� �� �������� ������ �ܸ��ؿ����� �׵��� ſ�� �Ǹ��� �������ϴ�." });
        talkData.Add(31, new string[] { "�׶� ������� �����Ҹ��� ��Ƚ��ϴ�." , "��԰� �־��� ������ �ڽ��� �ѵ� ���� ģ�������ϴ�." });       
        talkData.Add(32, new string[] { "����̴� ������ �츮�� ������ �ڽ��� �������� ���߽��ϴ�.","����̰� ���� ������ ������ ���� �ϴ� ����̿��� ��� ������ �������ϴ�.","�ѽö� ���� ������ ����� �߽��ϴ�." });
        talkData.Add(33, new string[] { "����̸� ���󰡴� ���� �� ���� ���� ���� �Ż翡 �����߽��ϴ�." ,"�ֺ��� ���� �ڶ� ���ʿ� �㸧�� ���¸� ���� ����� �ձ��� ���� �� ���� �ð��� ���� �� ���ҽ��ϴ�." });
        talkData.Add(34, new string[] { "�׸��� ����̰� ���߽��ϴ�.","������ �츮���� �ڽŰ� ����� �ؾ� �Ѵٰ�..." });

        //talk only data
        talkData.Add(100, new string[] { "����� ������ ���̴�." });
        talkData.Add(101, new string[] { "Z Ű�� ���� �������� ���� �� �ִ� �� ����.", "���� �������� I Ű�� Ȯ���� �� �ִ�." });
        talkData.Add(102, new string[] { "������ �޸� �� �ִٴ� �� �� ���� �͸��� �ƴ� �� ����." });
        talkData.Add(103, new string[] { "Space Bar�� ������ �� �� �ִ�.", "� ������ ����� ġ��߸� �޼��� �� �ִ�." });
        talkData.Add(104, new string[] { "���⿡�� �Ż簡 �ִ�." });

        talkData.Add(200, new string[] { "�� ������ ���Ű� ȯ���մϴ�!" });
        talkData.Add(201, new string[] { "���ʿ��� �츮 ������ ����� ������ �ֽ��ϴ�.", "���ʿ��� �츮�� �ķ��� å�� ���� ���� �ֽ��ϴ�.", "���ʿ���... (�۾��� �������� ���� ���� ����.)" });
        talkData.Add(202, new string[] { "���ֻ����� ĥ���� ���� �Ʒ����� �ƹ��� ���� ����̴�." });
        talkData.Add(203, new string[] { "��� ���� ���̴� �����̴�." });
        talkData.Add(204, new string[] { "Ź�� ���� ������ �絿�̴�. (���� ��� �ۿ� ����?)" });
        talkData.Add(205, new string[] { "�Ѳ��� ���� �絿�̴�. ����� �ȿ��� �ֺ� �� ����." });
        talkData.Add(207, new string[] { "�ʿ��� ���� ���ڿ��� ����������~ ���� ����� ���� ���� �ɴ� �ϵ� ����� ���ð��!" });
        talkData.Add(208, new string[] { "���� �ַο� ���̴� ���̴�." });
        talkData.Add(301, new string[] { "�ð��� �������� ����� ���� ������� ����." });
        talkData.Add(302, new string[] { "�̻��ϴ�... �� �տ��� ������ ������ �� �������� ������ ���� ������ �� ����." });
        talkData.Add(400, new string[] { "�Ʒ� ������ ���Ű� ȯ���մϴ�.", "�ź� Ȯ������ ������ ���� ����� ���� ������ ���� ���� �ٶ��ϴ�." });
        talkData.Add(401, new string[] { "�˹��� ���� �����Ⱑ ����.", "���̿��� �̸�, ����, ������ ����� �⺻ ������ �ۼ��� �� �ִ� ����̴�." });
        talkData.Add(402, new string[] { "�������尣", "��Ḹ ������ �����̵��� �����帳�ϴ�!" });
        talkData.Add(403, new string[] { "�̿��鵵 �����Դϴ�!", "���� �ʿ��Ͻø� ���� ������ �繫�ҿ� ���� ��Ź�帳�ϴ�." });
        talkData.Add(404, new string[] { "ȩ������", "�������� ��Ź�� ���� å�����帳�ϴ�!" });
        talkData.Add(405, new string[] { "�踦 �ޱ� �� ���� ȭ��." });
        talkData.Add(406, new string[] { "���� �Ѿ�� ������ ���� ������ �������� ������ ������ �� �����ϴ�.", "�����ϸ� ���ư��ú� �ٶ��ϴ�." });
        talkData.Add(408, new string[] { "������..." });
        talkData.Add(500, new string[] { "���� ������ ���� ���� ���� �ֹε鿡�� �����̾��� ���̴�." });
        talkData.Add(501, new string[] { "�� ������ ������ ���� ��ġ �Ʒ� ������ �� ������ �����κ��� �����ִ� �� ����." });
        talkData.Add(502, new string[] { "���������� �� ������ ������ ���డ ��Ҵ� ����̴�." });
        talkData.Add(503, new string[] { "������ ����." });
        talkData.Add(600, new string[] { "���� ����� �ֿ���." });
        talkData.Add(601, new string[] { "������ ����� �ֿ���." });

        talkData.Add(700, new string[] { "Special Thanks To...",
            "GoldMetal Studio Assets: Copyright c 2021 Goldmetal",
            "Sunnyside World by DanielDiggle",
            "Free Casual Game SFX Pack by Dusty Room",
            "Sunny Land by ansimus"
        });

        //npc talk data
        talkData.Add(1000, new string[] { "����� �� �̰��� �ݺ��ϴ� ������..." });
        talkData.Add(2000, new string[] { "ó�� ���� ���̳�.:2", "�� �糪��!:1" });
        talkData.Add(3000, new string[] { "��� �̷� ���� �Դ��� �𸣰ھ�. ���� ������ �� �̹� ������ �ٴٰ� �츱 ȯ���ϰ� �־���.:3", "������ �츰 �и� �̰����� ���� �� ���� �ž�.:1" });
        talkData.Add(5000, new string[] { "�̾߿�~" });

        //quest data
        talkData.Add(10 + 2000, new string[] { "�ȳ�, ������ �ݰ���! �� �糪��.:1","�̰����� ������ ����� ã�� �ִٰ�?:0","�絵�� ǥ���ǵ��� ���󰡸� �ȴٰ� �߾�...:3"});
        talkData.Add(11 + 3000, new string[] { "�ȳ�.:0","���̶�� �����ߴµ� �Ѹ��� �� �־���.:1"});
        talkData.Add(20 + 3000, new string[] { "���⼭ �������� ������ ������ ���ľ� �ϴ� �� ����.:0", "�Ż翡 �������� ������ ������ ��ġ�� �ҿ��� ���� ������ �츮�� �������� ������� ������? :0" });
        talkData.Add(21 + 4000, new string[] { "������ ���ݵ� �����̴�."});
        talkData.Add(30 + 3000, new string[] { "������ �����߱���.:1", ".....:2", "�̾���. �糪�� ���� �ȿͼ� �� �� �ְ� �� ������ �� �� ��ٸ���. �� ��� �ֺ��� �������� �� �־�?:0" });
        talkData.Add(31 + 1000, new string[] { "���������� ����, �ۿ��� ���δ� ����� ���... ������ �� ������ �����̴�." });
        talkData.Add(32 + 1000, new string[] { "��谡 ���� ����... ���� ������ ��ŭ ���� �����ϸ鼭 ����ó�� ������ �� ����." });
        talkData.Add(33 + 5000, new string[] { "�Ŀ�~", "(������� �����Ҹ��� ���� ���� ���δ�.)" });
        talkData.Add(34 + 4000, new string[] { "�ɿ� ��! ����~","����̰� ������ ���س´�." });
        talkData.Add(40 + 3000, new string[] { "...��. �ʱ���.:1","�糪�� �Ƹ��� �츮���� �ռ� �� �� ����.:0","���� �Դٴ� ǥ�ö� �������� �������ٵ�.:3" });
        talkData.Add(50 + 3000, new string[] { "�̷� ���� ����� �θ���?:2","������ ������ ������ �ð� �����̶�� ���� �� �� ���� ����.:1","�켱 �ٷ� �տ� �ִ� ���尣�� �� �ѷ�����?:0"});
        talkData.Add(51 + 2000, new string[] { "�� �ű⼭ �ֶ׸ֶ� ���ϰ� �־�?:2","���� ������ ��� ���� 10�� �� ���غ�!:3"});
        talkData.Add(52 + 3000, new string[] { "��...�糪? ���⼭ ���ϰ� �־�?!:3", "(�糪�� �ƹ��� ������ ����):3", "��°��...:0" });
        talkData.Add(53 + 4000, new string[] { "ȭ�ο��� ���������� �����̴�." });
        talkData.Add(54 + 2000, new string[] { "�����༭ ���� ����! �̰� ���� ����ߴ� �����̾�.:1", "�� �´�! �絵��� ����� ã�� �־�.:1" });
        talkData.Add(60 + 3000, new string[] { "�̾� ������� ���� �ؼ� .... �糪�� ����� �ؾ���� �� ����.:2", "�� ���� ��¼�� ����?:0", "�� �´�. ����̰� ������� �츱 ����� �� ����.:0", "�ѹ� �����°� �? �� �̰��� �糪�� ���� ������...:2" });


        //add portrait to array
        //�糪
        portraitData.Add(2000 + 0, portraitArray[0]);//talk
        portraitData.Add(2000 + 1, portraitArray[1]);//smile
        portraitData.Add(2000 + 2, portraitArray[2]);//idle
        portraitData.Add(2000 + 3, portraitArray[3]);//angry
        //�絵
        portraitData.Add(3000 + 0, portraitArray[4]);//talk
        portraitData.Add(3000 + 1, portraitArray[5]);//smile
        portraitData.Add(3000 + 2, portraitArray[6]);//idle
        portraitData.Add(3000 + 3, portraitArray[7]);//angry
    }

    private Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex]; 
    }

    private bool CheckDialogue(int id)
    {
        string temp = (talkData[id][0].Split(':')[0]);
        if (temp.Equals(talkData[id][0]))
            return true;
        else
            return false;
    }

    private void LoadText(int id)
    {
        //get text only data
        if (CheckDialogue(id)) AddSentence(id);
        //get dialogue data 
        else AddDialogue(id);
    }

    private void AddSentence(int index)
    {
        for (int i = 0; i < talkData[index].Length; i++)
            listSentences.Add(talkData[index][i]);
    }

    private void AddDialogue(int index)
    {
        animSprite.SetBool("Appear", true);
        for (int i = 0; i < talkData[index].Length; i++)
        {
            string temp = talkData[index][i];
            listSentences.Add(temp.Split(':')[0]);
            listSprites.Add(GetPortrait(index - index % 100, int.Parse(temp.Split(':')[1])));
        }
    }
    public void ShowDialogue(int _id)
    {
        //turn on values
        talking = true;

        //load the text from talkdata to list
        LoadText(_id);
        
        //Start coroutine to show text on screen
        StartCoroutine(StartDialogueCoroutine());
    }

    IEnumerator StartDialogueCoroutine()
    {
        //show count-th text 

        //show default dialogue panel
        animDialogueWindow.SetBool("Appear", false);
        animDialogueWindow.SetBool("Appear", true);

        /* for dialgue window change
         if (listDialogueWindows[count] != listDialogueWindows[count - 1])
            {
                animSprite.SetBool("Change", true);
                animDialogueWindow.SetBool("Appear", false);
                yield return new WaitForSeconds(0.2f);
                dialogueWindow.sprite = listDialogueWindows[count];
                image.sprite = listSprites[count];
                animDialogueWindow.SetBool("Appear", true);
                animSprite.SetBool("Change", false);
            }
         */
        //if npctalk, change portrait if different 
        if (listSprites.Count!=0)
        {
            if (page > 0)
            {
                if (listSprites[page] != listSprites[page - 1])
                {
                    animSprite.SetBool("Change", true);
                    yield return new WaitForSeconds(0.1f);
                    image.sprite = listSprites[page];
                    animSprite.SetBool("Change", false);
                }
                //when no change
                else
                    yield return new WaitForSeconds(0.05f);
            }
            else
            {
                image.sprite = listSprites[page];
            }
        }

        theType.SetMsg(listSentences[page]);
        yield return new WaitForSeconds(0.01f);

        keyActivated = true;
    }

    private void ExitDialogue()
    {
        //clear all used variables and lists and mend all ends
        page = 0;
        //clear list
        listSentences.Clear();
        listSprites.Clear();
        listDialogueWindows.Clear();
        //make disappear sprite and window
        animSprite.SetBool("Appear", false);
        animDialogueWindow.SetBool("Appear", false);
        talking = false;
    }

    private void Update()
    {
        //show next sentence if z is pressed by incrementing count and reset current dialogue
        if (Input.GetKeyDown(KeyCode.Z) && talking && keyActivated)
        {
            if (theType.isAnim)
            {
                theType.SetMsg("");
                return;
            }
            keyActivated = false;
            page++;


            //if end of talk, then stop all coroutine and exit dialogue
            if (page == listSentences.Count)
            {
                StopAllCoroutines();
                ExitDialogue();
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(StartDialogueCoroutine());  
            }
        }
    }
}
