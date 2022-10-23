using Prometheus;

namespace Webapi;

public class MetricsService:BackgroundService
{
    private static MetricsHub _metricsHub = new MetricsHub();
    private Gauge _testGauge;
    private Dictionary<string, string> _dictionary = new Dictionary<string, string>();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _dictionary.Add("label","value");
        // Guage 可增加可减少的数值
        _testGauge = Metrics.CreateGauge("test_count", "测试一个随机数值",new GaugeConfiguration()
        {
            StaticLabels = _dictionary
        });
        var counter = Metrics.CreateCounter("test_count_total","新增一个只增加不减少的count");
        while (true)
        {
            if (stoppingToken.IsCancellationRequested)
                break;
            if (new Random().NextInt64() % 2 == 0)
            {
                _testGauge.Inc(1);

            }
            else
            {
                _testGauge.Dec(1);
            }
            await Task.Delay(1000);
        }
    }
}

public class MetricsHub
{
    private static Dictionary<string, Counter> _counterDictionary = new Dictionary<string, Counter>();
    private static Dictionary<string, Dictionary<string, Gauge>> _gaugeDictionary = new Dictionary<string, Dictionary<string, Gauge>>();
    public Counter GetCounter(string key)
    {
        if (_counterDictionary.ContainsKey(key))
        {
            return _counterDictionary[key];
        }
        else
        {
            return null;
        }
    }
    public Dictionary<string, Gauge> GetGauge(string key)
    {
        if (_gaugeDictionary.ContainsKey(key))
        {
            return _gaugeDictionary[key];
        }
        else
        {
            return null;
        }
    }
    public void AddCounter(string key, Counter counter)
    {
        _counterDictionary.Add(key, counter);
    }
    public void AddGauge(string key, Dictionary<string, Gauge> gauges)
    {
        _gaugeDictionary.Add(key, gauges);
    }
}