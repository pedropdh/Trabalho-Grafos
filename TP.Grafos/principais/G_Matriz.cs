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
        public int _quantVertices { get; }
        public int _quantArestas { get; private set; }


        public G_Matriz(int totalVertices)
        {
            _quantVertices = totalVertices;
            custos = new double[totalVertices + 1, totalVertices + 1];
            capacidades = new double[totalVertices + 1, totalVertices + 1];

            for (int i = 1; i <= totalVertices; i++)
                for (int j = 1; j <= totalVertices; j++)
                    custos[i, j] = double.PositiveInfinity;
        }


        /// <summary>
        /// Adiciona uma aresta entre os vértices informados
        /// </summary>
        /// <param name="_origem"></param>
        /// <param name="_destino"></param>
        /// <param name="custo"></param>
        /// <param name="capacidade"></param>
        public void AdicionarAresta(int _origem, int _destino, double custo, double capacidade)
        {
            custos[_origem, _destino] = custo;
            capacidades[_origem, _destino] = capacidade;
            _quantArestas++;
        }
        /// <summary>
        /// Retora uma lista com as arestas adjacentes ao vértice informado
        /// </summary>
        /// <param name="vert"></param>
        /// <returns></returns>
        public List<Aresta> ObterAdjacentes(int vert)
        {
            var lista = new List<Aresta>();

            for (int _destino = 1; _destino <= _quantVertices; _destino++)
            {
                if (custos[vert, _destino] < double.PositiveInfinity)
                {
                    lista.Add(new Aresta(vert, _destino, custos[vert, _destino], capacidades[vert, _destino]));
                }
            }

            return lista;
        }
        /// <summary>
        /// Verifica se existe uma aresta entre os vértices informados
        /// </summary>
        /// <param name="_origem"></param>
        /// <param name="_destino"></param>
        /// <returns>Retona falso se não tiver</returns>
        public bool ExisteAresta(int _origem, int _destino)
            => custos[_origem, _destino] < double.PositiveInfinity;
        /// <summary>
        /// Retorna o custo da aresta entre os vértices informados
        /// </summary>
        /// <param name="_origem"></param>
        /// <param name="_destino"></param>
        /// <returns></returns>
        public double ObterCusto(int _origem, int _destino)
            => custos[_origem, _destino];
        /// <summary>
        /// Obtem a capacidade da aresta entre os vértices informados
        /// </summary>
        /// <param name="_origem"></param>
        /// <param name="_destino"></param>
        /// <returns></returns>
        public double ObterCapacidade(int _origem, int _destino)
            => capacidades[_origem, _destino];
    }
}
