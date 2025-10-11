namespace TaxHelper.Models;

public class ParsedCsvModel
{
    public ParsedCsvModel(
        IEnumerable<string> lines,
        int sumIndex,
        int dateIndex,
        int currencyIndex)
    {
        Lines = lines;
        SumIndex = sumIndex;
        DateIndex = dateIndex;
        CurrencyIndex = currencyIndex; 
    }
    
    public IEnumerable<string> Lines { get; private set; }
    public int SumIndex { get; private set; }
    public int DateIndex { get; private set; }
    public int CurrencyIndex { get; private set; }
}