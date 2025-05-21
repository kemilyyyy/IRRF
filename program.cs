using System;

namespace CalculoIRRF
{
    class INSS
    {
        private double salarioBruto;
        private const double tetoINSS = 951.62;

        public double DescontoINSS { get; private set; }

        public INSS(double salarioBruto)
        {
            this.salarioBruto = salarioBruto;
            DescontoINSS = CalcularDescontoINSS();
        }

        private double CalcularDescontoINSS()
        {
            // Para simplificar, usa alíquota fixa de 9% com teto
            double desconto = salarioBruto * 0.09;
            return desconto > tetoINSS ? tetoINSS : desconto;
        }
    }

    class IRRF
    {
        private double salarioBase;
        private int dependentes;
        private const double deducaoPorDependente = 189.59;

        private double aliquota;
        private double deducao;

        public IRRF(double salarioBase, int dependentes)
        {
            this.salarioBase = salarioBase;
            this.dependentes = dependentes;
            (aliquota, deducao) = BuscarFaixaIRRF();
        }

        private (double, double) BuscarFaixaIRRF()
        {
            if (salarioBase <= 1903.98)
                return (0.0, 0.0);
            else if (salarioBase <= 2826.65)
                return (0.075, 142.80);
            else if (salarioBase <= 3751.05)
                return (0.15, 354.80);
            else if (salarioBase <= 4664.68)
                return (0.225, 636.13);
            else
                return (0.275, 869.36);
        }

        public double CalcularIRRF()
        {
            double deducaoTotal = deducao + (deducaoPorDependente * dependentes);
            double valorIRRF = salarioBase * aliquota - deducaoTotal;
            return valorIRRF > 0 ? valorIRRF : 0;
        }
    }

    class SalarioLiquido
    {
        private double salarioBruto;
        private int dependentes;

        public double DescontoINSS { get; private set; }
        public double SalarioBase { get; private set; }
        public double DescontoIRRF { get; private set; }
        public double ValorSalarioLiquido { get; private set; }

        public SalarioLiquido(double salarioBruto, int dependentes)
        {
            this.salarioBruto = salarioBruto;
            this.dependentes = dependentes;
            Calcular();
        }

        private void Calcular()
        {
            INSS inss = new INSS(salarioBruto);
            DescontoINSS = inss.DescontoINSS;

            SalarioBase = salarioBruto - DescontoINSS;

            IRRF irrf = new IRRF(SalarioBase, dependentes);
            DescontoIRRF = irrf.CalcularIRRF();

            ValorSalarioLiquido = salarioBruto - DescontoINSS - DescontoIRRF;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            double salarioBruto = 3000.00;
            int dependentes = 1;

            SalarioLiquido calculo = new SalarioLiquido(salarioBruto, dependentes);

            Console.WriteLine($"Salário Bruto: R$ {salarioBruto:F2}");
            Console.WriteLine($"Desconto INSS: R$ {calculo.DescontoINSS:F2}");
            Console.WriteLine($"Salário Base para IRRF: R$ {calculo.SalarioBase:F2}");
            Console.WriteLine($"Desconto IRRF: R$ {calculo.DescontoIRRF:F2}");
            Console.WriteLine($"Salário Líquido: R$ {calculo.ValorSalarioLiquido:F2}");
        }
    }
}

