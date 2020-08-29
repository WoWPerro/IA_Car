using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IA : MonoBehaviour
{
    public Text scoreText;
    public int capas = 2;
    public int neuronas = 10;
    public Matriz[] pesos;
    public Matriz[] biases;
    Matriz entradas;
    float acceleration;
    float rotation;
    public float score;
    bool lost = false;

    //ForFitness
    private Vector3 lastPosition;
    private float distanceTraveled = 0;
    float accelerationPR = 0;
    int accelerationProm = 0;

    // Start is called before the first frame update
    void Start()
    {
        pesos = new Matriz[capas];
        biases = new Matriz[capas];
        entradas = new Matriz(1, 3);

        for (int i = 0; i < capas; i++)
        {
            if(i == 0)
            {
                pesos[i] = new Matriz(3, neuronas);
                pesos[i].RandomInitialize();
                biases[i] = new Matriz(1, 3);
                biases[i].RandomInitialize();
            }
            else if(i == capas - 1)
            {
                pesos[i] = new Matriz(2, neuronas);
                pesos[i].RandomInitialize();
                biases[i] = new Matriz(1, 2);
                biases[i].RandomInitialize();
            }
            else
            {
                pesos[i] = new Matriz(neuronas, neuronas);
                pesos[i].RandomInitialize();
                biases[i] = new Matriz(1, neuronas);
                biases[i].RandomInitialize();
            }
        }
        
    }

    public void Initialize()
    {
        pesos = new Matriz[capas];
        biases = new Matriz[capas];
        entradas = new Matriz(1, 3);

        for (int i = 0; i < capas; i++)
        {
            if (i == 0)
            {
                pesos[i] = new Matriz(3, neuronas);
                pesos[i].RandomInitialize();
                biases[i] = new Matriz(1, 3);
                biases[i].RandomInitialize();
            }
            else if (i == capas - 1)
            {
                pesos[i] = new Matriz(2, neuronas);
                pesos[i].RandomInitialize();
                biases[i] = new Matriz(1, 2);
                biases[i].RandomInitialize();
            }
            else
            {
                pesos[i] = new Matriz(neuronas, neuronas);
                pesos[i].RandomInitialize();
                biases[i] = new Matriz(1, neuronas);
                biases[i].RandomInitialize();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!lost)
        {
            float FD = GetComponent<Car>().ForwardDistance;
            float RD = GetComponent<Car>().RightDistance;
            float LD = GetComponent<Car>().LeftDistance;
            entradas.SetAt(0, 0, FD);
            entradas.SetAt(0, 1, RD);
            entradas.SetAt(0, 2, LD);
            resolve();

            transform.Translate(Vector3.forward * acceleration);
            //transform.rotation = Quaternion.Euler(0, rotation *30, 0);
            transform.eulerAngles = transform.eulerAngles + new Vector3(0, (rotation * 90) * 0.02f, 0);

            distanceTraveled += Vector3.Distance(transform.position, lastPosition);
            lastPosition = transform.position;
            accelerationPR += acceleration;
            accelerationProm++;
            SetScore();
            //scoreText.text = score.ToString();
        }
        
    }

    void resolve()
    {
        Matriz result;
        result = Activation((entradas * pesos[0]) + biases[0]);
        for (int i = 1; i < capas; i++)
        {
            //result = result * (Activation((pesos[i] * entradas) + biases[i]));
            result = (Activation((pesos[i] * result.Transpose()) + biases[i]));
        }
        ActivationLast(result);
    }


    Matriz Activation(Matriz m)
    {
        for (int i = 0; i < m.rows; i++)
        {
            for (int j = 0; j < m.columns; j++)
            {
                //m.SetAt(i, j, MathL.Sigmoid(m.GetAt(i, j)));
                m.SetAt(i, j, (float)MathL.HyperbolicTangtent(m.GetAt(i, j)));
            }
        }
        return m;
    }

    void ActivationLast(Matriz m)
    {
        rotation = (float)MathL.HyperbolicTangtent(m.GetAt(0, 0));
        acceleration = MathL.Sigmoid(m.GetAt(1, 0));
    }

    void SetScore()
    {
        float FD = GetComponent<Car>().ForwardDistance;
        float RD = GetComponent<Car>().RightDistance;
        float LD = GetComponent<Car>().LeftDistance;
        float s = (FD + RD + LD) / 3;
        s += ((distanceTraveled*8) + (acceleration));
        score += (float)Math.Pow(s,2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            lost = true;
            Genetics.carsAlive--;
        }
    }
}