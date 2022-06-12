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

    //최소거리를 기록-수정해야함
    double[] dist = new double[pointNum + 1];

    //각 지점들을 계산했는지 확인
    bool[] visited = new bool[pointNum + 1];

    //각 지점에서 목적지로 가는 다음점을 기록함
    int[] path = new int[pointNum + 1];

    //내비게이션 경로를 기록함
    int[] route = new int[pointNum];

    //지도의 간선정보-(지점번호/연결된 지점번호/거리)
    List<Tuple<int, int, double>> map = new List<Tuple<int, int, double>>();


    //지도 간선정보 입력
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
        map.Add(new Tuple<int, int, double>(29, 41, 5));//영풍문고

        map.Add(new Tuple<int, int, double>(30, 25, 11.6));
        map.Add(new Tuple<int, int, double>(30, 29, 21.4));
        map.Add(new Tuple<int, int, double>(30, 33, 19.6));
        map.Add(new Tuple<int, int, double>(30, 42, 13));//공수간

        map.Add(new Tuple<int, int, double>(31, 28, 19.7));
        map.Add(new Tuple<int, int, double>(31, 32, 19.5));
        map.Add(new Tuple<int, int, double>(31, 36, 10.7));

        map.Add(new Tuple<int, int, double>(32, 29, 21.3));
        map.Add(new Tuple<int, int, double>(32, 31, 19.5));
        map.Add(new Tuple<int, int, double>(32, 33, 23.8));

        map.Add(new Tuple<int, int, double>(33, 30, 19.6));
        map.Add(new Tuple<int, int, double>(33, 32, 23.8));
        map.Add(new Tuple<int, int, double>(33, 34, 16));
        map.Add(new Tuple<int, int, double>(33, 42, 7));//공수간

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

        map.Add(new Tuple<int, int, double>(41, 29, 5));//영풍문고

        map.Add(new Tuple<int, int, double>(42, 30, 13));//공수간
        map.Add(new Tuple<int, int, double>(42, 33, 7));//공수간

    }

    public void navSet(int s, int e)// 네비게이션 작동
    {
        //Console.Write("시작지점을 입력하세요. "); //이 부분은 이후 자동으로 받아오게 바꿈
        //string a = Console.ReadLine();
        //start = Convert.ToInt32(a);

        //Console.Write("도착지점을 입력하세요. ");
        // string b = Console.ReadLine();
        // end = Convert.ToInt32(b);


        start = s;

        end = e;

        //지도 정보 입력
        Debug.Log("makemap Start");
        makemap();

        Debug.Log("serch Start");
        search(start);
    }

    void search(int end)
    {
        for (int i = 0; i < pointNum + 1; i++)//최단거리 기록 초기화
        {
            dist[i] = BigSize;
        }

        for (int i = 0; i < pointNum + 1; i++)//최단경로 기록 초기화
        {
            path[i] = BigSize;
        }

        for (int i = 1; i < pointNum + 1; i++) //visited 초기화
        {
            visited[i] = false;
        }
        visited[0] = true;




        //지점간의 거리-50000으로 초기화
        dist = Enumerable.Repeat<double>(BigSize, pointNum + 1).ToArray<double>();

        //탐색 지점에 시작지점을 할당
        int curr = end;

        //시작점의 거리를 0으로 초기화
        dist[end] = 0;

        Debug.Log("dijk Start");
        Dijk(curr);//최단경로 계산

        Debug.Log("rec Start");
        rec(start);//최단경로 출력
    }

    void Dijk(int curr)//다익스트라 알고리즘을 이용하여 최단경로 계산
    {
        int[] nextlist = new int[pointNum];
        int listcount = 0;
        for (int i = 0; i < pointNum; i++)
        {
            nextlist[i] = BigSize;
        }
        nextlist[listcount] = curr;
        //모든 지점을 계산했는지 확인
        int pCount = 0;


        while (pCount < pointNum - 1)
        {
            // Debug.Log(pCount + "pCount fin");
            for (int i = 0; i < map.Count(); i++)
            {
                //Debug.Log(i + "i fin");
                curr = nextlist[pCount];//탐색지점 설정
                if (map[i].Item1 == curr)//현재 탐색 지점
                {
                    if (visited[map[i].Item1] == false)//탐색지점과 연결된 지점이 '탐색되지 않음'상태일경우
                    {
                        for (int j = 0; j < pointNum; j++)//nextlist 중복확인
                        {
                            //Debug.Log(j + "j fin");
                            if (nextlist[j] == map[i].Item2)
                                break;
                            if (j == pointNum - 1 && nextlist[j] != map[i].Item2)
                            {
                                listcount++;
                                nextlist[listcount] = map[i].Item2;//nextlist에 방문할 지점으로 넣음
                                //Debug.Log("list" + listcount + " = " + nextlist[listcount]);
                            }
                        }
                        //새로 찾은 경로 갱신
                        if (dist[map[i].Item2] > (dist[map[i].Item1] + map[i].Item3))//dist리스트에 기록된 거리와 비교했을때 새로운경로가 짧을경우
                        {
                            dist[map[i].Item2] = dist[map[i].Item1] + map[i].Item3;//dist 리스트 갱신
                            path[map[i].Item1] = map[i].Item2;//path 리스트 갱신
                        }
                    }
                    if (i == 113)
                    {
                        visited[map[i].Item1] = true;//현재 탐색지점 상태를 탐색함으로 바꿈
                        pCount += 1;//현재 탐색지점을 nextlist에 있는 다음 탐색지점으로 바꿈
                        //Debug.Log("pCount=" + pCount);
                    }
                    else if (map[i + 1].Item1 != curr)//현재 탐색지점에 대한 탐색이 끝날경우 
                    {
                        visited[map[i].Item1] = true;//현재 탐색지점 상태를 탐색함으로 바꿈
                        pCount += 1;//현재 탐색지점을 nextlist에 있는 다음 탐색지점으로 바꿈
                        //Debug.Log("pCount=" + pCount);
                    }
                }
                if (pCount == pointNum - 1)
                    break;
            }
        }
        Debug.Log("cal fin");
    }

    public void rec(int str)//목적지까지의 최단경로를 기록한다.
    {
        for (int i = 1; i < pointNum; i++) //route 초기화
        {
            route[i] = BigSize;
        }

        Debug.Log("rec start");
        int p = str;

        int j = 0;

        while (p == end)
        {//목적지부터 출발점까지 경로 기록
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