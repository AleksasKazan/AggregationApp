using Contracts.Models;

namespace TestsData
{
    public static class TestData
    {
        public static List<ElectricityData> ElectricityDataList => new List<ElectricityData>
        {
            new ElectricityData{ObtPavadinimas = "Butas", Tinklas = "Alytaus regiono tinklas", PPlus = 6860.574m, PMinus = 0},
            new ElectricityData{ObtPavadinimas = "Butas", Tinklas = "Kauno regiono tinklas", PPlus = 14338.8832m, PMinus = 4543.2898m},
            new ElectricityData{ObtPavadinimas = "Butas", Tinklas = "Klaipėdos regiono tinklas", PPlus = 15176.9473m, PMinus = 1535.2952m},
            new ElectricityData{ObtPavadinimas = "Butas", Tinklas = "Panevėžio regiono tinklas", PPlus = 854.0084m, PMinus = 0},
            new ElectricityData{ObtPavadinimas = "Butas", Tinklas = "Šiaulių regiono tinklas", PPlus = 10874.5818m, PMinus = 3270.3399m},
            new ElectricityData{ObtPavadinimas = "Butas", Tinklas = "Utenos regiono tinklas", PPlus = 636.0321m, PMinus = 810.3707m},
            new ElectricityData{ObtPavadinimas = "Butas", Tinklas = "Vilniaus regiono tinklas", PPlus = 57954.189333m, PMinus = 5501.7539m}
        };

        public static List<AggregatedData> AggregatedDataList => new List<AggregatedData>
        {
            new AggregatedData{Tinklas = "Alytaus regiono tinklas",TotalRecords = 1,PPlusSum = 6860.574m,PMinusSum = 0m},
            new AggregatedData{Tinklas = "Kauno regiono tinklas",TotalRecords = 1,PPlusSum = 14338.8832m,PMinusSum = 4543.2898m},
            new AggregatedData{Tinklas = "Klaipėdos regiono tinklas",TotalRecords = 1,PPlusSum = 15176.9473m,PMinusSum = 1535.2952m},
            new AggregatedData{Tinklas = "Panevėžio regiono tinklas",TotalRecords = 1,PPlusSum = 854.0084m,PMinusSum = 0m},
            new AggregatedData{Tinklas = "Šiaulių regiono tinklas",TotalRecords = 1,PPlusSum = 10874.5818m,PMinusSum = 3270.3399m},
            new AggregatedData{Tinklas = "Utenos regiono tinklas",TotalRecords = 1,PPlusSum = 636.0321m,PMinusSum = 810.3707m},
            new AggregatedData{Tinklas = "Vilniaus regiono tinklas",TotalRecords = 1,PPlusSum = 57954.189333m,PMinusSum = 5501.7539m}
        };
    };
}