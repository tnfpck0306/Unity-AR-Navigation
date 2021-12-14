using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchToolkit;
using UnityEngine.EventSystems;
public class arrow : MonoBehaviour
{
    Transform Arrow;//화살표 객체
    public Transform user;

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
    float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isStartNv)
        {
            Action();
        }

        transform.LookAt(GameObject.Find(midP).transform);
    }

    void Action() // arrow를 사용자에게 종속시킴
    {
        Parent = ArchToolkitManager.Instance.visitor.Head;
        Child = this.gameObject;

        Child.transform.parent = Parent.transform;

        this.transform.localPosition = new Vector3(0.0f, -1.0f, 2.00f);
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
            //fin = array[array.Length - 1];

            //if (conn == next)//화살표가 이번 중간지점에 도착했는지 확인
            if (conn == now)
            {
                Debug.Log("중간지점 도달");
                n++;
                //next = array[n];
                next = nv.show(n);
                //midP = "point-" + array[n].ToString();
                midP = "point-" + next.ToString();
                Debug.Log("다음 지점 할당");
            }

            if (conn != next)//화살표가 중간지점이 아닌 다른곳에 도착했을때 다시 리라우팅
            {
                Debug.Log("경로 이탈");
                rerouting();
                Debug.Log("경로 재정의");
            }

            if (conn == fin)//화살표가 이번 중간지점에 도착했는지 확인
            {
                Debug.Log(conn);
                Debug.Log(next);
                Debug.Log("목적지 도달");
            }
        }
    }
    public void startNavigation(string id)
    {
        
        isStartNv = true;
        Arrow = GetComponent<Transform>();
        Debug.Log("navSet Start");
        nv.navSet(4, int.Parse(id));//네비게이션 시작
        n = 0;
        Debug.Log("nvShow Start");
        next = nv.show(n);
        /*if(int.Parse(id) == 41)
        {
            array = arrayOne;
            Debug.Log("1할당 : " + array.Length);
        }
        else
        {
            array = arrayTwo;
            Debug.Log("2할당 : " + array.Length);
        }*/
        //midP = "point-" + array[n].ToString();
        Debug.Log("next point is" + next);
        midP = "point-" + next.ToString();
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
    }

    public void rerouting()
    {
        nv.navReset(conn);//현재 지점을 시작지점으로 다시 경로를 계산함
        n = 0;
        next = nv.show(n);
        Debug.Log("next point is" + next);
        midP = "point-" + next.ToString();
        Debug.Log(midP);
        //새로운 경로로 다시 시작함
    }
}