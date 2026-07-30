namespace Incapsulation.EnterpriseTask;

public class Enterprise
{
    public readonly Guid Guid;

    public string Name { get; set; }


    private string _inn = "";
    public string Inn
    {
        get => _inn;
        set
        {
            if (_inn.Length != 10 && !_inn.All(char.IsDigit))
                throw new ArgumentException();
            else _inn = value;
        }
    }

    public DateTime EstablishDate { get; set; }

    public TimeSpan ActiveTimeSpan
    {
        get => DateTime.Now - EstablishDate;
    }

    public Enterprise(Guid guid)
    {
        Guid = guid;
    }

    public double GetTotalTransactionsAmount()
    {
        DataBase.OpenConnection();
        var amount = 0.0;
        foreach (Transaction t in DataBase.Transactions().Where(z => z.EnterpriseGuid == Guid))
            amount += t.Amount;
        return amount;
    }
}