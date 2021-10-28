using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Controller : MonoBehaviour
{
    private int[,] stageNum;
    private GameObject[,] stageObject;
    private System.Random rand;
    private int maxElement = 2;

    private void Start()
    {
        stageNum = new int[4, 4];
        stageObject = new GameObject[4, 4];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                stageObject[i, j] = GameObject.Find("Image (" + (i * 4 + j) + ")");
            }
        }
        rand = new System.Random(Environment.TickCount);
        maxElement = 2;

        StageSetup();
    }

    private void Update()
    {
        Flick();
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(MoveUp()) GeneratElement();
            ShowElement();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (MoveDown()) GeneratElement();
            ShowElement();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (MoveRight()) GeneratElement();
            ShowElement();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (MoveLeft()) GeneratElement();
            ShowElement();
        }
    }

    private void ShowElement()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (stageNum[i, j] == -1) stageObject[i, j].transform.GetChild(0).GetComponent<Text>().text = "";
                else stageObject[i, j].transform.GetChild(0).GetComponent<Text>().text = stageNum[i, j].ToString();
                switch (stageNum[i, j])
                {
                    case 2:
                        stageObject[i, j].GetComponent<Image>().color = new Color(1f, 1f, 85f / 225f);
                        break;
                    case 4:
                        stageObject[i, j].GetComponent<Image>().color = new Color(1f, 170f / 225f, 85f / 225f);
                        break;
                    case 8:
                        stageObject[i, j].GetComponent<Image>().color = new Color(1f, 85f / 225f, 85f / 225f);
                        break;
                    case 16:
                        stageObject[i, j].GetComponent<Image>().color = new Color(1f, 85f / 225f, 1f);
                        break;
                    case 32:
                        stageObject[i, j].GetComponent<Image>().color = new Color(170f / 225f, 85f / 225f, 1f);
                        break;
                    case 64:
                        stageObject[i, j].GetComponent<Image>().color = new Color(85f / 225f, 85f / 225f, 1f);
                        break;
                    case 128:
                        stageObject[i, j].GetComponent<Image>().color = new Color(85f / 225f, 170f / 225f, 1f);
                        break;
                    case 256:
                        stageObject[i, j].GetComponent<Image>().color = new Color(85f / 225f, 1f, 1f);
                        break;
                    case 512:
                        stageObject[i, j].GetComponent<Image>().color = new Color(85f / 225f, 1f, 170f / 225f);
                        break;
                    case 1024:
                        stageObject[i, j].GetComponent<Image>().color = new Color(85f / 225f, 1f, 85f / 225f);
                        break;
                    case 2048:
                        stageObject[i, j].GetComponent<Image>().color = new Color(0f / 225f, 1f, 0f / 225f);
                        break;
                    default:
                        stageObject[i, j].GetComponent<Image>().color = Color.white;
                        break;
                }
                if (stageNum[i, j] > 2048) stageObject[i, j].GetComponent<Image>().color = Color.gray;
            }
        }
    }


    private void GeneratElement()
    {
        int cnt = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (stageNum[i, j] == -1) cnt++;
            }
        }
        int generatNum;

        if (cnt == 16) generatNum = 2;
        else if (cnt == 0) generatNum = 0;
        else generatNum = 1;
        
        int x, y;
        List<int> listX = new List<int>(), listY = new List<int>();
        for (int i = 0; i < generatNum; i++)
        {
            do
            {
                x = rand.Next(4);
                y = rand.Next(4);
            } while (stageNum[x, y] != -1);
            listX.Add(x);
            listY.Add(y);
            stageNum[x, y] = ChoiceElementNum();
        }
        ShowElement();
        for (int i = 0; i < generatNum; i++)
        {
            stageObject[listX[i],listY[i]].GetComponent<Animator>().SetTrigger("Generate");
        }
    }

    private int ChoiceElementNum()
    {
        int c = maxElement;
        while (c > 2)
        {
            if (1.0 / (double)c >= rand.NextDouble())
            {
                return c;
            }
            c /= 2;
        }
        return 2;
    }

    public void StageSetup()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                stageNum[i, j] = -1;
            }
        }
        GeneratElement();
        ShowElement();
    }

    private Vector3 touchStartPos;
    private Vector3 touchEndPos;

    void Flick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchStartPos = new Vector3(Input.mousePosition.x,
                                        Input.mousePosition.y,
                                        Input.mousePosition.z);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            touchEndPos = new Vector3(Input.mousePosition.x,
                                      Input.mousePosition.y,
                                      Input.mousePosition.z);
            GetDirection();
        }
    }

    private void GetDirection()
    {
        float directionX = touchEndPos.x - touchStartPos.x;
        float directionY = touchEndPos.y - touchStartPos.y;

        if (Mathf.Abs(directionY) < Mathf.Abs(directionX))
        {
            if (30 < directionX)
            {
                if (MoveRight()) GeneratElement();
                ShowElement();
            }
            else if (-30 > directionX)
            {
                if (MoveLeft()) GeneratElement();
                ShowElement();
            }
        }
        else if (Mathf.Abs(directionX) < Mathf.Abs(directionY))
        {
            if (30 < directionY)
            {
                if (MoveUp()) GeneratElement();
                ShowElement();
            }
            else if (-30 > directionY)
            {
                if (MoveDown()) GeneratElement();
                ShowElement();
            }
        }
    }

    private bool MoveRight()
    {
        bool f = false;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 3; j >= 0; j--)
            {
                if (stageNum[i, j] != -1 && j != 3)
                {
                    for (int k = j + 1; k < 4; k++)
                    {
                        if (stageNum[i, k] == stageNum[i, j])
                        {
                            stageNum[i, k] = stageNum[i, j] * 2;
                            stageNum[i, j] = -1;
                            f = true;
                            break;
                        }
                        else if (stageNum[i, k] != stageNum[i, j] && stageNum[i, k] != -1)
                        {
                            stageNum[i, k - 1] = stageNum[i, j];
                            if (k - 1 != j)
                            {
                                stageNum[i, j] = -1;
                                f = true;
                            }
                            break;
                        }
                        else if (k == 3)
                        {
                            stageNum[i, k] = stageNum[i, j];
                            stageNum[i, j] = -1;
                            f = true;
                            break;
                        }
                    }
                }
            }
        }
        return f;
    }

    private bool MoveLeft()
    {
        bool f = false;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (stageNum[i, j] != -1 && j != 0)
                {
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if(stageNum[i, k] == stageNum[i, j])
                        {
                            stageNum[i, k] = stageNum[i, j] * 2;
                            stageNum[i, j] = -1;
                            f = true;
                            break;
                        }
                        else if (stageNum[i, k] != stageNum[i, j] && stageNum[i, k] != -1)
                        {
                            stageNum[i, k + 1] = stageNum[i, j];
                            if (k + 1 != j)
                            {
                                stageNum[i, j] = -1;
                                f = true;
                            }
                            break;
                        }
                        else if (k == 0)
                        {
                            stageNum[i, k] = stageNum[i, j];
                            stageNum[i, j] = -1;
                            f = true;
                            break;
                        }
                    }
                }
            }
        }
        return f;
    }

    private bool MoveUp()
    {
        bool f = false;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 3; j >= 0; j--)
            {
                if (stageNum[j, i] != -1 && j != 3)
                {
                    for (int k = j + 1; k < 4; k++)
                    {
                        if (stageNum[k, i] == stageNum[j, i])
                        {
                            f = true;
                            stageNum[k, i] = stageNum[j, i] * 2;
                            stageNum[j, i] = -1;
                            break;
                        }
                        else if (stageNum[k, i] != stageNum[j, i] && stageNum[k, i] != -1)
                        {

                            stageNum[k - 1, i] = stageNum[j, i];
                            if (k - 1 != j)
                            {
                                stageNum[j, i] = -1;
                                f = true;
                            }
                            break;
                        }
                        else if (k == 3)
                        {
                            f = true;
                            stageNum[k, i] = stageNum[j, i];
                            stageNum[j, i] = -1;
                            break;
                        }
                    }
                }
            }
        }
        return f;

    }

    private bool MoveDown()
    {
        bool f = false;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (stageNum[j, i] != -1 && j != 0)
                {
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (stageNum[k, i] == stageNum[j, i])
                        {
                            stageNum[k, i] = stageNum[j, i] * 2;
                            stageNum[j, i] = -1;
                            f = true;
                            break;
                        }
                        else if (stageNum[k, i] != stageNum[j, i] && stageNum[k, i] != -1)
                        {
                            stageNum[k + 1, i] = stageNum[j, i];
                            if (k + 1 != j)
                            {
                                stageNum[j, i] = -1;
                                f = true;
                            }
                            break;
                        }
                        else if (k == 0)
                        {
                            stageNum[k, i] = stageNum[j, i];
                            stageNum[j, i] = -1;
                            f = true;
                            break;
                        }
                    }
                }
            }
        }
        return f;
    }
}
