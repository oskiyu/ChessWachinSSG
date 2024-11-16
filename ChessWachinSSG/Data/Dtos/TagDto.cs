using ChessWachinSSG.HTML.Tags;

namespace ChessWachinSSG.Data.Dtos {

    /// <summary>
    /// Datos de un tag externo.
    /// </summary>
    /// <param name="Tag">Texto del tag, sin llaves.</param>
    /// <param name="ReplacerType">Tipo de tag.</param>
    /// <param name="TagReplacerData">Información adicional.</param>
    public record class TagDto(string Tag, TagReplacerType ReplacerType, string TagReplacerData);

}
