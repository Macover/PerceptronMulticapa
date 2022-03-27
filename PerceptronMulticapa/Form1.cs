using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerceptronMulticapa
{
    public partial class Form1 : Form
    {

        int[] arquitecturaRed;
        double[,] patronesEntrada;
        double[,] patronesSalida;

        int numeroCapas = 0, neuronasEntrada=0, neuronasSalida=0, numeroPatrones=0;
        Int32 iteracionesMaximas = 0;
        int[] n;
        double alfa = 0.0, errorMinimo = 0.0, neuronasMaximas = 0.0;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnEmpezar_Click(object sender, EventArgs e)
        {
            leerTxt();
        }

        private void leerTxt()
        {
            string[] datos;
            int numeroLinea = 0, posI = 0, posJ = 0;
            
            StreamReader sr = new StreamReader("grafica.txt");
            string linea = sr.ReadLine();
            int posicionSalida = 0;

            while (linea != null)
            {
                switch (numeroLinea)
                {
                    case 0:
                        datos = linea.Split(' ');
                        numeroCapas = Convert.ToInt16(datos[0]);
                        arquitecturaRed = new int[Convert.ToInt16(datos[0]) + 1];
                        for (int i = 0; i < arquitecturaRed.Length; i++)
                        {
                            arquitecturaRed[i] = Convert.ToInt16(datos[i]);
                        }
                        n = new int[numeroCapas + 1];
                        n = arquitecturaRed;
                        break;
                    case 1:
                        alfa = Convert.ToDouble(linea);
                        break;
                    case 2:
                        errorMinimo = Convert.ToDouble(linea);
                        break;
                    case 3:
                        iteracionesMaximas= Convert.ToInt32(linea);
                        break;
                    case 4:
                        numeroPatrones = Convert.ToInt16(linea);
                        break;
                }
                if(numeroLinea == 4)
                {
                    patronesEntrada = new double[numeroPatrones + 1, arquitecturaRed[1]];
                    patronesSalida = new double[numeroPatrones + 1, arquitecturaRed[1]];
                }
                if(numeroLinea <= (numeroPatrones + 4) && numeroLinea > 4)
                {
                    //min6.45
                    if(linea != "")
                    {
                        datos = linea.Split('\t');
                        for (posJ = 0; posJ<arquitecturaRed[1]; posJ++)
                        {
                            patronesEntrada[posI, posJ] = Convert.ToDouble(datos[posJ]);
                        }
                        posI++;
                    }
                }
                if(numeroLinea> numeroPatrones + 5 && posicionSalida < numeroPatrones)
                {
                    if (linea != "")
                    {
                        datos = linea.Split('\t');
                        for (posJ = 0; posJ < arquitecturaRed[1]; posJ++)
                        {
                            patronesSalida[posicionSalida, posJ] = Convert.ToDouble(datos[posJ]);
                        }
                        posicionSalida++;
                    }
                }
                numeroLinea++;
                linea = sr.ReadLine();
            }
            sr.Close();
        }
    }
}
