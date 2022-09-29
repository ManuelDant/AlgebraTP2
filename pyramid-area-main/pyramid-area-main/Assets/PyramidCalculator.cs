using System;
using UnityEngine;

public class PyramidCalculator : MonoBehaviour
{
    public Vector3 vectorN1;    //Primer vector, dado por el usuario
    public Vector3 vectorN2;
    public Vector3 vectorN3;

    public Vector3 n1Normalized;
    public Vector3 n2Normalized;
    public Vector3 n3Normalized;

    float pyramid;

    //Teorema de pitagoras
    float Pitagoras(Vector3 vector)
    {
        return MathF.Sqrt(MathF.Pow(vector.x, 2) + MathF.Pow(vector.y, 2) + MathF.Pow(vector.z, 2));
    }

    //Producto cruz entre dos vectores 
    Vector3 CrossProduct(Vector3 firstVector3, Vector3 secondVector3)
    {
        float i = (firstVector3.y * vectorN2.z) - (firstVector3.z * vectorN2.y);
        float j = (firstVector3.x * vectorN2.z) - (firstVector3.z * vectorN2.x);
        float k = (firstVector3.x * vectorN2.y) - (firstVector3.y * vectorN2.x);

        Debug.LogWarning(new Vector3(i, -j, k));

        return new Vector3(i, -j, k);
    }

    void Start()    //Ejecutar una vez
    {
        vectorN2 = new Vector3(vectorN1.y, -vectorN1.x, vectorN1.z);    //Calcular vector rotado 90 grados respecto al primero
        vectorN3 = CrossProduct(vectorN1, vectorN2);    //Calcular vector perpendicular al resto de vectores
        CalculateArea(vectorN1, vectorN2, vectorN3);    //Encontrar el menor de los vectores y utilizarlo para cortar la piramide y formar una base

        pyramid = PyramidSurface(n1Normalized, n2Normalized, n3Normalized); //Sumatoria de la superficie de las 3 caras de la piramide

        Debug.LogError(pyramid);    //Printear el resultado
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position+vectorN1);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position+vectorN2);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position+vectorN3);
    }

    //Calcula area total de las tres caras de la piramide, formadas por los vectores generados
    private void CalculateArea(Vector3 firstVector3, Vector3 secondVector3, Vector3 thirdVector3)
    {
        Vector3[] vector = new Vector3[3];
        vector[0] = firstVector3;
        vector[1] = secondVector3;
        vector[2] = thirdVector3;


        if ((vector[0].x >= 0 && vector[1].x >= 0 && vector[2].x >= 0) || (vector[0].x < 0 && vector[1].x < 0 & vector[2].x < 0))
        {
            for (int i = 0; i < vector.Length - 1; i++)
            {
                for (int j = 0; j < vector.Length - i - 1; j++)
                {
                    if (MathF.Abs(vector[j].x) > MathF.Abs(vector[j + 1].x))
                    {
                        Vector3 aux = vector[j + 1];
                        vector[j + 1] = vector[j];
                        vector[j] = aux;
                    }
                }
            }
            n1Normalized = vector[0];
            n2Normalized = CutPyramid(vector[0], vector[1], 0);
            n3Normalized = CutPyramid(vector[0], vector[2], 0);

        }
        if ((vector[0].y >= 0 && vector[1].y >= 0 && vector[2].y >= 0) || (vector[0].y < 0 && vector[1].y < 0 & vector[2].y < 0))
        {
            for (int i = 0; i < vector.Length - 1; i++)
            {
                for (int j = 0; j < vector.Length - i - 1; j++)
                {
                    if (MathF.Abs(vector[j].y) > MathF.Abs(vector[j + 1].y))
                    {
                        Vector3 aux = vector[j + 1];
                        vector[j + 1] = vector[j];
                        vector[j] = aux;

                    }
                }
            }

            n1Normalized = vector[0];
            n2Normalized = CutPyramid(vector[0], vector[1], 1);
            n3Normalized = CutPyramid(vector[0], vector[2], 1);

        }
        if ((vector[0].z >= 0 && vector[1].z >= 0 && vector[2].z >= 0) || (vector[0].z < 0 && vector[1].z < 0 & vector[2].z < 0))
        {
            for (int i = 0; i < vector.Length - 1; i++)
            {
                for (int j = 0; j < vector.Length - i - 1; j++)
                {
                    if (MathF.Abs(vector[j].z) > MathF.Abs(vector[j + 1].z))
                    {
                        Vector3 aux = vector[j + 1];
                        vector[j + 1] = vector[j];
                        vector[j] = aux;
                    }
                }
            }

            n1Normalized = vector[0];
            n2Normalized = CutPyramid(vector[0], vector[1], 2);
            n3Normalized = CutPyramid(vector[0], vector[2], 2);

        }
    }

    //Tomando la altura de menor valor, corta todos los vectores a esa altura en la piramide
    Vector3 CutPyramid(Vector3 origin, Vector3 toCut, int axis)
    {

        Vector3 cut = new Vector3();
        float test;
        float test3;
        float newX;
        float newY;

        float newZ;
        switch (axis)
        {
            case 0:
                test = MathF.Atan(toCut.x / toCut.y);
                newY = origin.x / MathF.Tan(test);
                test3 = MathF.Atan(toCut.x / toCut.z);
                newZ = origin.x / MathF.Tan(test3);
                cut = new Vector3(origin.x, newY, newZ);
                break;
            case 1:
                test = MathF.Atan(toCut.y / toCut.x);
                newX = origin.y / MathF.Tan(test);
                test3 = MathF.Atan(toCut.y / toCut.z);
                newZ = origin.y / MathF.Tan(test3);
                cut = new Vector3(newX, origin.y, newZ);
                break;
            case 2:

                test = MathF.Atan(toCut.z / toCut.y);
                newY = origin.z / MathF.Tan(test);
                test3 = MathF.Atan(toCut.z / toCut.x);
                newX = origin.z / MathF.Tan(test3);
                cut = new Vector3(newX, newY, origin.z);
                break;
        }

        return cut;
    }

    //Superficie total de las 3 caras externas de la piramide, sumadas
    float PyramidSurface(Vector3 vector1, Vector3 vector2, Vector3 vector3)
    {
        float totalSurface = TriangleArea(vector1, vector2) + TriangleArea(vector1, vector3) + TriangleArea(vector3, vector2);

        return totalSurface;
    }

    //Devuelve la area del triangulo, averiguando una base utilizando dos catetos
    float TriangleArea(Vector3 vector1, Vector3 vector2)
    {
        float triangleBase = MathF.Sqrt(MathF.Pow(vector2.x - vector1.x, 2) + MathF.Pow(vector2.y - vector1.y, 2) + MathF.Pow(vector2.z - vector1.z, 2));

        Vector3 middlePoint;
        middlePoint.x = (vector1.x + vector2.x) / 2;
        middlePoint.y = (vector1.y + vector2.y) / 2;
        middlePoint.z = (vector1.z + vector2.z) / 2;

        float triangleHeight = Pitagoras(middlePoint);

        return triangleBase * triangleHeight / 2;
    }
}