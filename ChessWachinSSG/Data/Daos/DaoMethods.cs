using NLog;

using System.Text.Json;

namespace ChessWachinSSG.Data.Daos
{

    internal static class DaoMethods
    {

        /// <summary>
        /// Intenta obtener un string del JSON.
        /// </summary>
        /// <param name="key">Nombre de la propiedad que almacena el string.</param>
        /// <param name="json">Elemento JSON.</param>
        /// <param name="logger">Logger.</param>
        /// <returns>
        /// String si lo encuentra,
        /// null en caso contrario.
        /// </returns>
        public static string? GetJsonString(string key, JsonElement json, ILogger logger)
        {
            string? output = null;

            try
            {
                output = json.GetProperty(key).GetString()!;
            }
            catch (InvalidOperationException ex)
            {
                logger.Error(ex, $"La propiedad \"{key}\" no es un objeto en el archivo JSON.");
            }
            catch (KeyNotFoundException ex)
            {
                logger.Error(ex, $"La propiedad \"{key}\" no se encuentra.");
            }
            catch (ArgumentNullException ex)
            {
                logger.Error(ex, $"La propiedad \"{key}\" es null.");
            }

            return output;
        }

        /// <summary>
        /// Intenta obtener un int del JSON.
        /// </summary>
        /// <param name="key">Nombre de la propiedad que almacena el int.</param>
        /// <param name="json">Elemento JSON.</param>
        /// <param name="logger">Logger.</param>
        /// <returns>
        /// Int si lo encuentra,
        /// null en caso contrario.
        /// </returns>
        public static int? GetJsonInt(string key, JsonElement json, ILogger logger)
        {
            int? output = null;

            try
            {
                output = json.GetProperty(key).GetInt32()!;
            }
            catch (InvalidOperationException ex)
            {
                logger.Error(ex, $"La propiedad \"{key}\" no es un objeto en el archivo JSON.");
            }
            catch (KeyNotFoundException ex)
            {
                logger.Error(ex, $"La propiedad \"{key}\" no se encuentra.");
            }
            catch (ArgumentNullException ex)
            {
                logger.Error(ex, $"La propiedad \"{key}\" es null.");
            }

            return output;
        }

    }

}
