using Bobi.Api.Application.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bobi.Api.Domain.Build
{
    public class Voice : BaseEntity
    {
        public DateTime VoiceDate { get; set; }
        public DateTime VoiceTime { get; set; }
        public string Link { get; set; }
        public string BuildId { get; set; }

    }
}
