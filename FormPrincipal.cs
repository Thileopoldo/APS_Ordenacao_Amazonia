using System.Diagnostics;

namespace APS_Ordenacao_Amazonia;

public sealed class FormPrincipal : Form
{
    private const int LimiteBubbleSort = 50_000;

    private readonly ComboBox _comboTamanho = new();
    private readonly ComboBox _comboEstado = new();
    private readonly Button _botaoExecutar = new();
    private readonly DataGridView _gradeResultados = new();

    public FormPrincipal()
    {
        Text = "Análise de Ordenação - Amazônia";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(640, 420);
        Size = new Size(800, 520);

        ConfigurarInterface();
    }

    private void ConfigurarInterface()
    {
        var painelControles = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            Padding = new Padding(12),
            WrapContents = true
        };

        painelControles.Controls.Add(new Label
        {
            Text = "Tamanho do vetor:",
            AutoSize = true,
            Margin = new Padding(3, 8, 3, 3)
        });

        _comboTamanho.DropDownStyle = ComboBoxStyle.DropDownList;
        _comboTamanho.Width = 110;
        _comboTamanho.Items.AddRange(new object[] { 1_000, 10_000, 50_000, 100_000 });
        _comboTamanho.SelectedIndex = 0;
        painelControles.Controls.Add(_comboTamanho);

        painelControles.Controls.Add(new Label
        {
            Text = "Estado dos dados:",
            AutoSize = true,
            Margin = new Padding(12, 8, 3, 3)
        });

        _comboEstado.DropDownStyle = ComboBoxStyle.DropDownList;
        _comboEstado.Width = 190;
        _comboEstado.Items.AddRange(Enum.GetValues<EstadoDados>().Cast<object>().ToArray());
        _comboEstado.SelectedIndex = 0;
        painelControles.Controls.Add(_comboEstado);

        _botaoExecutar.Text = "Executar Teste";
        _botaoExecutar.AutoSize = true;
        _botaoExecutar.Margin = new Padding(12, 2, 3, 3);
        _botaoExecutar.Click += BotaoExecutar_Click;
        painelControles.Controls.Add(_botaoExecutar);

        _gradeResultados.Dock = DockStyle.Fill;
        _gradeResultados.AllowUserToAddRows = false;
        _gradeResultados.AllowUserToDeleteRows = false;
        _gradeResultados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _gradeResultados.ReadOnly = true;
        _gradeResultados.RowHeadersVisible = false;
        _gradeResultados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _gradeResultados.Columns.Add("Algoritmo", "Algoritmo");
        _gradeResultados.Columns.Add("Tempo", "Tempo (ms)");

        Controls.Add(_gradeResultados);
        Controls.Add(painelControles);
    }

    private async void BotaoExecutar_Click(object? sender, EventArgs e)
    {
        int tamanho = (int)_comboTamanho.SelectedItem!;
        EstadoDados estado = (EstadoDados)_comboEstado.SelectedItem!;
        int[] dados = GeradorDados.GerarVetor(tamanho, estado);

        _botaoExecutar.Enabled = false;
        _gradeResultados.Rows.Clear();

        try
        {
            List<ResultadoOrdenacao> resultados = await Task.Run(() => ExecutarBenchmarks(dados));

            foreach (ResultadoOrdenacao resultado in resultados)
            {
                _gradeResultados.Rows.Add(resultado.Algoritmo, resultado.TempoEmMilissegundos);
            }
        }
        catch (Exception excecao)
        {
            MessageBox.Show(
                this,
                $"Não foi possível executar o teste: {excecao.Message}",
                "Erro",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        finally
        {
            _botaoExecutar.Enabled = true;
        }
    }

    private static List<ResultadoOrdenacao> ExecutarBenchmarks(int[] dados)
    {
        var resultados = new List<ResultadoOrdenacao>(3);

        if (dados.Length <= LimiteBubbleSort)
        {
            resultados.Add(Medir("Bubble Sort", dados, AlgoritmosOrdenacao.BubbleSort));
        }
        else
        {
            resultados.Add(new ResultadoOrdenacao("Bubble Sort", "Ignorado (> 50.000 elementos)"));
        }

        resultados.Add(Medir("Quick Sort", dados, AlgoritmosOrdenacao.QuickSort));
        resultados.Add(Medir("Merge Sort", dados, AlgoritmosOrdenacao.MergeSort));

        return resultados;
    }

    private static ResultadoOrdenacao Medir(
        string nome,
        int[] dados,
        Action<int[]> algoritmo)
    {
        int[] copia = (int[])dados.Clone();
        var cronometro = Stopwatch.StartNew();
        algoritmo(copia);
        cronometro.Stop();

        return new ResultadoOrdenacao(nome, cronometro.Elapsed.TotalMilliseconds.ToString("F3"));
    }

    private sealed record ResultadoOrdenacao(string Algoritmo, string TempoEmMilissegundos);
}
