using System.ComponentModel;

namespace ManagementPias.Domain.Enums;

public enum Portafolio
{
    [Description("AB LOW VOLATILITY EQUITY PORTFOLIO A USD")]
    AbLowVolatilityEquity = 1,

    [Description("AB SICAV I - AMERICAN GROWTH PORTFOLIO C EUR")]
    AbSicavIAmericanGrowth = 2,

    [Description("BNP PARIBAS FUNDS DISRUPTIVE TECHNOLOGY N EUR")]
    BnpParibasFundsDisruptiveTechnology = 3,

    [Description("AXA WF FRAMLINGTON EVOLVING TRENDS E")]
    AxaWfFramlingtonEvolvingTrends = 4,

    [Description("AB SICAV I - EUROZONE EQUITY PORTFOLIO A EUR")]
    AbSicavIEurozoneEquity = 5,

    [Description("FIDELITY WORLD E EUR")]
    FidelityWorldEEur = 6,

    [Description("AXA WF FRAMLINGTON NEXT GENERATION A EUR")]
    AxaWfFramlingtonNextGeneration = 7,

    [Description("AXA TRESOR COURT TERME EUR ACC")]
    AxaTresorCourtTermeEurAcc = 8,
}


