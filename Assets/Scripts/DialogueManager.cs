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
        talkData.Add(1, new string[] { "신사다. 안에는 동전이 쌓여 있다" });
        talkData.Add(2, new string[] { "허름한 신사다." });
        //story data
        talkData.Add(3, new string[] { "옛날 옛적에 한 마을에 사이 좋은 두 형제가 있었습니다." });
        talkData.Add(4, new string[] { "동생은 밭일과 낚시를 잘하여 매번 형을 위해 맛있는 밥상을 차렸고...", "형은 대장일과 싸움을 잘하여 나쁜 사람들로부터 동생을 지켜줬습니다. " });
        talkData.Add(5, new string[] { "둘은 어릴 적부터 부모 없이 자랐지만 대신 더할 나위 없는 형제 간의 우애를 가지고 있었습니다.", "그들의 고양이 친구 또한 소중한 연이 되어 마음의 공허를 채웠습니다. " });
        talkData.Add(6, new string[] { "어느 날 먼 이웃 나라에서 온 상인이 형이 만든 무기들을 보고 감탄하여 자신과 같이 장사를 하길 제안했습니다.", "제안을 수락만 한다면 부와 명예는 다 그의 것이 될 것이라고 속삭였습니다."});
        talkData.Add(7, new string[] { "하지만 형은 부와 명예에는 관심이 없다고 거절했습니다." ,"자신은 동생을 지키기 위해 쇠를 달구고 두들기지 돈에는 조금도 관심이 없다고 했습니다. ." });
        talkData.Add(8, new string[] { "그걸 들은 상인은 아쉬운 표정을 짓고 다시 갈 길을 가려고 했지만 순간 머릿속에 형을 설득할 기막힌 생각이 났습니다."});
        talkData.Add(9, new string[] { "부와 명예는 다름 아닌 동생을 지키기 위해 사용할 수 있다고 속삭였습니다." });
        talkData.Add(10, new string[] { "그러자 형은 망설이기 시작했습니다.","항상 자신을 위해 밭에서 힘들게 일하고 밥상을 차리는 동생에게 미안해졌습니다.","돈과 명예가 있으면 동생을 더 이상 힘들게 할 필요가 없을 것 같았습니다." });
        talkData.Add(11, new string[] { "동생 또한 고민하는 형을 보면서 미안함이 생겼습니다.", "약한 자신을 지키느라 언제나 의생하는 형의 앞날을 자가가 막는 게 아닐까 하고 슬퍼졌습니다." });
        talkData.Add(12, new string[] { "형은 고민을 해결하지 못한 채로 잠에 들었지만 동생은 그날 밤 마음을 굳게 먹었습니다.", "형에게 더 이상 자신을 위해 희생할 필요가 없다고 설득하기로 했습니다." });
        talkData.Add(13, new string[] { "동생은 다음날 아침 일찍 상인을 찾아갔습니다." ,"분명 마음은 먹었지만 형을 설득하기엔 자신은 너무나 약했습니다." }); 
        talkData.Add(14, new string[] { "상인은 동생의 예상대로 해결책을 제시해 주었습니다.","그의 가문 오래전부터 내려온 비약 제조법이 있는데 그 비약만 마시면 형보다 쌔질 수 있다고 했습니다."});
        talkData.Add(15, new string[] { "제조법을 건너 받은 즉시 동생은 재료를 모으기 시작했습니다." });
        talkData.Add(16, new string[] { "첫번째 재료는 노을이었습니다.","동생은 노을을 담을 방법을 몰랐으나 풍부한 상상력으로 노을을 오른쪽 눈 속에 담아 보관하기로 했습니다.","그건 마치 노을처럼 아름다웠습니다." });
        talkData.Add(17, new string[] { "두번째 재료는 태양이었습니다.", "동생은 저 거대한 태양을 이번에도 한쪽 눈을 바쳐 담아 보기로 했습니다.", "비록 더 이상 앞을 볼 수 없었지만 손 안에 느껴지는 따뜻함 덕분에 안심할 수 있었습니다." });
        talkData.Add(18, new string[] { "재료가 하나 더 남았지만 상인은 세 번째 재료는 굳이 찾으러 갈 필요가 없다고 했습니다.", "그건 이미 충분하다고 상인이 말했습니다." });
        talkData.Add(19, new string[] { "동생은 상인에게서 받은 약을 가지고 바로 형을 찾아 갔습니다." });
        talkData.Add(20, new string[] { "형은 수상한 약을 가지고 등장한 동생을 보고 깜짝 놀랐지만 그 무엇보다도 눈을 잃은 동생을 보고 가슴이 찢어졌습니다." });
        talkData.Add(21, new string[] { "동생은 자신이 이걸 마시면 더 이상 형의 걸림돌이 되지 않으리라 했지만 자상한 형은 동생에 대해서 그런 생각을 단 한 번도 해본 적이 없습니다.","오히려 자신을 위해 이렇게 되어버린 동생을 보고 더욱 큰 죄책감이 들기 시작했습니다." });
        talkData.Add(22, new string[] { "앞을 보지 못했지만 동생은 형의 기분이 나빠졌다는 걸 알 수 있었습니다.","이대로 가다가 형을 위한 계획이 물거품이 될 것을 짐작한 동생은 형에게 매달려 애걸복걸 애원했습니다." });
        talkData.Add(23, new string[] { "동생의 눈물을 본 형은 더 이상 동생 앞에 있을 자신이 없었습니다." });
        talkData.Add(24, new string[] { "형은 그 이후로 상인과 함께 마을 떠나 장사를 시작했습니다.","그 장사는 생각보다 더욱 잘 되어 부를 축척할 수 있었고 빠른 기간에 안에 동생이 있는 근처에 마을을 지을 수 있었습니다." });
        talkData.Add(25, new string[] { "간간히 들려오는 형에 대한 소문은 동생을 매우 기쁘고 자랑스럽게 했습니다.","비록 만나러 갈 수 없었지만 언젠가 형이 찾아올 날을 기대하며 매일 행복하게 지낼 수 있었습니다." });
        talkData.Add(26, new string[] { "그에 비해 형은 매일 무거워지는 죄책감 때문에 동생을 보고싶음에도 불구하고 슬픔을 억누르며 멀리서 지켜볼 뿐이었습니다." });
        talkData.Add(27, new string[] { "그런 하루하루가 반복되던 중 비가 올 것처럼 우울한 어느 날, 형이 동생에 대한 슬픈 소식을 전달받았습니다.","그가 마을 연못에 물을 퍼러 가는 도중 물에 빠졌다는 소식이었습니다." });
        talkData.Add(28, new string[] { "형은 하던 일을 멈추고 즉시 동생을 보러 달려갔습니다.","하지만 그를 반기는 건 의식 없는 동생의 차가운 몸 뿐이었습니다." });
        talkData.Add(29, new string[] { "알고 보니 동생은 마을 주민들로부터 외면을 받고 있었으며 자기가 붙여준 하인은 돈을 들고 도망친지 오래였습니다.","자신의 집에서도 움직이는 것조차 불편할 텐데 동생은 모든 것을 견뎌내려고 했습니다." });
        talkData.Add(30, new string[] { "형은 마을사람들을 탓할 수가 없었습니다.","동생에게 제일 필요했던 존재인 그는 그 누구보다 동생을 외면해왔으니 그들을 탓할 권리가 없었습니다." });
        talkData.Add(31, new string[] { "그때 고양이의 울음소리가 들렸습니다." , "까먹고 있었던 동생과 자신의 둘도 없는 친구였습니다." });       
        talkData.Add(32, new string[] { "고양이는 동생을 살리고 싶으면 자신을 따라오라고 말했습니다.","고양이가 말을 했지만 형에겐 말을 하는 고양이에게 놀랄 여유가 없었습니다.","한시라도 빨리 동생을 살려야 했습니다." });
        talkData.Add(33, new string[] { "고양이를 따라가니 형은 한 번도 본적 없는 신사에 도착했습니다." ,"주변에 높게 자란 잡초와 허름한 상태를 보니 사람의 손길은 받은 지 오랜 시간이 지난 것 같았습니다." });
        talkData.Add(34, new string[] { "그리고 고양이가 말했습니다.","동생을 살리려면 자신과 약속을 해야 한다고..." });

        //talk only data
        talkData.Add(100, new string[] { "여기는 시작의 섬이다." });
        talkData.Add(101, new string[] { "Z 키를 눌러 아이템을 주을 수 있는 것 같다.", "주은 아이템은 I 키로 확인할 수 있다." });
        talkData.Add(102, new string[] { "빠르게 달릴 수 있다는 건 꼭 좋은 것만은 아닌 것 같다." });
        talkData.Add(103, new string[] { "Space Bar로 공격을 할 수 있다.", "어떤 목적은 희생을 치뤄야만 달성할 수 있다." });
        talkData.Add(104, new string[] { "여기에도 신사가 있다." });

        talkData.Add(200, new string[] { "윗 마을에 오신걸 환영합니다!" });
        talkData.Add(201, new string[] { "동쪽에는 우리 마을의 명소인 연못이 있습니다.", "서쪽에는 우리의 식량을 책임 지는 밭이 있습니다.", "남쪽에는... (글씨가 지워져서 읽을 수가 없다.)" });
        talkData.Add(202, new string[] { "자주색으로 칠해진 지붕 아래에는 아무도 없는 모양이다." });
        talkData.Add(203, new string[] { "어딘가 슬퍼 보이는 연못이다." });
        talkData.Add(204, new string[] { "탁한 물로 가득찬 양동이다. (물은 어디서 퍼온 거지?)" });
        talkData.Add(205, new string[] { "뚜껑이 닫힌 양동이다. 흔들어보니 안에는 텅빈 것 같다." });
        talkData.Add(207, new string[] { "필요한 밀은 상자에서 가져가세요~ 다음 사람을 위해 씨앗 심는 일도 까먹지 마시고요!" });
        talkData.Add(208, new string[] { "왠지 왜로워 보이는 집이다." });
        talkData.Add(301, new string[] { "시간이 지날수록 기억이 점차 희미해져 간다." });
        talkData.Add(302, new string[] { "이상하다... 이 앞에도 마을이 있지만 두 마을간의 교류는 거의 없었던 것 같다." });
        talkData.Add(400, new string[] { "아랫 마을에 오신걸 환영합니다.", "신변 확인절차 때문에 줄이 길어질 수도 있으니 많은 양해 바랍니다." });
        talkData.Add(401, new string[] { "검문소 같은 분위기가 난다.", "종이에는 이름, 나이, 직업을 비롯한 기본 정보를 작성할 수 있는 모양이다." });
        talkData.Add(402, new string[] { "찰스대장간", "재료만 있으면 무엇이든지 만들어드립니다!" });
        talkData.Add(403, new string[] { "이웃들도 가족입니다!", "집이 필요하시면 저희 스테판 사무소에 연락 부탁드립니다." });
        talkData.Add(404, new string[] { "홉스농장", "여러분의 식탁은 저희가 책임져드립니다!" });
        talkData.Add(405, new string[] { "쇠를 달굴 때 쓰는 화로." });
        talkData.Add(406, new string[] { "여길 넘어가면 위험한 숲이 있으니 여러분의 안전을 보장할 수 없으니다.", "가능하면 돌아가시빌 바랍니다." });
        talkData.Add(408, new string[] { "구구구..." });
        talkData.Add(500, new string[] { "숲의 마물은 힘이 없고 약한 주민들에게 위험이었을 것이다." });
        talkData.Add(501, new string[] { "두 마을의 구조를 보면 마치 아랫 마을이 윗 마을을 마물로부터 지켜주는 것 같다." });
        talkData.Add(502, new string[] { "조사히보니 이 집에는 옛날에 마녀가 살았던 모양이다." });
        talkData.Add(503, new string[] { "진실의 동굴." });
        talkData.Add(600, new string[] { "형의 기억을 주웠다." });
        talkData.Add(601, new string[] { "동생의 기억을 주웠다." });

        talkData.Add(700, new string[] { "Special Thanks To...",
            "GoldMetal Studio Assets: Copyright c 2021 Goldmetal",
            "Sunnyside World by DanielDiggle",
            "Free Casual Game SFX Pack by Dusty Room",
            "Sunny Land by ansimus"
        });

        //npc talk data
        talkData.Add(1000, new string[] { "기억해 내 이것을 반복하는 이유를..." });
        talkData.Add(2000, new string[] { "처음 보는 얼굴이네.:2", "난 루나야!:1" });
        talkData.Add(3000, new string[] { "어떻게 이런 곳에 왔는지 모르겠어. 눈을 떠봤을 때 이미 끝없는 바다가 우릴 환영하고 있었지.:3", "하지만 우린 분명 이곳에서 나갈 수 있을 거야.:1" });
        talkData.Add(5000, new string[] { "미야오~" });

        //quest data
        talkData.Add(10 + 2000, new string[] { "안녕, 만나서 반가워! 난 루나야.:1","이곳에서 나가는 방법을 찾고 있다고?:0","루도는 표지판들을 따라가면 된다고 했어...:3"});
        talkData.Add(11 + 3000, new string[] { "안녕.:0","둘이라고 생각했는데 한명이 더 있었네.:1"});
        talkData.Add(20 + 3000, new string[] { "여기서 나가려면 일종의 예물을 바쳐야 하는 것 같아.:0", "신사에 가본적은 없지만 동전을 바치고 소원을 빌면 누군가 우리의 간절함을 들어주지 않을까? :0" });
        talkData.Add(21 + 4000, new string[] { "금으로 도금된 동전이다."});
        talkData.Add(30 + 3000, new string[] { "무사히 도착했구나.:1", ".....:2", "미안해. 루나가 아직 안와서 난 그 애가 올 때까지 좀 더 기다릴게. 나 대신 주변을 정찰해줄 수 있어?:0" });
        talkData.Add(31 + 1000, new string[] { "연못에서는 생선, 퍼오는 물로는 곡식을 재배... 연못이 곧 마을의 심장이다." });
        talkData.Add(32 + 1000, new string[] { "경계가 없는 구조... 작은 마을인 만큼 서로 의지하면서 가족처럼 지냈을 것 같다." });
        talkData.Add(33 + 5000, new string[] { "냐옹~", "(고양이의 울음소리가 힘이 없어 보인다.)" });
        talkData.Add(34 + 4000, new string[] { "케엑 켁! 껄룩~","고양이가 동전을 토해냈다." });
        talkData.Add(40 + 3000, new string[] { "...어. 너구나.:1","루나는 아마도 우리보다 앞서 간 것 같아.:0","먼저 왔다는 표시라도 남겼으면 좋았을텐데.:3" });
        talkData.Add(50 + 3000, new string[] { "이런 곳을 뭐라고 부를까?:2","이전에 지나간 마을이 시골 마을이라면 여긴 좀 더 도시 같아.:1","우선 바로 앞에 있는 대장간을 좀 둘러볼까?:0"});
        talkData.Add(51 + 2000, new string[] { "너 거기서 멀뚱멀뚱 뭐하고 있어?:2","손이 있으면 어디서 장작 10개 좀 구해봐!:3"});
        talkData.Add(52 + 3000, new string[] { "루...루나? 여기서 뭐하고 있어?!:3", "(루나는 아무런 반응이 없다):3", "어째서...:0" });
        talkData.Add(53 + 4000, new string[] { "화로에서 떨어져나온 파편이다." });
        talkData.Add(54 + 2000, new string[] { "도아줘서 정말 고마워! 이건 내가 약속했던 보상이야.:1", "아 맞다! 루도라는 사람이 찾고 있어.:1" });
        talkData.Add(60 + 3000, new string[] { "미안 여기까지 오게 해서 .... 루나는 기억을 잊어버린 것 같아.:2", "난 이제 어쩌면 좋지?:0", "아 맞다. 고양이가 여기까지 우릴 따라온 것 같아.:0", "한번 가보는거 어때? 난 이곳에 루나랑 남아 있을게...:2" });


        //add portrait to array
        //루나
        portraitData.Add(2000 + 0, portraitArray[0]);//talk
        portraitData.Add(2000 + 1, portraitArray[1]);//smile
        portraitData.Add(2000 + 2, portraitArray[2]);//idle
        portraitData.Add(2000 + 3, portraitArray[3]);//angry
        //루도
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
