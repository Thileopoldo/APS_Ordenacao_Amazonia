namespace APS_Ordenacao_Amazonia;

public static class AlgoritmosOrdenacao
{
    public static void BubbleSort(int[] vetor)
    {
        ArgumentNullException.ThrowIfNull(vetor);

        for (int fim = vetor.Length - 1; fim > 0; fim--)
        {
            bool houveTroca = false;

            for (int indice = 0; indice < fim; indice++)
            {
                if (vetor[indice] > vetor[indice + 1])
                {
                    (vetor[indice], vetor[indice + 1]) = (vetor[indice + 1], vetor[indice]);
                    houveTroca = true;
                }
            }

            if (!houveTroca)
            {
                break;
            }
        }
    }

    public static void QuickSort(int[] vetor)
    {
        ArgumentNullException.ThrowIfNull(vetor);
        QuickSort(vetor, 0, vetor.Length - 1);
    }

    private static void QuickSort(int[] vetor, int inicio, int fim)
    {
        while (inicio < fim)
        {
            int pivo = Particionar(vetor, inicio, fim);

            // Processa primeiro a menor partição para limitar a profundidade da pilha.
            if (pivo - inicio < fim - pivo)
            {
                QuickSort(vetor, inicio, pivo);
                inicio = pivo + 1;
            }
            else
            {
                QuickSort(vetor, pivo + 1, fim);
                fim = pivo;
            }
        }
    }

    private static int Particionar(int[] vetor, int inicio, int fim)
    {
        int meio = inicio + (fim - inicio) / 2;
        int pivo = Mediana(vetor[inicio], vetor[meio], vetor[fim]);
        int esquerda = inicio - 1;
        int direita = fim + 1;

        while (true)
        {
            do
            {
                esquerda++;
            } while (vetor[esquerda] < pivo);

            do
            {
                direita--;
            } while (vetor[direita] > pivo);

            if (esquerda >= direita)
            {
                return direita;
            }

            (vetor[esquerda], vetor[direita]) = (vetor[direita], vetor[esquerda]);
        }
    }

    private static int Mediana(int primeiro, int segundo, int terceiro)
    {
        if (primeiro < segundo)
        {
            return segundo < terceiro ? segundo : Math.Max(primeiro, terceiro);
        }

        return primeiro < terceiro ? primeiro : Math.Max(segundo, terceiro);
    }

    public static void MergeSort(int[] vetor)
    {
        ArgumentNullException.ThrowIfNull(vetor);

        if (vetor.Length < 2)
        {
            return;
        }

        int[] auxiliar = new int[vetor.Length];
        MergeSort(vetor, auxiliar, 0, vetor.Length - 1);
    }

    private static void MergeSort(int[] vetor, int[] auxiliar, int inicio, int fim)
    {
        if (inicio >= fim)
        {
            return;
        }

        int meio = inicio + (fim - inicio) / 2;
        MergeSort(vetor, auxiliar, inicio, meio);
        MergeSort(vetor, auxiliar, meio + 1, fim);
        Intercalar(vetor, auxiliar, inicio, meio, fim);
    }

    private static void Intercalar(int[] vetor, int[] auxiliar, int inicio, int meio, int fim)
    {
        Array.Copy(vetor, inicio, auxiliar, inicio, fim - inicio + 1);

        int esquerda = inicio;
        int direita = meio + 1;

        for (int destino = inicio; destino <= fim; destino++)
        {
            if (esquerda > meio)
            {
                vetor[destino] = auxiliar[direita++];
            }
            else if (direita > fim)
            {
                vetor[destino] = auxiliar[esquerda++];
            }
            else if (auxiliar[esquerda] <= auxiliar[direita])
            {
                vetor[destino] = auxiliar[esquerda++];
            }
            else
            {
                vetor[destino] = auxiliar[direita++];
            }
        }
    }
}
