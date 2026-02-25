using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDryClean.Application.Common.Interfaces
{
    public interface IBarCodeService
    {
        byte[] GenerateCode128(string content);
    }
}
