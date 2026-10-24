using System.Globalization;

namespace APS_Ordenacao_Amazonia;

public enum EstadoDados
{
    Aleatorio,
    Ordenado,
    InversamenteOrdenado
}

public static class GeradorDados
{
    public static int[] GerarVetor(int tamanho, EstadoDados estado)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(tamanho);

        if (!Enum.IsDefined(estado))
        {
            throw new ArgumentOutOfRangeException(nameof(estado), estado, "Estado dos dados inválido.");
        }

        int[] vetor = new int[tamanho];

        for (int indice = 0; indice < tamanho; indice++)
        {
            vetor[indice] = indice + 1;
        }

        switch (estado)
        {
            case EstadoDados.Aleatorio:
                for (int indice = vetor.Length - 1; indice > 0; indice--)
                {
                    int outroIndice = Random.Shared.Next(indice + 1);
                    (vetor[indice], vetor[outroIndice]) = (vetor[outroIndice], vetor[indice]);
                }

                break;

            case EstadoDados.InversamenteOrdenado:
                Array.Reverse(vetor);
                break;
        }

        return vetor;
    }

    public static int[] CarregarDeArquivo(string caminhoArquivo)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(caminhoArquivo);

        string conteudo = File.ReadAllText(caminhoArquivo);
        string[] valores = conteudo.Split(
            new[] { ',', ';', ' ', '\t', '\r', '\n' },
            StringSplitOptions.RemoveEmptyEntries);
        int[] vetor = new int[valores.Length];

        for (int indice = 0; indice < valores.Length; indice++)
        {
            if (!int.TryParse(valores[indice], NumberStyles.Integer, CultureInfo.InvariantCulture, out vetor[indice]))
            {
                throw new FormatException(
                    $"O valor '{valores[indice]}' na posição {indice + 1} não é um número inteiro válido.");
            }
        }

        return vetor;
    }
}
