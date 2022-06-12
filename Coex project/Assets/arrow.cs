using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchToolkit;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class arrow : MonoBehaviour
{
    Transform Arrow;//화살표 객체
    public Transform user;

    public GameObject Marker; //미니맵 사용자 위치 마커
    public GameObject Markerpoint; //미니맵에서 중간지점 이름
    public GameObject RawImage;

    string midP;//중간지점 이름

    int next;//현재 할당된 중간지점
    int n;//경로 순서

    int conn;//지금 도착한 지점
    int fin;//목적지

    int[] array = new int[] { };
    int[] arrayOne = new int[] { 9, 13, 14, 20, 21, 41 };
    int[] arrayTwo = new int[] { 9, 10, 22, 23, 25, 30, 42 };
    nevigation nv = new nevigation();//네비게이션 가져옴
    bool isStartNv = false;

    GameObject Parent;
    GameObject Child;
    public GameObject nextObj; //다음 포인트 
    private float Dist; //다음지점까지 거리
    private int distance; //거리를 정수형으로 나타냄
    float timer;
    public Text ScriptTxt; //오른쪽 하단에 나오는 text

    // Start is called before the first frame update
    void Start()
    {
        RawImage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartNv)
        {
            Action();

        }


        transform.LookAt(GameObject.Find(midP).transform);

        Dist = Vector3.Distance(Parent.transform.position, nextObj.transform.position);  //다음지점까지 거리 계산

    }

    void Action() // arrow를 사용자에게 종속시킴
    {
        ScriptTxt.text = "";
        Parent = ArchToolkitManager.Instance.visitor.Head;
        Child = this.gameObject;

        Child.transform.parent = Parent.transform;

        this.transform.localPosition = new Vector3(0.0f, -1.0f, 2.00f);

        // 마커 위치 갱신
        Marker.transform.position = new Vector3(Parent.transform.position.x, Marker.transform.position.y, Parent.transform.position.z);

        Markerpoint.transform.position = new Vector3(nextObj.transform.position.x, Markerpoint.transform.position.y, nextObj.transform.position.z);

        Dist = Vector3.Distance(Parent.transform.position, nextObj.transform.position);  //다음지점까지 거리 계산

    }

    private void LateUpdate()
    {
        distance = (int)Dist;
        ScriptTxt.text = "다음 지점까지" + distance + "m"; //text에 다음 지점까지 몇 m 남았습니다 표시
        Debug.Log("Dist: " + Dist);
    }

    private void Reset()//모든 변수를 초기화함
    {

    }
    void findConn(string a) // 도착한 중간지점을 확인 
    {
        string b;
        b = a.Replace("point-", "");

        conn = int.Parse(b);
        Debug.Log("conn is " + conn);

    }

    private void OnTriggerEnter(Collider other)//중간지점이랑 화살표가 접촉함
    {
        if (isStartNv)
        {
            string a = other.name;

            findConn(a);

            int now = next;
            fin = array[array.Length - 1];

            //if (conn == next)//화살표가 이번 중간지점에 도착했는지 확인
            if (conn == now)
            {
                Debug.Log("중간지점 도달");
                n++;
                next = array[n];
                //next = nv.show(n);
                midP = "point-" + array[n].ToString();
                //midP = "point-" + next.ToString();
                nextObj = GameObject.Find(midP); //다음지점을 nextObj 게임 오브젝트에 할당
                Debug.Log("다음 지점 할당");
            }
            /*
            if (conn != next)//화살표가 중간지점이 아닌 다른곳에 도착했을때 다시 리라우팅
            {
                Debug.Log("경로 이탈");
                nv.rec(conn);//현제 지점을 시작점으로 경로 재설정
                n = 0;      //재설정된 경로로 다시 시작
                Debug.Log("경로 재정의");
            }*/

            if (conn == fin)//화살표가 이번 중간지점에 도착했는지 확인
            {
                Debug.Log("목적지 도달");
            }
        }
    }
    public void startNavigation(string id)
    {

        isStartNv = true;
        RawImage.SetActive(true);
        Arrow = GetComponent<Transform>();
        Debug.Log("navSet Start");
        nv.navSet(9, int.Parse(id));//네비게이션 시작

        n = 0;
        Debug.Log("nvShow Start");
        next = nv.show(n);

        if(int.Parse(id) == 41)
        {
            array = arrayOne;
            Debug.Log("1할당 : " + array.Length);
        }
        /*
        else
        {
            array = arrayTwo;
            Debug.Log("2할당 : " + array.Length);
        }*/
        midP = "point-" + array[n].ToString();
        next = array[n];
        Debug.Log("next point is " + next);
        //midP = "point-" + next.ToString();
        nextObj = GameObject.Find(midP); //다음지점을 nextObj 게임 오브젝트에 할당
        Debug.Log(midP);
    }

    public void OnClickCategory()
    {
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;
        string id = clickObj.ToString();
        int len = id.Length;
        len = len - 25;
        id = id.Substring(0, len);
        Debug.Log(id);
        startNavigation(id);
        fin = int.Parse(id);
    }
}