using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Result
    {
        public bool Correct { get; set; }
        public string MensajeError { get; set; }
        public List<object> Objetos { get; set; }
        public Object Objeto { get; set; }
        public Exception Ex { get; set; }
    }
}
