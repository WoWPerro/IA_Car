using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityScript.TypeSystem;

public class Genetics : MonoBehaviour
{
    public GameObject prefab;
    public static int carsAlive;

    public int poblacion = 30;
    public float probDeMutacion = .05f;

    public int mejoresCromosomas = 5;
    public int peoresCromosomas = 2;
    public int cromosomasParaMutar = 10;
    public int mutacionesporCromosoma = 5;

    private List<GameObject> Cars;

    // Start is called before the first frame update
    void Start()
    {
        carsAlive = poblacion;
        Cars = new List<GameObject>();
        for(int i = 0; i < poblacion; i++)
        {
            Cars.Add(Instantiate(prefab));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(carsAlive <= 0)
        {
            NextEpoch();
            //deleteCars
            carsAlive = poblacion;
        }
    }

    void NextEpoch()
    {
        Cars.Sort((x, y) => x.GetComponent<IA>().score.CompareTo(y.GetComponent<IA>().score));
        List<GameObject> CarsNew;
        CarsNew = new List<GameObject>();
        for (int i = 0; i < mejoresCromosomas; i++)
        {
            CarsNew.Add(Cars[poblacion - 1 - i]);
        }
        for(int i = 0; i < peoresCromosomas; i++)
        {
            CarsNew.Add(Cars[i]);
        }
        int k = mejoresCromosomas + peoresCromosomas;

        for (int i = 0; i < CarsNew.Count; i++)
        {
            Instantiate(CarsNew[i]);
        }

        while (k < poblacion)
        {
            int n1 = UnityEngine.Random.Range(0, k);
            int n2 = UnityEngine.Random.Range(0, k);
            Cross(CarsNew[n1], CarsNew[n2]);
            k++;
        }
        //mutate
        for (int i = 0; i < cromosomasParaMutar; i++)
        {
            int n1 = UnityEngine.Random.Range(0, poblacion);
            IA iaN = CarsNew[n1].GetComponent<IA>();

            for (int j = 0; j < iaN.biases.Length; j++)
            {
                CarsNew[n1].GetComponent<IA>().biases[i].Mutate(mutacionesporCromosoma);
            }
            for (int j = 0; j < iaN.pesos.Length; j++)
            {
                CarsNew[n1].GetComponent<IA>().pesos[i].Mutate(mutacionesporCromosoma);
            }
        }
        //Cars
        //foreach(GameObject g in CarsNew)
        //{
        //    GameObject.Instantiate(g);
        //}
    }

    GameObject Cross(GameObject g1, GameObject g2)
    {
        GameObject r = Instantiate(prefab);
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
}
