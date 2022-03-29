using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerceptronMulticapa
{
    public partial class Form1 : Form
    {
        PerceptronMulticapa neurona = new PerceptronMulticapa();

        int[] arquitecturaRed;
        double[,] patronesEntrada;
        double[,] patronesSalida;

        int numeroCapas = 0, neuronasEntrada=0, neuronasSalida=0, numeroPatrones=0;
        Int32 iteracionesMaximas = 0;
        int[] n;
        double alfa = 0.0, errorMinimo = 0.0, neuronasMaximas = 0.0;

        //SERIAL
        SerialPort serialPort1;        
        string Recibidos;
        double distanciaSerial = 0;       
        
        public Form1()
        {
            InitializeComponent();
            //PUERTO SERIAL
            serialPort1 = new SerialPort();
            serialPort1.PortName = "COM3";
            serialPort1.BaudRate = 9600;
            serialPort1.DtrEnable = true;
            serialPort1.ReadTimeout = 500;
            try
            {
                serialPort1.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show("Revisa el ESP32");
            }
        }        
        private void btnGetPatrones_Click(object sender, EventArgs e)
        {
            serialPort1.DataReceived += serialPort1_DataReceived;
            MessageBox.Show(distanciaSerial + "distancia");
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Recibidos = serialPort1.ReadLine();
            this.BeginInvoke(new lineReceivedEvent(lineReceived), Recibidos);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.DataReceived += serialPort1_DataReceived;
            double var = distanciaSerial;
            lblValorRecibido.Text = "Valor recibido: " + distanciaSerial.ToString();
            //
            double[,] patronesEntradaF = new Double[1, 1];          
            //
            neurona.numeroPatrones = 1;
           
            neurona.aF = new double[neurona.C + 1, (int)neuronasMaximas + 1];
            neurona.yF = new double[neurona.numeroPatrones + 1, n[neurona.C] + 1];
            neurona.normalizarEntradasF(var);
            for (int i = 0; i < neurona.numeroPatrones; i++)
            {
                neurona.activacionEntradaF(i);
                neurona.propagacionNeuronasF(i);
            }
            double salidaYPerceptron = neurona.yF[0, 0];
            lblValorSalida.Text = "Valor salida perceptron: "+neurona.yF[0,0].ToString();
            string cadenaLabel = "";
            if(salidaYPerceptron < 0.25)
            {
                cadenaLabel = "Muy cerca";
            }
            else if(salidaYPerceptron > 0.25 && salidaYPerceptron <= 0.50)
            {
                cadenaLabel = "Cerca";
            }
            else if (salidaYPerceptron > 0.50 && salidaYPerceptron <= 0.75)
            {
                cadenaLabel = "Lejos";
            }
            else
            {
                cadenaLabel = "Muy Lejos";
            }
            lblValorLinguistico.Text = cadenaLabel;

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblValorSalida_Click(object sender, EventArgs e)
        {

        }

        private delegate void lineReceivedEvent(string line);
        private void lineReceived(string line)
        {
            distanciaSerial = Convert.ToDouble(line);
            //label1.Text = line;
        }
        private void btnEmpezar_Click(object sender, EventArgs e)
        {
            leerTxt();
            try
            {
                int[] temp = new int[numeroCapas + 1];
                Array.Copy(arquitecturaRed, temp, arquitecturaRed.Length);
                Array.Sort(temp);
                neuronasMaximas = temp[temp.Length - 1];
                
                neurona.numeroPatrones = numeroPatrones;
                neurona.n = n;
                neurona.x = patronesEntrada;                
                neurona.errorEntrenamiento = 1;
                neurona.C = numeroCapas;
                neurona.a = new double[neurona.C + 1, (int)neuronasMaximas + 1];
                neurona.y = new double[neurona.numeroPatrones + 1, n[neurona.C] + 1];
                neurona.delta = new double[neurona.C + 1, (int)neuronasMaximas + 1];
                neurona.s = new double[neurona.numeroPatrones + 1, n[neurona.C] + 1];
                neurona.errorCuadratico = 0;
                neurona.alfa = alfa;
                neurona.s = patronesSalida;
                neurona.crearPesos();
                neurona.crearUmbrales();
                neurona.encuentraMaxMinEntradas();
                neurona.encuentraMaxMinSalidas();
                neurona.normalizarEntradas();
                neurona.normalizarSalidas();
                while(neurona.errorEntrenamiento>=errorMinimo && iteracionesMaximas >= 0)
                {
                    neurona.errorCuadratico = 0;
                    for (int i = 0; i < numeroPatrones; i++)
                    {
                        neurona.activacionEntrada(i);
                        neurona.propagacionNeuronas();
                        neurona.errorCuadraticoM(i);
                        neurona.retropropagacion(i);
                    }
                    neurona.errorAprendisaje();
                    iteracionesMaximas--;
                }
                guardarSalida(neurona);
                button1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("-->" + ex);
            }
        }

        private void guardarSalida(PerceptronMulticapa neurona)
        {
            try
            {
                StreamWriter archivo = new StreamWriter("entrenamiento.txt");
                string temp = "";
                for (int i = 1; i < neurona.n.Length; i++)
                {
                    temp += " " + neurona.n[i];
                }
                archivo.WriteLine(neurona.n.Length - 1 + temp);
                archivo.WriteLine(neurona.alfa);
                archivo.WriteLine(errorMinimo);
                for (int c = 1; c <= neurona.C - 1; c++)
                {
                    for (int i = 1; i <= neurona.n[c + 1]; i++)
                    {
                        for (int j = 1; j < neurona.n[c]; j++)
                        {
                            archivo.WriteLine(neurona.w[c, j, i]);
                        }
                    }
                }
                archivo.WriteLine("\n");
                for (int c = 2; c <= neurona.C; c++)
                {
                    for (int i = 1; i <= neurona.n[c]; i++)
                    {
                        archivo.WriteLine(neurona.u[c, i]);
                    }
                }
                archivo.WriteLine("\n");
                for (int i = 0; i < neurona.numeroPatrones; i++)
                {
                    for (int j = 0; j < neurona.n[1]; j++)
                    {
                        archivo.WriteLine(neurona.s[i, j] + "\t" + neurona.y[i, j]); 
                    }
                }
                archivo.WriteLine("\n");
                archivo.WriteLine(neurona.errorEntrenamiento);
                archivo.Close();
                MessageBox.Show("Archivo creado");
            }
            catch (Exception ex)
            {

            }
        }

        private void leerTxt()
        {
            string[] datos;
            int numeroLinea = 0, posI = 0, posJ = 0;
            
            StreamReader sr = new StreamReader("grafica2.txt");
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
