using System;

public class Matriz
{
    float[,] mat;
    public int rows;
    public int columns;

    public Matriz(int _rows, int _columns)
    {
        rows = _rows;
        columns = _columns;
        mat = new float[rows, columns];
    }

    public float GetAt(int x, int y)
    {
        return mat[x,y];
    }

    public void SetAt(int x, int y, float v)
    {
        mat[x,y] = v;
    }

    public static Matriz operator +(Matriz m1, Matriz m2)
    {
        if(m1.rows == m2.rows && m1.columns == m2.columns)
        {
            for (int i = 0; i < m1.rows; i++)
            {
                for (int j = 0; j < m1.columns; j++)
                {
                    m1.SetAt(i, j, m1.GetAt(i, j) + m2.GetAt(i, j));
                }
            }
        }
        return m1;
    }

    public static Matriz operator *(Matriz m1, Matriz m2)
    {
        Matriz mat2 = new Matriz(0,0);

        if (m1.columns == m2.rows)
        {
            Matriz mat3 = new Matriz(m1.rows, m2.columns);

            for (int i = 0; i < m1.rows; i++)
            {
                for (int k = 0; k < m2.columns; k++)
                {
                    for (int j = 0; j < m2.rows; j++)
                    {
                        //UnityEngine.Debug.Log(i + " " + k + " " + j);
                        mat3.SetAt(i, k, mat3.GetAt(i, k) + m1.GetAt(i, j) * m2.GetAt(j, k));
                    }
                }
            }
            //UnityEngine.Debug.Log("Succes");
            return mat3; 
        }
        else
        {
            //UnityEngine.Debug.LogError("FAIL");
            return mat2;
        }
    }


    public void RandomInitialize()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                mat[i, j] = UnityEngine.Random.Range(-100f, 100f);
            }
        }
    }

    public Matriz Transpose()
    {
        Matriz m = new Matriz(columns,rows);
        for(int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                m.SetAt(j, i, mat[i, j]);
            }
        }
        return m;
    }
    
    public static Matriz SinglePointCross(Matriz m1, Matriz m2)
    {
        Matriz mr = new Matriz(m1.rows, m1.columns);
        int crosspointC = UnityEngine.Random.Range(0, m1.columns);
        int crosspointR = UnityEngine.Random.Range(0, m1.rows);
        if (m1.columns == m2.columns && m1.rows == m2.rows)
        {
            for (int i = 0; i < m1.rows; i++)
            {
                for (int j = 0; j < m1.columns; j++)
                {
                    if(i < crosspointC || j < crosspointR)
                    //if(i < crosspointC)
                    {
                        mr.SetAt(i, j, m1.GetAt(i, j));
                    }
                    else
                    {
                        mr.SetAt(i, j, m2.GetAt(i, j));
                    }
                }
            }
            return mr;
        }
        UnityEngine.Debug.LogError("BAD SINGLEPOINTCROSS");
        return null;
    }

    public void Mutate(int mut)
    {
        for(int i = 0; i < mut; i++)
        {
            int n1 = UnityEngine.Random.Range(0, rows - 1);
            int n2 = UnityEngine.Random.Range(0, columns - 1);
            mat[n1, n2] = mat[n1,n2] + UnityEngine.Random.Range(-100, 100);
        }
    }

    public void print()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Console.WriteLine(mat[i,j]);
            }
        }
    }
}
