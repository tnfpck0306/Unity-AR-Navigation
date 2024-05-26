using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class nevigation
{
    int start;
    public int end;

    private static int BigSize = 50000;
    private static int pointNum = 42;

    //�ּҰŸ��� ���-�����ؾ���
    double[] dist = new double[pointNum + 1];

    //�� �������� ����ߴ��� Ȯ��
    bool[] visited = new bool[pointNum + 1];

    //�� �������� �������� ���� �������� �����
    int[] path = new int[pointNum + 1];

    //������̼� ��θ� �����
    int[] route = new int[pointNum];

    //������ ��������-(������ȣ/����� ������ȣ/�Ÿ�)
    List<Tuple<int, int, double>> map = new List<Tuple<int, int, double>>();


    //���� �������� �Է�
    void makemap()
    {
        map.Add(new Tuple<int, int, double>(1, 2, 11.1));
        map.Add(new Tuple<int, int, double>(1, 4, 17.6));

        map.Add(new Tuple<int, int, double>(2, 1, 11.1));
        map.Add(new Tuple<int, int, double>(2, 3, 40));
        map.Add(new Tuple<int, int, double>(2, 5, 30));

        map.Add(new Tuple<int, int, double>(3, 2, 40));
        map.Add(new Tuple<int, int, double>(3, 6, 22.8));

        map.Add(new Tuple<int, int, double>(4, 1, 17.6));
        map.Add(new Tuple<int, int, double>(4, 9, 29.5));

        map.Add(new Tuple<int, int, double>(5, 2, 30));
        map.Add(new Tuple<int, int, double>(5, 10, 27.2));

        map.Add(new Tuple<int, int, double>(6, 3, 22.8));
        map.Add(new Tuple<int, int, double>(6, 8, 7.4));

        map.Add(new Tuple<int, int, double>(7, 8, 9.3));
        map.Add(new Tuple<int, int, double>(7, 10, 26));

        map.Add(new Tuple<int, int, double>(8, 6, 7.4));
        map.Add(new Tuple<int, int, double>(8, 7, 9.3));
        map.Add(new Tuple<int, int, double>(8, 11, 15.6));

        map.Add(new Tuple<int, int, double>(9, 4, 29.5));
        map.Add(new Tuple<int, int, double>(9, 10, 11.7));
        map.Add(new Tuple<int, int, double>(9, 13, 10));

        map.Add(new Tuple<int, int, double>(10, 5, 27.2));
        map.Add(new Tuple<int, int, double>(10, 7, 26));
        map.Add(new Tuple<int, int, double>(10, 9, 11.7));
        map.Add(new Tuple<int, int, double>(10, 11, 28));
        map.Add(new Tuple<int, int, double>(10, 13, 11.5));
        map.Add(new Tuple<int, int, double>(10, 14, 12.3));
        map.Add(new Tuple<int, int, double>(10, 22, 34.3));

        map.Add(new Tuple<int, int, double>(11, 8, 15.6));
        map.Add(new Tuple<int, int, double>(11, 10, 28));
        map.Add(new Tuple<int, int, double>(11, 12, 11));
        map.Add(new Tuple<int, int, double>(11, 15, 10.8));

        map.Add(new Tuple<int, int, double>(12, 11, 11));

        map.Add(new Tuple<int, int, double>(13, 9, 10));
        map.Add(new Tuple<int, int, double>(13, 10, 11.5));
        map.Add(new Tuple<int, int, double>(13, 14, 7));
        map.Add(new Tuple<int, int, double>(13, 19, 18.9));

        map.Add(new Tuple<int, int, double>(14, 10, 12.3));
        map.Add(new Tuple<int, int, double>(14, 13, 7));
        map.Add(new Tuple<int, int, double>(14, 20, 20));

        map.Add(new Tuple<int, int, double>(15, 11, 10.8));
        map.Add(new Tuple<int, int, double>(15, 16, 8));

        map.Add(new Tuple<int, int, double>(16, 15, 8));
        map.Add(new Tuple<int, int, double>(16, 17, 8));
        map.Add(new Tuple<int, int, double>(16, 22, 14.8));

        map.Add(new Tuple<int, int, double>(17, 16, 8));

        map.Add(new Tuple<int, int, double>(18, 19, 21));
        map.Add(new Tuple<int, int, double>(18, 27, 29));

        map.Add(new Tuple<int, int, double>(19, 13, 18.9));
        map.Add(new Tuple<int, int, double>(19, 18, 21));
        map.Add(new Tuple<int, int, double>(19, 20, 19.1));

        map.Add(new Tuple<int, int, double>(20, 14, 20));
        map.Add(new Tuple<int, int, double>(20, 19, 19.1));
        map.Add(new Tuple<int, int, double>(20, 21, 11.6));
        map.Add(new Tuple<int, int, double>(20, 28, 36));

        map.Add(new Tuple<int, int, double>(21, 20, 11.6));
        map.Add(new Tuple<int, int, double>(21, 22, 21.3));
        map.Add(new Tuple<int, int, double>(21, 29, 28.4));

        map.Add(new Tuple<int, int, double>(22, 10, 34.3));
        map.Add(new Tuple<int, int, double>(22, 16, 14.8));
        map.Add(new Tuple<int, int, double>(22, 21, 21.3));
        map.Add(new Tuple<int, int, double>(22, 23, 10.1));

        map.Add(new Tuple<int, int, double>(23, 22, 10.1));
        map.Add(new Tuple<int, int, double>(23, 24, 13.6));
        map.Add(new Tuple<int, int, double>(23, 25, 10.4));

        map.Add(new Tuple<int, int, double>(24, 23, 13.6));

        map.Add(new Tuple<int, int, double>(25, 23, 10.4));
        map.Add(new Tuple<int, int, double>(25, 26, 16.5));
        map.Add(new Tuple<int, int, double>(25, 30, 11.6));

        map.Add(new Tuple<int, int, double>(26, 25, 16.5));

        map.Add(new Tuple<int, int, double>(27, 18, 29));
        map.Add(new Tuple<int, int, double>(27, 28, 22));
        map.Add(new Tuple<int, int, double>(27, 35, 37.3));

        map.Add(new Tuple<int, int, double>(28, 20, 36));
        map.Add(new Tuple<int, int, double>(28, 27, 22));
        map.Add(new Tuple<int, int, double>(28, 29, 26.4));
        map.Add(new Tuple<int, int, double>(28, 31, 19.7));

        map.Add(new Tuple<int, int, double>(29, 21, 28.4));
        map.Add(new Tuple<int, int, double>(29, 28, 26.4));
        map.Add(new Tuple<int, int, double>(29, 30, 21.4));
        map.Add(new Tuple<int, int, double>(29, 32, 21.3));
        map.Add(new Tuple<int, int, double>(29, 41, 5));//��ǳ����

        map.Add(new Tuple<int, int, double>(30, 25, 11.6));
        map.Add(new Tuple<int, int, double>(30, 29, 21.4));
        map.Add(new Tuple<int, int, double>(30, 33, 19.6));
        map.Add(new Tuple<int, int, double>(30, 42, 13));//������

        map.Add(new Tuple<int, int, double>(31, 28, 19.7));
        map.Add(new Tuple<int, int, double>(31, 32, 19.5));
        map.Add(new Tuple<int, int, double>(31, 36, 10.7));

        map.Add(new Tuple<int, int, double>(32, 29, 21.3));
        map.Add(new Tuple<int, int, double>(32, 31, 19.5));
        map.Add(new Tuple<int, int, double>(32, 33, 23.8));

        map.Add(new Tuple<int, int, double>(33, 30, 19.6));
        map.Add(new Tuple<int, int, double>(33, 32, 23.8));
        map.Add(new Tuple<int, int, double>(33, 34, 16));
        map.Add(new Tuple<int, int, double>(33, 42, 7));//������

        map.Add(new Tuple<int, int, double>(34, 33, 16));
        map.Add(new Tuple<int, int, double>(34, 40, 61.7));

        map.Add(new Tuple<int, int, double>(35, 27, 37.3));
        map.Add(new Tuple<int, int, double>(35, 36, 18.2));
        map.Add(new Tuple<int, int, double>(35, 37, 36.1));

        map.Add(new Tuple<int, int, double>(36, 31, 10.7));
        map.Add(new Tuple<int, int, double>(36, 35, 18.2));
        map.Add(new Tuple<int, int, double>(36, 38, 38.8));

        map.Add(new Tuple<int, int, double>(37, 35, 36.1));
        map.Add(new Tuple<int, int, double>(37, 38, 17.2));

        map.Add(new Tuple<int, int, double>(38, 37, 17.2));
        map.Add(new Tuple<int, int, double>(38, 39, 19));

        map.Add(new Tuple<int, int, double>(39, 38, 19));
        map.Add(new Tuple<int, int, double>(39, 40, 28.3));

        map.Add(new Tuple<int, int, double>(40, 39, 28.3));

        map.Add(new Tuple<int, int, double>(41, 29, 5));//��ǳ����

        map.Add(new Tuple<int, int, double>(42, 30, 13));//������
        map.Add(new Tuple<int, int, double>(42, 33, 7));//������

    }

    public void navSet(int s, int e)// �׺���̼� �۵�
    {
        //Console.Write("���������� �Է��ϼ���. "); //�� �κ��� ���� �ڵ����� �޾ƿ��� �ٲ�
        //string a = Console.ReadLine();
        //start = Convert.ToInt32(a);

        //Console.Write("���������� �Է��ϼ���. ");
        // string b = Console.ReadLine();
        // end = Convert.ToInt32(b);


        start = s;

        end = e;

        //���� ���� �Է�
        Debug.Log("makemap Start");
        makemap();

        Debug.Log("serch Start");
        search(start);
    }

    void search(int end)
    {
        for (int i = 0; i < pointNum + 1; i++)//�ִܰŸ� ��� �ʱ�ȭ
        {
            dist[i] = BigSize;
        }

        for (int i = 0; i < pointNum + 1; i++)//�ִܰ�� ��� �ʱ�ȭ
        {
            path[i] = BigSize;
        }

        for (int i = 1; i < pointNum + 1; i++) //visited �ʱ�ȭ
        {
            visited[i] = false;
        }
        visited[0] = true;




        //�������� �Ÿ�-50000���� �ʱ�ȭ
        dist = Enumerable.Repeat<double>(BigSize, pointNum + 1).ToArray<double>();

        //Ž�� ������ ���������� �Ҵ�
        int curr = end;

        //�������� �Ÿ��� 0���� �ʱ�ȭ
        dist[end] = 0;

        Debug.Log("dijk Start");
        Dijk(curr);//�ִܰ�� ���

        Debug.Log("rec Start");
        rec(start);//�ִܰ�� ���
    }

    void Dijk(int curr)//���ͽ�Ʈ�� �˰����� �̿��Ͽ� �ִܰ�� ���
    {
        int[] nextlist = new int[pointNum];
        int listcount = 0;
        for (int i = 0; i < pointNum; i++)
        {
            nextlist[i] = BigSize;
        }
        nextlist[listcount] = curr;
        //��� ������ ����ߴ��� Ȯ��
        int pCount = 0;


        while (pCount < pointNum - 1)
        {
            // Debug.Log(pCount + "pCount fin");
            for (int i = 0; i < map.Count(); i++)
            {
                //Debug.Log(i + "i fin");
                curr = nextlist[pCount];//Ž������ ����
                if (map[i].Item1 == curr)//���� Ž�� ����
                {
                    if (visited[map[i].Item1] == false)//Ž�������� ����� ������ 'Ž������ ����'�����ϰ��
                    {
                        for (int j = 0; j < pointNum; j++)//nextlist �ߺ�Ȯ��
                        {
                            //Debug.Log(j + "j fin");
                            if (nextlist[j] == map[i].Item2)
                                break;
                            if (j == pointNum - 1 && nextlist[j] != map[i].Item2)
                            {
                                listcount++;
                                nextlist[listcount] = map[i].Item2;//nextlist�� �湮�� �������� ����
                                //Debug.Log("list" + listcount + " = " + nextlist[listcount]);
                            }
                        }
                        //���� ã�� ��� ����
                        if (dist[map[i].Item2] > (dist[map[i].Item1] + map[i].Item3))//dist����Ʈ�� ��ϵ� �Ÿ��� �������� ���ο��ΰ� ª�����
                        {
                            dist[map[i].Item2] = dist[map[i].Item1] + map[i].Item3;//dist ����Ʈ ����
                            path[map[i].Item1] = map[i].Item2;//path ����Ʈ ����
                        }
                    }
                    if (i == 113)
                    {
                        visited[map[i].Item1] = true;//���� Ž������ ���¸� Ž�������� �ٲ�
                        pCount += 1;//���� Ž�������� nextlist�� �ִ� ���� Ž���������� �ٲ�
                        //Debug.Log("pCount=" + pCount);
                    }
                    else if (map[i + 1].Item1 != curr)//���� Ž�������� ���� Ž���� ������� 
                    {
                        visited[map[i].Item1] = true;//���� Ž������ ���¸� Ž�������� �ٲ�
                        pCount += 1;//���� Ž�������� nextlist�� �ִ� ���� Ž���������� �ٲ�
                        //Debug.Log("pCount=" + pCount);
                    }
                }
                if (pCount == pointNum - 1)
                    break;
            }
        }
        Debug.Log("cal fin");
    }

    public void rec(int str)//������������ �ִܰ�θ� ����Ѵ�.
    {
        for (int i = 1; i < pointNum; i++) //route �ʱ�ȭ
        {
            route[i] = BigSize;
        }

        Debug.Log("rec start");
        int p = str;

        int j = 0;

        while (p == end)
        {//���������� ��������� ��� ���
            route[j] = path[p];
            Debug.Log(path[p] + "->");
            j++;
            p = path[p];
        }


    }

    public int show(int n)
    {

        int i;
        i = route[n];

        return i;
    }

}