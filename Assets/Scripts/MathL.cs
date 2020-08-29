using System;

public static class MathL
{
    public static float Sigmoid(double value)
    {
        return 1.0f / (1.0f + (float)Math.Exp(-value));
    }

    public static double HyperbolicTangtent(double x)
    {
        if (x < -45.0) return -1.0;
        else if (x > 45.0) return 1.0;
        else return Math.Tanh(x);
    }
}
