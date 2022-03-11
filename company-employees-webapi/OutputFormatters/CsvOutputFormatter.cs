using System.Text;
using company_employees_shared.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace company_employees_webapi.OutputFormatters;

public class CsvOutputFormatter : TextOutputFormatter
{
    public CsvOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    protected override bool CanWriteType(Type? type)
    {
        if (typeof(CompanyDto).IsAssignableFrom(type)
            || typeof(IEnumerable<CompanyDto>).IsAssignableFrom(type))
        {
            return base.CanWriteType(type);
        }

        return false;
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var response = context.HttpContext.Response;
        var buffer = new StringBuilder();

        if (context.Object is IEnumerable<CompanyDto>)
        {
            foreach (var company in (IEnumerable<CompanyDto>)context.Object)
            {
                FormatCsv(buffer, company);
            }

        }
        else
        {
            FormatCsv(buffer, (CompanyDto?)context.Object);
        }

        await response.WriteAsync(buffer.ToString());
    }

    private void FormatCsv(StringBuilder buffer, CompanyDto? companyDto)
    {
        if (companyDto is null) return;

        buffer.AppendLine($"\"{companyDto.Id}\",\"{companyDto.Name}\",\"{companyDto.FullAddress}\"");
    }
}