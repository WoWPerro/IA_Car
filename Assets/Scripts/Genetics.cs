using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using UnityScript.TypeSystem;

public class Genetics : MonoBehaviour
{
    public Text EpochsText;
    public int epochs = 0;
    public GameObject prefab;
    public static int carsAlive;

    public int poblacion = 30;
    public float probDeMutacion = .05f;

    public int mejoresCromosomas = 5;
    public int peoresCromosomas = 2;
    public int cromosomasParaMutar = 20;
    public int mutacionesporCromosoma = 5;

    private List<GameObject> Cars;
    private List<GameObject> newerCars;

    // Start is called before the first frame update
    void Start()
    {
        carsAlive = poblacion;
        Cars = new List<GameObject>();
        newerCars = new List<GameObject>();
        for(int i = 0; i < poblacion; i++)
        {
            GameObject newObject = Instantiate(prefab) as GameObject;
            Cars.Add(newObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        EpochsText.text = "Epochs: " + epochs.ToString();
        if(carsAlive <= 0)
        {
            NextEpoch();
            DeleteCars();
            carsAlive = poblacion;
            epochs++;
        }
    }

    void DeleteCars()
    {
        for(int i = 0; i < Cars.Count; i++)
        {
            Destroy(Cars[i]);
        }
        Cars.Clear();
        Cars = newerCars;
    }

    void NextEpoch()
    {
        Cars.Sort((x, y) => x.GetComponent<IA>().score.CompareTo(y.GetComponent<IA>().score));
        List<GameObject> CarsNew;
        CarsNew = new List<GameObject>();
        for (int i = 0; i < mejoresCromosomas; i++)
        {
            CarsNew.Add(Copy(Cars[poblacion - 1 - i]));
        }
        for(int i = 0; i < peoresCromosomas; i++)
        {
            CarsNew.Add(Copy(Cars[i]));
        }
        int k = mejoresCromosomas + peoresCromosomas;

        //for (int i = 0; i < CarsNew.Count; i++)
        //{
        //    Instantiate(CarsNew[i]);
        //}

        while (k < poblacion)
        {
            int n1 = UnityEngine.Random.Range(0, k - 1);
            int n2 = UnityEngine.Random.Range(0, k - 1);
            CarsNew.Add(Cross(CarsNew[n1], CarsNew[n2]));
            k++;
        }
        //mutate
        
        for (int i = 0; i < cromosomasParaMutar; i++)
        {
            //Debug.Log("1");
            int n1 = UnityEngine.Random.Range(0, poblacion - 1);
            IA iaN = CarsNew[n1].GetComponent<IA>();

            //Debug.Log("2");
            for (int j = 0; j < iaN.biases.Length; j++)
            {
                CarsNew[n1].GetComponent<IA>().biases[j].Mutate(mutacionesporCromosoma);
            }
            //Debug.Log("3");
            for (int j = 0; j < iaN.pesos.Length; j++)
            {
                CarsNew[n1].GetComponent<IA>().pesos[j].Mutate(mutacionesporCromosoma);
            }
        }
        newerCars = CarsNew;
    }

    GameObject Cross(GameObject g1, GameObject g2)
    {
        GameObject newObject = Instantiate(prefab) as GameObject;
        GameObject r = newObject;
        r.GetComponent<IA>().Initialize();
        IA ia1 = g1.GetComponent<IA>();
        IA ia2 = g2.GetComponent<IA>();

        for(int i = 0; i < ia1.biases.Length; i++)
        {
            r.GetComponent<IA>().biases[i] = Matriz.SinglePointCross(ia1.biases[i], ia2.biases[i]);
        }

        for (int i = 0; i < ia1.pesos.Length; i++)
        {
            r.GetComponent<IA>().pesos[i] = Matriz.SinglePointCross(ia1.pesos[i], ia2.pesos[i]);
        }
        return r;
    }

    GameObject Copy(GameObject c)
    {
        GameObject newObject = Instantiate(prefab) as GameObject;
        GameObject r = newObject;
        r.GetComponent<IA>().Initialize();
        IA ia1 = c.GetComponent<IA>();

        for (int i = 0; i < ia1.biases.Length; i++)
        {
            r.GetComponent<IA>().biases[i] = ia1.biases[i];
        }

        for (int i = 0; i < ia1.pesos.Length; i++)
        {
            r.GetComponent<IA>().pesos[i] = ia1.pesos[i];
        }
        return r;
    }
}
