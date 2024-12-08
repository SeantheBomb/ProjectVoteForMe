using E2C;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CampaignReportLevelController : MonoBehaviour
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

        PollingResults[] results = macro.GetCurrentResults();

        data.title = "Poll Results by Neighbourhood";

        var player = new E2ChartData.Series()
        {
            name = "Player Tally",
            dataX = new List<float>(),
            dataY = new List<float>()
        };
        var opposing = new E2ChartData.Series()
        {
            name = "Opposing Tally",
            dataX = new List<float>(),
            dataY = new List<float>()
        };

        data.series.Clear();
        data.series.Add(player);
        data.series.Add(opposing);


        for (int i = 0; i < results.Length; i++)
        {
            var r = results[i];
            player.dataX.Add(i);
            player.dataY.Add(r.playerTally);

            opposing.dataX.Add(i);
            opposing.dataY.Add(r.opposingTally);
        }
        chart.UpdateChart();
    }

}
