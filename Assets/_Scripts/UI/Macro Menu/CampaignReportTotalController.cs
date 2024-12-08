using E2C;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampaignReportTotalController : MonoBehaviour
{
    E2Chart chart;

    MacroMenuController macro;

    private void Start()
    {
        chart = GetComponentInChildren<E2Chart>();
        macro = GetComponentInParent<MacroMenuController>();
        PopulateChart();
    }


    void PopulateChart()
    {
        E2ChartData data = chart.chartData;

        PollingResults result = macro.GetTotalResults();

        data.title = "Poll Results Total";

        chart.chartType = E2Chart.ChartType.PieChart;

        var player = new E2ChartData.Series()
        {
            name = "Player Tally",
            dataName = new List<string>(),
            dataX = new List<float>(),
            dataY = new List<float>()
        };
        //var opposing = new E2ChartData.Series()
        //{
        //    name = "Opposing Tally",
        //    dataX = new List<float>(),
        //    dataY = new List<float>()
        //};

        data.series.Clear();
        data.series.Add(player);
        //data.series.Add(opposing);

        player.dataName.Add("Player Tally");
        player.dataName.Add("Opposing Tally");

        var r = result;
        player.dataX.Add(0);
        player.dataY.Add(r.playerTally);

        player.dataX.Add(1);
        player.dataY.Add(r.opposingTally);

        chart.UpdateChart();
    }
}
