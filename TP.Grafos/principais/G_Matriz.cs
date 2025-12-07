using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP.Grafos
{
    internal class G_Matriz : IGrafos
    {
        private double[,] custos;
        private double[,] capacidades;
        public int QuantVertices { get; }
        public int QuantArestas { get; private set; }


        public G_Matriz(int totalVertices)
        {
            QuantVertices = totalVertices;
            custos = new double[totalVertices + 1, totalVertices + 1];
            capacidades = new double[totalVertices + 1, totalVertices + 1];

            for (int i = 1; i <= totalVertices; i++)
                for (int j = 1; j <= totalVertices; j++)
                    custos[i, j] = double.PositiveInfinity;
        }


        /// <summary>
        /// Adiciona uma aresta entre os vértices informados
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        /// <param name="custo"></param>
        /// <param name="capacidade"></param>
        public void AdicionarAresta(int origem, int destino, double custo, double capacidade)
        {
            custos[origem, destino] = custo;
            capacidades[origem, destino] = capacidade;
            QuantArestas++;
        }
        /// <summary>
        /// Retora uma lista com as arestas adjacentes ao vértice informado
        /// </summary>
        /// <param name="vert"></param>
        /// <returns></returns>
        public List<Aresta> ObterAdjacentes(int vert)
        {
            var lista = new List<Aresta>();

            for (int destino = 1; destino <= QuantVertices; destino++)
            {
                if (custos[vert, destino] < double.PositiveInfinity)
                {
                    lista.Add(new Aresta(vert, destino, custos[vert, destino], capacidades[vert, destino]));
                }
            }

            return lista;
        }
        /// <summary>
        /// Verifica se existe uma aresta entre os vértices informados
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        /// <returns>Retona falso se não tiver</returns>
        public bool ExisteAresta(int origem, int destino)
            => custos[origem, destino] < double.PositiveInfinity;
        /// <summary>
        /// Retorna o custo da aresta entre os vértices informados
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        /// <returns></returns>
        public double ObterCusto(int origem, int destino)
            => custos[origem, destino];
        /// <summary>
        /// Obtem a capacidade da aresta entre os vértices informados
        /// </summary>
        /// <param name="origem"></param>
        /// <param name="destino"></param>
        /// <returns></returns>
        public double ObterCapacidade(int origem, int destino)
            => capacidades[origem, destino];
    }
}
