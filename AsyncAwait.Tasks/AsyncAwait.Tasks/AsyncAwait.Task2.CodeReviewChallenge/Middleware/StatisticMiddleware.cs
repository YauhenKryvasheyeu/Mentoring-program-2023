using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwait.Task2.CodeReviewChallenge.Headers;
using CloudServices.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AsyncAwait.Task2.CodeReviewChallenge.Middleware;

public class StatisticMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IStatisticService _statisticService;

    public StatisticMiddleware(RequestDelegate next, IStatisticService statisticService)
    {
        _next = next;
        _statisticService = statisticService ?? throw new ArgumentNullException(nameof(statisticService));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var task = GetVisitsCountAsync(context.Request.Path);

        context.Items.Add(CustomHttpHeaders.TotalPageVisits, task);

        await _next(context);

        async Task<long> GetVisitsCountAsync(string path)
        {
            await _statisticService.RegisterVisitAsync(path);
            return await _statisticService.GetVisitsCountAsync(path);
        }
    }
}
