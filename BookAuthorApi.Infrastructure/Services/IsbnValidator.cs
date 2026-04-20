using BookAuthorApi.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace BookAuthorApi.Infrastructure.Services;

public class IsbnValidator : IIsbnValidator
{
    private readonly HttpClient _httpClient;
    private readonly string _soapBaseUrl;
    private readonly string _soapAction;

    public IsbnValidator(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _soapBaseUrl = configuration["IsbnValidator:SoapBaseUrl"] ?? "http://webservices.daehosting.com/services/isbnservice.wso?wsdl";
        _soapAction = configuration["IsbnValidator:SoapAction"] ?? "http://webservices.daehosting.com/services/IsbnService/IsValidISBN10";
    }

    public async Task<bool> IsValidIsbnAsync(string isbn)
    {
        try
        {
            // Clean the ISBN
            isbn = isbn.Replace("-", "").Replace(" ", "");

            // First do basic format validation
            if (string.IsNullOrWhiteSpace(isbn) || (isbn.Length != 10 && isbn.Length != 13))
            {
                return false;
            }

            // Call SOAP service for validation
            var soapRequest = CreateSoapRequest(isbn);
            var content = new StringContent(soapRequest, System.Text.Encoding.UTF8, "text/xml");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("SOAPAction", _soapAction);

            var response = await _httpClient.PostAsync(_soapBaseUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var soapResponse = await response.Content.ReadAsStringAsync();
                return ParseSoapResponse(soapResponse);
            }

            // Fallback to local validation if SOAP service fails
            return IsValidIsbnLocal(isbn);
        }
        catch
        {
            // Fallback to local validation if SOAP service fails
            return IsValidIsbnLocal(isbn);
        }
    }

    public bool IsValidIsbn(string isbn)
    {
        // For backward compatibility, make it synchronous by running async method
        // In a real application, you'd want to make the interface async
        var task = Task.Run(() => IsValidIsbnAsync(isbn));
        return task.GetAwaiter().GetResult();
    }

    private string CreateSoapRequest(string isbn)
    {
        return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <IsValidISBN10 xmlns=""http://webservices.daehosting.com/services"">
      <sISBN>{isbn}</sISBN>
    </IsValidISBN10>
  </soap:Body>
</soap:Envelope>";
    }

    private bool ParseSoapResponse(string soapResponse)
    {
        // Simple XML parsing - look for true in the response
        return soapResponse.Contains("<IsValidISBN10Result>true</IsValidISBN10Result>");
    }

    private bool IsValidIsbnLocal(string isbn)
    {
        // Fallback local validation
        if (isbn.Length == 10)
        {
            return IsValidIsbn10(isbn);
        }
        else if (isbn.Length == 13)
        {
            return IsValidIsbn13(isbn);
        }

        return false;
    }

    private bool IsValidIsbn10(string isbn)
    {
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            if (!char.IsDigit(isbn[i])) return false;
            sum += (isbn[i] - '0') * (10 - i);
        }
        char check = isbn[9];
        if (check == 'X') sum += 10;
        else if (char.IsDigit(check)) sum += check - '0';
        else return false;

        return sum % 11 == 0;
    }

    private bool IsValidIsbn13(string isbn)
    {
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            if (!char.IsDigit(isbn[i])) return false;
            sum += (isbn[i] - '0') * (i % 2 == 0 ? 1 : 3);
        }
        int check = (10 - (sum % 10)) % 10;
        return check == (isbn[12] - '0');
    }
}