using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchToolkit;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class arrow : MonoBehaviour
{
    Transform Arrow;//ȭ��ǥ ��ü
    public Transform user;

    public GameObject Marker; //�̴ϸ� ����� ��ġ ��Ŀ
    public GameObject Markerpoint; //�̴ϸʿ��� �߰����� �̸�
    public GameObject RawImage;

    string midP;//�߰����� �̸�

    int next;//���� �Ҵ�� �߰�����
    int n;//��� ����

    int conn;//���� ������ ����
    int fin;//������

    int[] array = new int[] { };
    int[] arrayOne = new int[] { 9, 13, 14, 20, 21, 41 };
    int[] arrayTwo = new int[] { 9, 10, 22, 23, 25, 30, 42 };
    nevigation nv = new nevigation();//�׺���̼� ������
    bool isStartNv = false;

    GameObject Parent;
    GameObject Child;
    public GameObject nextObj; //���� ����Ʈ 
    private float Dist; //������������ �Ÿ�
    private int distance; //�Ÿ��� ���������� ��Ÿ��
    float timer;
    public Text ScriptTxt; //������ �ϴܿ� ������ text

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

        Dist = Vector3.Distance(Parent.transform.position, nextObj.transform.position);  //������������ �Ÿ� ���

    }

    void Action() // arrow�� ����ڿ��� ���ӽ�Ŵ
    {
        ScriptTxt.text = "";
        Parent = ArchToolkitManager.Instance.visitor.Head;
        Child = this.gameObject;

        Child.transform.parent = Parent.transform;

        this.transform.localPosition = new Vector3(0.0f, -1.0f, 2.00f);

        // ��Ŀ ��ġ ����
        Marker.transform.position = new Vector3(Parent.transform.position.x, Marker.transform.position.y, Parent.transform.position.z);

        Markerpoint.transform.position = new Vector3(nextObj.transform.position.x, Markerpoint.transform.position.y, nextObj.transform.position.z);

        Dist = Vector3.Distance(Parent.transform.position, nextObj.transform.position);  //������������ �Ÿ� ���

    }

    private void LateUpdate()
    {
        distance = (int)Dist;
        ScriptTxt.text = "���� ��������" + distance + "m"; //text�� ���� �������� �� m ���ҽ��ϴ� ǥ��
        Debug.Log("Dist: " + Dist);
    }

    private void Reset()//��� ������ �ʱ�ȭ��
    {

    }
    void findConn(string a) // ������ �߰������� Ȯ�� 
    {
        string b;
        b = a.Replace("point-", "");

        conn = int.Parse(b);
        Debug.Log("conn is " + conn);

    }

    private void OnTriggerEnter(Collider other)//�߰������̶� ȭ��ǥ�� ������
    {
        if (isStartNv)
        {
            string a = other.name;

            findConn(a);

            int now = next;
            fin = array[array.Length - 1];

            //if (conn == next)//ȭ��ǥ�� �̹� �߰������� �����ߴ��� Ȯ��
            if (conn == now)
            {
                Debug.Log("�߰����� ����");
                n++;
                next = array[n];
                //next = nv.show(n);
                midP = "point-" + array[n].ToString();
                //midP = "point-" + next.ToString();
                nextObj = GameObject.Find(midP); //���������� nextObj ���� ������Ʈ�� �Ҵ�
                Debug.Log("���� ���� �Ҵ�");
            }
            /*
            if (conn != next)//ȭ��ǥ�� �߰������� �ƴ� �ٸ����� ���������� �ٽ� �������
            {
                Debug.Log("��� ��Ż");
                nv.rec(conn);//���� ������ ���������� ��� �缳��
                n = 0;      //�缳���� ��η� �ٽ� ����
                Debug.Log("��� ������");
            }*/

            if (conn == fin)//ȭ��ǥ�� �̹� �߰������� �����ߴ��� Ȯ��
            {
                Debug.Log("������ ����");
            }
        }
    }
    public void startNavigation(string id)
    {

        isStartNv = true;
        RawImage.SetActive(true);
        Arrow = GetComponent<Transform>();
        Debug.Log("navSet Start");
        nv.navSet(9, int.Parse(id));//�׺���̼� ����

        n = 0;
        Debug.Log("nvShow Start");
        next = nv.show(n);

        if(int.Parse(id) == 41)
        {
            array = arrayOne;
            Debug.Log("1�Ҵ� : " + array.Length);
        }
        /*
        else
        {
            array = arrayTwo;
            Debug.Log("2�Ҵ� : " + array.Length);
        }*/
        midP = "point-" + array[n].ToString();
        next = array[n];
        Debug.Log("next point is " + next);
        //midP = "point-" + next.ToString();
        nextObj = GameObject.Find(midP); //���������� nextObj ���� ������Ʈ�� �Ҵ�
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