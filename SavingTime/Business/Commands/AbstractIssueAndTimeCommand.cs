using CommandLine;
using Microsoft.Extensions.Hosting;
using SavingTime.Data;

namespace SavingTime.Business.Commands
{
    public abstract class AbstractIssueAndTimeCommand : AbstractTimeCommand
    {
        /// <summary>
        /// Propriedade abstrata que deve ser implementada pelas classes filhas
        /// para definir o campo Issue com o tipo de atributo apropriado
        /// </summary>
        public abstract string? Issue { get; set; }

        [Option('c', "comment", Required = false, HelpText = "Issue comment (use quotes for values with spaces)")]
        public string? Comment { get; set; }
    }
}
