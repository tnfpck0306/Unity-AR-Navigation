using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArchToolkit;
using UnityEngine.EventSystems;
public class arrow : MonoBehaviour
{
    Transform Arrow;//ȭ��ǥ ��ü
    public Transform user;

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

    void Action() // arrow�� ����ڿ��� ���ӽ�Ŵ
    {
        Parent = ArchToolkitManager.Instance.visitor.Head;
        Child = this.gameObject;

        Child.transform.parent = Parent.transform;

        this.transform.localPosition = new Vector3(0.0f, -1.0f, 2.00f);
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
            //fin = array[array.Length - 1];

            //if (conn == next)//ȭ��ǥ�� �̹� �߰������� �����ߴ��� Ȯ��
            if (conn == now)
            {
                Debug.Log("�߰����� ����");
                n++;
                //next = array[n];
                next = nv.show(n);
                //midP = "point-" + array[n].ToString();
                midP = "point-" + next.ToString();
                Debug.Log("���� ���� �Ҵ�");
            }

            if (conn != next)//ȭ��ǥ�� �߰������� �ƴ� �ٸ����� ���������� �ٽ� �������
            {
                Debug.Log("��� ��Ż");
                rerouting();
                Debug.Log("��� ������");
            }

            if (conn == fin)//ȭ��ǥ�� �̹� �߰������� �����ߴ��� Ȯ��
            {
                Debug.Log(conn);
                Debug.Log(next);
                Debug.Log("������ ����");
            }
        }
    }
    public void startNavigation(string id)
    {
        
        isStartNv = true;
        Arrow = GetComponent<Transform>();
        Debug.Log("navSet Start");
        nv.navSet(4, int.Parse(id));//�׺���̼� ����
        n = 0;
        Debug.Log("nvShow Start");
        next = nv.show(n);
        /*if(int.Parse(id) == 41)
        {
            array = arrayOne;
            Debug.Log("1�Ҵ� : " + array.Length);
        }
        else
        {
            array = arrayTwo;
            Debug.Log("2�Ҵ� : " + array.Length);
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
        nv.navReset(conn);//���� ������ ������������ �ٽ� ��θ� �����
        n = 0;
        next = nv.show(n);
        Debug.Log("next point is" + next);
        midP = "point-" + next.ToString();
        Debug.Log(midP);
        //���ο� ��η� �ٽ� ������
    }
}