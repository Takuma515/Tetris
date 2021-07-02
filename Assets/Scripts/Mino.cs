using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mino : MonoBehaviour
{
    public float previousTime;
    public float fallTime = 1.0f;   // minoが落ちる時間
    public Vector3 rotationPoint;   // mino回転
    AudioSource deleteSE;

    //ステージの大きさ
    int width = 10;
    int height = 20;
    private static Transform[,] grid = new Transform[10, 20];

    void Start()
    {
        deleteSE = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        MinoMovement();
    }

    private void MinoMovement()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMovement())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }
            
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMovement())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }
        // 自動で下に移動させつつ、下矢印キーでも移動する
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            
            if (!ValidMovement())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckLines();
                this.enabled = false;   // スクリプトの無効
                FindObjectOfType<SpawnMino>().NewMino(); // 次のminoの生成
            }

            previousTime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // minoの回転
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }
    }

    // minoをグリッドに追加
    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundX, roundY] = children;

            // GameOver判定
            if (roundY >= height - 1)
            {
                FindObjectOfType<GameManager>().GameOver();
            }
        }
    }

    // ラインの確認
    public void CheckLines()
    {
        for (int i = height -1; i>=0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
            }
        }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
            {
                return false;
            }
        }

        FindObjectOfType<GameManager>().AddScore(); // スコアの追加
        return true;
    }

    // 列を消す
    void DeleteLine(int i)
    {
        deleteSE.Play();
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }

    // 列を下げる
    public void RowDown(int i)
    {
        for (int  y = i; y < height; y++)
        {
            for (int j=0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    // minoの移動範囲の制御
    bool ValidMovement()
    {
        // minoの構成要素それぞれについて
        foreach (Transform children in transform) 
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            // ステージからはみ出しているかどうか
            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }

            // 他のminoが存在するかどうか
            if (grid[roundX, roundY] != null)
            {
                return false;
            }

        }
        return true;
    }
}
