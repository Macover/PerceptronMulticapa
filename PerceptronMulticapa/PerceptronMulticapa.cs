using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronMulticapa
{
    public class PerceptronMulticapa
    {
        public int C { get; set; } //numero maximo de capas
        public int[] n { get; set; } //arquitectura de la red
        public double[,] x { get; set; } //patrones de entrada
        public double[,] y { get; set; } //salidas
        public double[,] s { get; set; } //patrones deseados
        public double[,] a { get; set; } //funciona como la activacion
        public double[,,] w { get; set; }//pesos
        public double[,] u { get; set; } //umbrales
        public int numeroPatrones { get; set; } //numeroPatrones
        /*se utilizan para normalizar los patrones y sean
         * patrones entre 0 y 1 para poder ocupar la funcion sigmoidal
        */
        public double maximoEntradas { get; set; } //El maximo de entradas
        public double minimoEntradas { get; set; } //El minimo de entradas
        public double maximoSalidas { get; set; } //El maximo de salidas
        public double minimoSalidas { get; set; } //El minimo de salidas
        public double errorCuadratico { get; set; } //Se calcula cada que se propaga un patron
        public double errorEntrenamiento { get; set; } //Se calcula al final de cada iteracion
        public double alfa { get; set; }
        public double[,] delta { get; set; }

        Random rand = new Random();
        double sumaErrores = 0.0;

        public PerceptronMulticapa()
        {
            sumaErrores = 0.0;
            errorCuadratico = 0.0;
        }

        public double sigmoidal(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public void crearUmbrales()
        {
            u = new double[C + 1, n.Max() + 1];
            for (int c = 2; c <= C; c++)
            {
                for (int i = 1; i <= n[c]; i++)
                {
                    u[c, i] = rand.NextDouble();
                }
            }
        }
        public void crearPesos()
        {
            w = new double[C + 1, n.Max() + 1, n.Max() + 1];
            for (int c = 1; c <= C - 1; c++)
            {
                for (int i = 1; i <= n[c + 1]; i++)
                {
                    for (int j = 1; j <= n[c]; j++)
                    {
                        w[c, j, i] = rand.NextDouble();
                    }
                }
            }
        }
        public double normalizacion(double valor, double _maximo, double _minimo)
        {
            return (1 / (_maximo - _minimo)) * (valor - _minimo);
        }
        public void normalizarEntradas()
        {
            double[] numeros = new double[numeroPatrones];
            for (int i = 0; i < numeros.Length; i++)
            {
                x[i, 0] = normalizacion(x[i, 0], maximoEntradas, minimoEntradas);
            }
        }
        public void normalizarSalidas()
        {
            double[] numeros = new double[numeroPatrones];
            for (int i = 0; i < numeros.Length; i++)
            {
                s[i, 0] = normalizacion(s[i, 0], maximoSalidas, minimoSalidas);
            }
        } 
        public void encuentraMaxMinEntradas()
        {
            double[] numeros = new double[numeroPatrones];
            for (int i = 0; i < numeros.Length; i++)
            {
                numeros[i] = x[i, 0];
            }
            Array.Sort(numeros);
            maximoEntradas = numeros[numeros.Length - 1];
            minimoEntradas = numeros[0];
        }
        public void encuentraMaxMinSalidas()
        {
            double[] numeros = new double[numeroPatrones];
            for (int i = 0; i < numeros.Length; i++)
            {
                numeros[i] = s[i, 0];
            }
            Array.Sort(numeros);
            maximoSalidas = numeros[numeros.Length - 1];
            minimoSalidas = numeros[0];
        }
        public void activacionEntrada(int patron)
        {
            for (int i = 1; i <= n[1]; i++)
            {
                a[1, i] = x[patron, i - 1];
            }
        }
        public void propagacionNeuronas()
        {
            double suma = 0.0;
            for (int c = 2; c <=C; c++)
            {
                for (int i = 1; i <= n[c]; i++)
                {
                    suma = 0.0;
                    for (int j = 1; j <=n[c-1]; j++)
                    {
                        suma += w[c - 1, j, i] * a[c - 1, j];
                    }
                    a[c, i] = suma + u[c, i];
                    a[c, i] = sigmoidal(a[c, i]);
                }
            }
        }
        public void errorCuadraticoM(int nPatron)
        {
            double temp = 0.0;
            for (int i = 1; i <= n[C]; i++)
            {
                temp += Math.Pow(s[nPatron, 0] - a[C, i], 2);
                y[nPatron, 0] = a[C, i];
            }
            errorCuadratico = temp / 2;
            sumaErrores = errorCuadratico + sumaErrores;
        } 
        public void errorAprendisaje()
        {
            errorEntrenamiento = sumaErrores / numeroPatrones;
            sumaErrores = 0;
        }
        public void retropropagacion(int patron)
        {
            calcularDeltas(patron);
            calcularPesosYUmbrales();
        }
        private void calcularPesosYUmbrales()
        {            
            for (int c = 1; c <=C-1; c++)
            {
                for (int i = 1; i <= n[c+1]; i++)
                {
                    for (int j = 1; j <=n[c]; j++)
                    {
                        w[c, j, i] = w[c, j, i] + alfa * delta[c + 1, i] * a[c, j];
                    }
                    u[c + 1, i] = u[c + 1, i] + alfa * delta[c + 1, i];
                }
            }                
        }
        private void calcularDeltas(int patron)
        {
            double suma = 0.0;
            //caso a
            for (int i = 1; i <= n[C]; i++)
            {
                delta[C, i] = (s[patron, i - 1] - a[C, i]) * a[C, i] * (1 - a[C, i]);
            }
            //caso b
            for (int c = C-1; c > 1; c--)
            {
                for (int j = 1; j <= n[c]; j++)
                {
                    suma = 0.0;
                    for (int i = 1; i <= n[c+1]; i++) 
                    {
                        suma += delta[c + 1, i] * w[c, j, i];
                    }
                    delta[c, j] = a[c, j] * (1 - a[c, j]) * suma;
                }
            }
        }
    }
}
