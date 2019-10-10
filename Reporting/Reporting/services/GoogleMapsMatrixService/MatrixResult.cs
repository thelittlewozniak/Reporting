using System.Collections.Generic;

namespace Reporting.services.GoogleMapsMatrixService
{
    class MatrixResult
    {
        public string status { get; set; }
        public List<string> origin_addresses { get; set; }
        public List<string> destination_addresses { get; set; }
        public List<Row> rows { get; set; }
    }
}
