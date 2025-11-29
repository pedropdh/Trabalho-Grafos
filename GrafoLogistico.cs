using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class GrafoLogistico
{
    private int numVertices;
    private int[,] capacidadeOriginal;
    private int[,] capacidadeResidual;
    private int[] parent;

    private bool BFS_EncontrarCaminhoLivre(int S, int T)
    {
        bool[] visitado = new bool[numVertices + 1];
       
        for (int i = 0; i <= numVertices; i++)
        {
            visitado[i] = false;
        }

        parent = new int[numVertices + 1];
        
        for (int i = 0; i <= numVertices; i++)
        {
            parent[i] = 0;
        }

        Queue<int> fila = new Queue<int>();
        fila.Enqueue(S);
        visitado[S] = true;
        parent[S] = -1;

        while (fila.Count != 0)
        {
            int u = fila.Dequeue();

            for (int v = 1; v <= numVertices; v++)
            {
                if (!visitado[v] && capacidadeResidual[u, v] > 0)
                {
                    fila.Enqueue(v);
                    visitado[v] = true;
                    parent[v] = u;

                    if (v == T)
                        return true;
                }
            }
        }
        return false;
    }

    public (int fluxoMaximo, List<(int u, int v)> corteMinimo) CalcularFluxoMaximo(int S, int T)
    {
        capacidadeResidual = (int[,])capacidadeOriginal.Clone();
        int maxFlow = 0;

        while (BFS_EncontrarCaminhoLivre(S, T))
        {
            int fluxoDoCaminho = int.MaxValue;
            int v = T;
            while (v != S)
            {
                int u = parent[v];
                fluxoDoCaminho = Math.Min(fluxoDoCaminho, capacidadeResidual[u, v]);
                v = u;
            }

            v = T;
            while (v != S)
            {
                int u = parent[v];
                capacidadeResidual[u, v] -= fluxoDoCaminho;
                capacidadeResidual[v, u] += fluxoDoCaminho;
                v = u;
            }

            maxFlow += fluxoDoCaminho;
        }

        List<(int u, int v)> corteMinimo = EncontrarCorteMinimo(S);

        return (maxFlow, corteMinimo);
    }

    private List<(int u, int v)> EncontrarCorteMinimo(int S)
    {
        List<(int u, int v)> arestasDoCorte = new List<(int u, int v)>();

        bool[] alcançavel = new bool[numVertices + 1];

        for (int i = 0; i <= numVertices; i++)
        {
            alcançavel[i] = false;
        }

        BuscaApenasAlcancaveis(S, alcançavel);

        for (int u = 1; u <= numVertices; u++)
        {
            for (int v = 1; v <= numVertices; v++)
            {
                if (alcançavel[u] && !alcançavel[v] && capacidadeOriginal[u, v] > 0)
                {
                    arestasDoCorte.Add((u, v));
                }
            }
        }
        return arestasDoCorte;
    }

    private void BuscaApenasAlcancaveis(int S, bool[] alcançavel)
    {
        Queue<int> fila = new Queue<int>();
        fila.Enqueue(S);
        alcançavel[S] = true;

        while (fila.Count != 0)
        {
            int u = fila.Dequeue();
            for (int v = 1; v <= numVertices; v++)
            {
                if (!alcançavel[v] && capacidadeResidual[u, v] > 0)
                {
                    alcançavel[v] = true;
                    fila.Enqueue(v);
                }
            }
        }
    }

    public string FormatarResultadoII(int S, int T, int fluxoMaximo, List<(int u, int v)> corteMinimo)
    {
        string resultadoFinal = "";

        resultadoFinal += "--- 2. Capacidade Máxima de Escoamento (Hub " + S + " -> Hub " + T + ") ---\n";

        resultadoFinal += "Fluxo Máximo (toneladas): " + fluxoMaximo + "\n";

        string cutString = "";

        foreach ((int u, int v) aresta in corteMinimo)
        {
            cutString += "(" + aresta.u + ", " + aresta.v + "), ";
        }

        if (corteMinimo.Count > 0)
        {
            cutString = cutString.Substring(0, cutString.Length - 2);
        }

        resultadoFinal += "Corte Mínimo (rotas críticas): " + cutString + "\n";

        return resultadoFinal;
    }
}