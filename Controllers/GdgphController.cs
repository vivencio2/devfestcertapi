using devfestcertapi.Models;
using devfestcertapi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore;

namespace devfestcertapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GdgphController : ControllerBase
    {        
        private readonly ILogger<GdgphController> _logger;
        private readonly IGeneratePdf _generatePdf;
        public GdgphController(ILogger<GdgphController> logger, IGeneratePdf generatePdf)
        {
            _logger = logger;
            _generatePdf = generatePdf;
        }

        [HttpGet("{ticket}")]
        public IActionResult ValidateAttendance(string ticket)
        {
            var connection = new Connection();
            var response = connection.IsTicketNumberValid(ticket);
            return new OkObjectResult(response);
        }
        [HttpGet("{ticket}/{firstName}/{lastName}")]
        public async Task<IActionResult> GenerateCertificate(string ticket, string firstName, string lastName)
        {
            var connection = new Connection();
            var response = connection.IsTicketNumberValid(ticket);
            if (response.IsValid)
            {   
                var assembly = Assembly.GetEntryAssembly();
                using (var stream = assembly.GetManifestResourceStream("devfestcertapi.PDF.template.html"))
                using (var reader = new StreamReader(stream))
                {
                    string template = await reader.ReadToEndAsync();
                    var updatedtemplate = template
                        .Replace("{attendee}", string.Format("{0} {1}", response.FirstName, response.LastName))
                        .Replace("{ticketNumber}", ticket);
                    var options = new ConvertOptions();
                    options.IsLowQuality = false;
                    options.PageMargins.Bottom = 0;
                    options.PageMargins.Left = 0;
                    options.PageMargins.Right = 0;
                    options.PageMargins.Top = 0;
                    options.PageOrientation = Wkhtmltopdf.NetCore.Options.Orientation.Landscape;
                    
                    _generatePdf.SetConvertOptions(options);
                    var pdf = _generatePdf.GetPDF(updatedtemplate);
                    Response.Headers.Add("Content-Disposition", "inline; filename=GDGPH2020Certificate.pdf");
                    return File(pdf.ToArray(), "application/pdf");
                }

            }
            return new BadRequestResult();
        }
        [HttpPut("{ticket}")]
        public IActionResult UpdateAttendee(string ticket, [FromBody]Request request)
        {
            try
            {
                var connection = new Connection();
                var success = connection.UpdateAttendeeNameByTicketNumber(ticket, request.FirstName, request.LastName);
                return new OkObjectResult(success);
            }
            catch
            {
                return new BadRequestResult();
            }
        }

    }
}
