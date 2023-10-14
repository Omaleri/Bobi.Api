using Bobi.Api.Application.Domain.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Application.Contracts.DTO.ResponseModel
{
    public class VoiceResponseModel : BaseDtoModel
    {
        public DateTime VoiceDate { get; set; }
        public DateTime VoiceTime { get; set; }
        public string Link { get; set; }
        public int BuildId { get; set; }
    }
}
