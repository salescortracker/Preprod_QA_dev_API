using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface ICaptchaService
    {
        (string Code, byte[] ImageBytes) GenerateCaptcha();
    }
}
